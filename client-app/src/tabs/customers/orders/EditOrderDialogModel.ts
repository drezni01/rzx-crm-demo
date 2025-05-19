import {HoistModel, managed, TaskObserver, XH} from '@xh/hoist/core';
import {action, makeObservable, observable} from '@xh/hoist/mobx';
import {FormModel} from '@xh/hoist/cmp/form';
import {lengthIs, numberIs, required} from '@xh/hoist/data';
import {asServerException, Order} from '../../../data/DataTypes';

export class EditOrderDialogModel extends HoistModel {
    @observable isOpen = false;
    @observable isEditMode = false;
    @managed formModel = this.createFormModel();

    constructor() {
        super();
        makeObservable(this);
    }

    @action open(order: Order) {
        this.isEditMode = !!order;
        this.formModel.init(order || {});
        this.formModel.validateAsync();
        this.isOpen = true;
    }

    @action close() {
        this.isOpen = false;
    }

    async saveAsync() {
        try {
            const orderData = this.formModel.getData() as Order;

            if (orderData.orderId) {
                await XH.orderService.updateOrderAsync(orderData);
            } else {
                orderData.orderId = 0;
                orderData.timestamp = new Date().toISOString();
                await XH.orderService.addOrderAsync(orderData);
            }

            this.close();
        } catch (e) {
            XH.handleException(asServerException(e));
        }
    }

    private createFormModel(): FormModel {
        return new FormModel({
            fields: [
                {name: 'orderId'},
                {name: 'customerId', displayName: 'Customer', disabled: true, rules: [required]},
                {name: 'productId', displayName: 'Product', rules: [required]},
                {
                    name: 'salesPersonId',
                    displayName: 'Sales Person',
                    disabled: true,
                    rules: [required]
                },
                {name: 'quantity', rules: [required, numberIs({min: 1})]},
                {name: 'timestamp'}
            ]
        });
    }
}
