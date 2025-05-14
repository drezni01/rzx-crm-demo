import '../Bootstrap';

import {XH} from '@xh/hoist/core';
import {AppContainer} from '@xh/hoist/desktop/appcontainer';
import {App} from '../app/App';
import {AppModel} from '../app/AppModel';

XH.renderApp({
    clientAppCode: 'app',
    clientAppName: 'Hoist App',
    componentClass: App,
    modelClass: AppModel,
    containerClass: AppContainer,
    isMobileApp: false,
    isSSO: true,
    webSocketsEnabled: true,
    checkAccess: 'USER'
});
