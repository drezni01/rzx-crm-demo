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
LicenseManager.setLicenseKey(
    'Using_this_AG_Grid_Enterprise_key_( AG-041262 )_in_excess_of_the_licence_granted_is_not_permitted___Please_report_misuse_to_( legal@ag-grid.com )___For_help_with_changing_this_key_please_contact_( info@ag-grid.com )___( Extremely Heavy Industries Inc. )_is_granted_a_( Single Application )_Developer_License_for_the_application_( Toolbox )_only_for_( 6 )_Front-End_JavaScript_developers___All_Front-End_JavaScript_developers_working_on_( Toolbox )_need_to_be_licensed___( Toolbox )_has_been_granted_a_Deployment_License_Add-on_for_( 1 )_Production_Environment___This_key_works_with_AG_Grid_Enterprise_versions_released_before_( 3 June 2024 )____[v2]_MTcxNzM2OTIwMDAwMA==a7351c8195af47efc83e07db2a5f2c88'
);

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
