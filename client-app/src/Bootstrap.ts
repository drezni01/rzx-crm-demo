import {ModuleRegistry} from '@ag-grid-community/core';
import {AgGridReact} from '@ag-grid-community/react';
import {ClientSideRowModelModule} from '@ag-grid-community/client-side-row-model';
import {installAgGrid} from '@xh/hoist/kit/ag-grid';
import {EnterpriseCoreModule, LicenseManager} from '@ag-grid-enterprise/core';
import {MenuModule} from '@ag-grid-enterprise/menu';
import {RowGroupingModule} from '@ag-grid-enterprise/row-grouping';
import {ClipboardModule} from '@ag-grid-enterprise/clipboard';
import '@ag-grid-community/styles/ag-grid.css';
import '@ag-grid-community/styles/ag-theme-balham.css';
import Highcharts from 'highcharts/highstock';
import {installHighcharts} from '@xh/hoist/kit/highcharts';
import {EmployeeService} from './svc/EmployeeService';
import {ProductService} from './svc/ProductService';
import {CustomerService} from './svc/CustomerService';
import {OrderService} from './svc/OrderService';
import {MessageHub} from './svc/MessageHub';

declare module '@xh/hoist/core' {
    export interface XHApi {
        messageHub: MessageHub;
        employeeService: EmployeeService;
        productService: ProductService;
        customerService: CustomerService;
        orderService: OrderService;
    }
    // @ts-ignore
    export const XH: XHApi;
}

ModuleRegistry.registerModules([
    ClientSideRowModelModule,
    ClipboardModule,
    MenuModule,
    RowGroupingModule
]);
installAgGrid(AgGridReact, EnterpriseCoreModule.version);

installHighcharts(Highcharts);

import {library} from '@fortawesome/fontawesome-svg-core';
import {
    faGift,
    faUserTie,
    faUserPlus,
    faUserPen,
    faUserXmark,
    faGrid2Plus,
    faInputNumeric,
    faShuffle
} from '@fortawesome/pro-light-svg-icons';
library.add(
    faGift,
    faUserTie,
    faUserPlus,
    faUserPen,
    faUserXmark,
    faGrid2Plus,
    faInputNumeric,
    faShuffle
);
