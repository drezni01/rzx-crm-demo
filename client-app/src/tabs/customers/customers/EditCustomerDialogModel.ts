import {HoistModel, managed, TaskObserver, XH} from '@xh/hoist/core';
import {action, makeObservable, observable} from '@xh/hoist/mobx';
import {Customer} from '../../../data/DataTypes';
import {FormModel} from '@xh/hoist/cmp/form';
import {lengthIs, required} from '@xh/hoist/data';

export class EditCustomerDialogModel extends HoistModel {
    @observable isOpen = false;
    @observable isEditMode = false;
    @managed formModel = this.createFormModel();

    constructor() {
        super();
        makeObservable(this);
    }

    @action open(customer: Customer) {
        this.isEditMode = !!customer;
        this.formModel.init(customer || {});
        this.formModel.validateAsync();
        this.isOpen = true;
    }

    @action close() {
        this.isOpen = false;
    }

    async saveAsync() {
        try {
            const customerData = this.formModel.getData() as Customer;

            if (customerData.customerId) {
                await XH.customerService.updateCustomerAsync(customerData);
            } else {
                customerData.customerId = 0;
                customerData.timestamp = new Date().toISOString();
                await XH.customerService.addCustomerAsync(customerData);
            }

            this.close();
        } catch (e) {
            XH.handleException(e);
        }
    }

    private createFormModel(): FormModel {
        return new FormModel({
            fields: [
                {name: 'customerId'},
                {name: 'firstName', rules: [required]},
                {name: 'lastName', rules: [required]},
                {name: 'middleInitial', rules: [lengthIs({max: 1})]},
                {name: 'timestamp'}
            ]
        });
    }
}
