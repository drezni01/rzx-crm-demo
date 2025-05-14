# Bare-bone starter framework, created by stripping down the Toolkit sample app
You can customize the codebase by replacing "hoistapp" with an appropriate appCode (in the client-app, grails-app, and loose files/scripts

Please refer to the [Hoist Core](https://github.com/xh/hoist-core) and
[Hoist React](https://github.com/xh/hoist-react) repos for detailed information and documentation on
Hoist, including more information on configuring a local development environment.

## Configuration
### Instance Configuration
Hoist applications can read low-level, instance-specific information from a YML configuration file
on the local machine where they run. This is used primarily to set database/other credentials, which we
don't wish to check in to source control but which are required to connect to the DB

* Create a new instance config file - the default location is `/etc/hoist/conf/hoistapp.yml`.
  * If you don't wish to create that directory structure under `/etc/`, you can place the same file
    elsewhere and point the server there with a JavaOpt - see the hoist-core provided
    [`InstanceConfigUtils.groovy`](https://github.com/xh/hoist-core/blob/develop/src/main/groovy/io/xh/hoist/util/InstanceConfigUtils.groovy)
    for details.
* The contents of this file (for hoistapp) will typically be as follows:

```
environment: Development
serverURL: http://localhost

# For first run of the project before you have OAuth set up. 
useOAuth: false
adminUsername: admin@xh.io 
adminPassword: "a password of your choice"

# Enable in memory h2 database option. When ready, configure proper DB below and set to false
useH2: true

# DB config - provide either root or a dedicated local account, if using.
dbHost: localhost:3306
dbSchema: hoistapp
dbUser: root
dbPassword: "your database user password" 
dbCreate: update
```

When running the hoistapp server, look for a message along the lines of "Loaded 10 instanceConfigs from /etc/hoist/conf/ hoistapp.yml" to be logged to the console early on in the startup process.  This will indicate that Hoist has successfully  read your config.

### Database
* For initial/test usage, hoistapp is configured to  use an in-memory H2 database that will be 
  rebuilt at startup of the app. This is governed by the `useH2`.
* For persistent deployments, hoistapp is designed to work with MySQL 5.x.* or MS SQL Server.  This is governed by the `dbType` = mssql | mysql.
* Create a new empty database named `hoistapp`, being sure to use a UTF8 charset..
* For local development, use of the `root` account is fine, or you can create a local user and
  password dedicated to hoistapp. If using a non-root account, ensure that the user has DBO rights on
  the new database.
* If the server is started against an empty database, Grails will auto-create the required schemas
  on first run as long as a suitable value is provided for the `dbCreate` data source parameter. See
  `grails-app/conf/runtime.groovy` for where this is set - we leave hoistapp on `update` to allow for
  automatic schema changes as needed.


### Authentication
Note that hoistapp uses 'Auth0' as its authentication provider, and includes various client-side and
server side adaptors for that. Typical Hoist applications will use an enterprise-specific Single
Sign-On (SSO) such as JESPA/NTLM, kerberos, or another OAuth based solution.

auth0 keys are configured in `/etc/hoist/conf/hoistapp.yml` as
```
useOAuth: true
auth0ClientId: X8D...
auth0Domain: dev-sgl...xyz.us.auth0.com
auth0Jwks: '{"keys":[{"alg":"RS256",...}'
```
in `Auth0Service.groovy` `getInstanceConfig('')` is used to get the values, could be switched to `configService.getString('')` and configure in admin UI

the url to get Json Web Key Set from Auth0 is 'https://<auth0Domain>/.well-known/jwks.json'

## Run
* grails server: `gradlew bootRun` (in project root)
* React UI: `yarn start` (in client-app)

## Build
* `build.bat` contains commands to copy app files and configs to nginx (paths hardcoded)
* nginx should run on the port configured in callback url(s) in auth0
* nginx -c conf/hoistapp.conf
* nginx -s reload
* nginx -s stop
	

## Breaking code changes vs. Toolkitpackages changed from io.xh.toolbox.xyz to io.hoistapp.xyz
* UserService.getOrCreateFromJwtResult() notifyOnUserCreated is disabled
* domain.User.constraints email constraints are relaxed to allow usernames (email without @xyz) to be used
* RoleService is modified to support new scheme users/groups/inherits
* MS Sql Server support is in DBConfig.groovy and build.gradle dependencies{} `runtimeOnly "mysql:mysql-connector-java:8.0.29"` or `runtimeOnly "net.sourceforge.jtds:jtds:1.3.1"`
* Please specify your expected timezone in the `xhExpectedServerTimeZone` config

## TO DO
* customize build versions in build script