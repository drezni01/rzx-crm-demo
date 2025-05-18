import {AboutDialogItem, HoistAppModel, LoadSpec, managed, XH} from '@xh/hoist/core';
import {Route} from 'router5';
import {EmployeeService} from './svc/EmployeeService';
import {ProductService} from './svc/ProductService';
import {CustomerService} from './svc/CustomerService';
import {TabContainerModel, TabModel} from '@xh/hoist/cmp/tab';
import {customersTab} from './tabs/customers/CustomersTab';
import {productsTab} from './tabs/products/EmployeesTab';
import {employeesTab} from './tabs/employees/EmployeesTab';
import {OrderService} from './svc/OrderService';
import {MessageHub} from './svc/MessageHub';
import {wsIndicator} from './cmp/wsIndicator/WsIndicator';
import {Icon} from '@xh/hoist/icon';
import {action, bindable, makeObservable, observable} from '@xh/hoist/mobx';
import {getRandomUser} from './cmp/login/FakeLogin';

export class CrmAppModel extends HoistAppModel {
    @managed tabModel = this.createTabModel();
    @observable showLogin = true;
    @observable userId: number;

    constructor() {
        super();
        makeObservable(this);
    }

    override async initAsync(): Promise<void> {
        await XH.installServicesAsync(MessageHub);
        await XH.installServicesAsync(EmployeeService, ProductService, CustomerService);
        await XH.installServicesAsync(OrderService);

        this.userId = getRandomUser();
    }

    override getRoutes(): Route[] {
        return [
            {
                name: 'default',
                path: '/app',
                forwardTo: 'default.customers',
                children: [
                    {name: 'customers', path: '/customers'},
                    {name: 'products', path: '/products'},
                    {name: 'employees', path: '/employees'}
                ]
            }
        ];
    }

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        await Promise.all([
            XH.employeeService.refreshAsync(),
            XH.productService.refreshAsync(),
            XH.customerService.refreshAsync()
        ]);
        await XH.orderService.refreshAsync();
    }

    override getAboutDialogItems(): AboutDialogItem[] {
        let builtIns = super.getAboutDialogItems();
        builtIns = builtIns.filter(item => item.label !== 'WebSockets');

        return [
            ...builtIns,
            {
                label: 'CRM WebSockets',
                value: wsIndicator({showDescription: true})
            }
        ];
    }

    @action login() {
        this.showLogin = false;
    }

    private createTabModel(): TabContainerModel {
        return new TabContainerModel({
            route: 'default',
            switcher: false,
            tabs: [
                {id: 'customers', content: customersTab, icon: Icon.users()},
                {
                    id: 'products',
                    content: productsTab,
                    icon: Icon.icon({iconName: 'gift', prefix: 'fal'})
                },
                {
                    id: 'employees',
                    content: employeesTab,
                    icon: Icon.icon({iconName: 'user-tie', prefix: 'fal'})
                }
            ]
        });
    }
}
