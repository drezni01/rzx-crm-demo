import '../Bootstrap';

import {XH} from '@xh/hoist/core';
import {AppComponent} from '@xh/hoist/admin/AppComponent';
import {AppModel} from '@xh/hoist/admin/AppModel';
import {AppContainer} from '@xh/hoist/desktop/appcontainer';

XH.renderApp({
    clientAppCode: 'admin',
    clientAppName: 'Hoist App Admin',
    componentClass: AppComponent,
    modelClass: AppModel,
    containerClass: AppContainer,
    isMobileApp: false,
    checkAccess: 'HOIST_ADMIN_READER'
});
