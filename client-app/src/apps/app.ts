import '../Bootstrap';

import {XH} from '@xh/hoist/core';
import {AppContainer} from '@xh/hoist/desktop/appcontainer';
import {CrmApp} from '../CrmApp';
import {CrmAppModel} from '../CrmAppModel';

XH.renderApp({
    clientAppCode: 'CRM',
    clientAppName: 'CRM',
    componentClass: CrmApp,
    modelClass: CrmAppModel,
    containerClass: AppContainer,
    isMobileApp: false,
    enableLogout: false,
    webSocketsEnabled: false,
    checkAccess: 'USER'
});
