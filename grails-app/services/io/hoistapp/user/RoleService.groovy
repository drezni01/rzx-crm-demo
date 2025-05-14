package io.hoistapp.user

import io.xh.hoist.user.BaseRoleService
import java.util.concurrent.ConcurrentHashMap

import static io.xh.hoist.util.DateTimeUtils.getMINUTES


/**
 * Resolves user -> role memberships based on mappings in the required `roles` AppConfig entry.
 * This configuration entry is a JSON object where each key represents a role to be configured,
 * and each value contains one or more of the following properties (all string arrays):
 *
 *      users - list of "plain" usernames (e.g. jsmith) to add directly to the role.
 *              Useful when suitable AD groups not available or simple direct listings are desired.
 *      groups - list of Active Directory group DNs whose members should be added to the role.
 *              Group membership will be resolved recursively - i.e. members of nested groups
 *              will be included.
 *      inherits - list of other roles to also be applied to all members of the role.
 *              Note that inherited roles do not need to have top-level keys - i.e. a role may
 *              be defined and assigned to users through inheritance only. Role inheritance is also
 *              resolved recursively within this service.
 *
 * This service meets the Hoist Core requirement for a concrete implementation of BaseRoleService,
 * and will be called into by ElliottUser objects to return their role memberships on the fly.
 */
class RoleService extends BaseRoleService {

    def configService,
        ldapService

    static clearCachesConfigs = ['roles']

    private Map<String, Set<String>> _rolesToUsers

    void init() {
        _rolesToUsers = new ConcurrentHashMap()

        createTimer(
            interval: 'rolesReloadIntervalMins',
            intervalUnits: MINUTES,
            runFn: this.&load,
            runImmediatelyAndBlock: true
        )

        super.init()
    }

    Map<String, Set<String>> getAllRoleAssignments() {
        return _rolesToUsers
    }


    //----------------------------------
    // Implementation
    //----------------------------------
    private void load() {
        Map<String, Set<String>> rolesToUsers = (new ConcurrentHashMap()).withDefault{new HashSet()}

        def mappings = getRoleMappings()
        withDebug("Processing role membership for ${mappings.size()} configured mappings.") {

            // Determine set of LDAP groups configured for role mappings.
            // De-duplicate for more efficient resolution of members via ldapService.
            Set<String> allGroups = new HashSet()
            mappings.values().each {roleConfig ->
                def groups = (roleConfig.groups ?: []) as Set<String>
                allGroups.addAll(groups)
            }

            // Ask LDAP service for a map of all groups to their members.
            if (allGroups && !ldapService.enabled) {
                log.warn("Role mappings include AD groups, but LDAP services are not fully configured.")
            }

            def groupsToMembers = allGroups ? ldapService.lookupGroupMembers(allGroups) : [:]

            // Loop over role mappings and populate role sets by user.
            mappings.each {parentRole, roleConfig ->
                def users = (roleConfig.users ?: []) as Set<String>,
                    groups = (roleConfig.groups ?: []) as Set<String>,
                    roleAndInheritedRoles = resolveInheritedRoles(parentRole)

                log.debug("Processing role ${parentRole} - resolved to [${roleAndInheritedRoles.join(',')}]")

                roleAndInheritedRoles.each{role ->
                    def roleUsers = rolesToUsers[role]

                    // Process individual users, if any.
                    log.debug("Found ${users.size()} users configured for membership in ${role}")
                    users.each {username ->
                        roleUsers.add(username.toLowerCase().trim())
                    }

                    // Process LDAP groups, if any, using already resolved member sets.
                    log.debug("Found ${groups.size()} groups configured for membership in ${role}")
                    groups.each {group ->
                        def groupMembers = groupsToMembers[group]
                        groupMembers.each {ldapUser ->
                            String username = ldapUser.samaccountname.toLowerCase()
                            roleUsers.add(username)
                        }
                    }
                }
            }

            // Flush into local cache map
            _rolesToUsers = rolesToUsers
        }
    }

    // Returns roles inherited by the given role, looked up recursively and including the given role itself
    private Set<String> resolveInheritedRoles(String role, Set<String> resolvedRoles = []) {
        def mappings = getRoleMappings(),
            roleConf = mappings[role]

        resolvedRoles << role
        if (!roleConf || !roleConf.inherits) return resolvedRoles

        roleConf.inherits.each{String inheritedRole ->
            if (!resolvedRoles.contains(inheritedRole)) {
                resolvedRoles.addAll(resolveInheritedRoles(inheritedRole, resolvedRoles))
            }
        }

        return resolvedRoles
    }

    private Map<String, Map> getRoleMappings() {
        return configService.getMap('roles')
    }

    void clearCaches() {
        load()
    }
}