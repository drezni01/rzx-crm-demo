package io.hoistapp.security

import io.xh.hoist.security.AccessAll
import io.hoistapp.BaseController

/**
 * Controller, accessible pre-auth via AuthenticationService whitelist, to allow soft-config of
 * Auth0/Oauth related settings on the client.
 */
@AccessAll
class OauthConfigController extends BaseController {

    def auth0Service

    def index() {
        renderJSON(auth0Service.clientConfig)
    }

}
