package io.hoistapp

import grails.gorm.transactions.Transactional
import io.xh.hoist.util.Utils
import io.hoistapp.user.User

import static io.xh.hoist.BaseService.parallelInit
import java.time.LocalDate

import static io.xh.hoist.util.InstanceConfigUtils.getInstanceConfig
import static io.xh.hoist.util.Utils.*

class BootStrap {

    def init = {servletContext ->
        logStartupMsg()
       
        def services = xhServices.findAll {
            it.class.canonicalName.startsWith(this.class.package.name)
        }
        parallelInit(services)

        createLocalAdminUserIfNeeded()
    }

    def destroy = {}

    //------------------------
    // Implementation
    //------------------------
    @Transactional
    private void createLocalAdminUserIfNeeded() {
        String adminUsername = getInstanceConfig('adminUsername')
        String adminPassword = getInstanceConfig('adminPassword')

        if (adminUsername && adminPassword) {
            def user = User.findByEmail(adminUsername)
            if (!user) {
                new User(
                    email: adminUsername,
                    password: adminPassword,
                    name: 'hoistapp Admin',
                    profilePicUrl: 'admin-profile-pic.png'
                ).save()
            } else if (!user.checkPassword(adminPassword)) {
                user.password = adminPassword
                user.save()
            }

            log.info("Local admin user available as per instanceConfig | $adminUsername")

            Utils.configService.ensureRequiredConfigsCreated(
                roles: [
                    valueType: 'json',
                    defaultValue: ['HOIST_ADMIN': [adminUsername], 'APP_READER': [adminUsername]]
                ]
            )
        } else {
            log.warn("Default admin user not created. To provide admin access, specify credentials in a hoistapp.yml instance config file.")
        }
    }

    private void logStartupMsg() {
        log.info("""
\n                                                                         
         ${appName} v${appVersion} - ${appEnvironment}
        """)
    }
}
