const configureWebpack = require('@xh/hoist-dev-utils/configureWebpack'),
    path = require('path');

const customPkgPath = path.resolve('node_modules/@xh/package-template');

module.exports = (env = {}) => {
    return configureWebpack({
        appCode: 'CRM',
        appName: 'CRM',
        appVersion: '0.0.1',
        favicon: './public/favicon.svg',
        devServerOpenPage: 'app/',
        sourceMaps: 'devOnly',
        // Use React prod mode, primarily to avoid console warnings for react 18
        reactProdMode: false,
        babelIncludePaths: [customPkgPath],
        ...env
    });
};
