import {HoistModel, managed} from '@xh/hoist/core';
import {makeObservable} from '@xh/hoist/mobx';
import {CustomersModel} from './customers/CustomersModel';
import {OrdersModel} from './orders/OrdersModel';

export class CustomersTabModel extends HoistModel {
    @managed customersModel = new CustomersModel();
    @managed ordersModel = new OrdersModel();

    constructor() {
        super();
        makeObservable(this);

        this.addReaction({
            track: () => this.customersModel.selectedCustomer,
            run: customer => {
                this.ordersModel.customer = customer;
            }
        });
    }
}
