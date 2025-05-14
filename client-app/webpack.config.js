const configureWebpack = require('@xh/hoist-dev-utils/configureWebpack');

module.exports = (env = {}) => {
    return configureWebpack({
        appCode: 'hoistapp',
        appName: 'Hoist App',
        appVersion: env.appVersion || '4.0-SNAPSHOT',
        favicon: './public/favicon.svg',
        devServerOpenPage: 'app/',
        sourceMaps: 'devOnly',
        // Use React prod mode, primarily to avoid console warnings for react 18
        reactProdMode: false,
        ...env
    });
};
