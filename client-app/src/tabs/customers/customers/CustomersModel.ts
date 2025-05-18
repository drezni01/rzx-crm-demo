import {HoistModel, managed, XH} from '@xh/hoist/core';
import {makeObservable} from '@xh/hoist/mobx';
import {EditCustomerDialogModel} from './EditCustomerDialogModel';
import {PanelConfig} from '@xh/hoist/desktop/cmp/panel';
import {Customer} from '../../../data/DataTypes';
import {DataViewModel} from '@xh/hoist/cmp/dataview';
import {customerCard} from './Customers';
import {Icon} from '@xh/hoist/icon';

export class CustomersModel extends HoistModel {
    @managed customerView = this.createViewModel();
    @managed editDialog = new EditCustomerDialogModel();
    @managed panelConfig: PanelConfig = {
        side: 'left',
        defaultSize: 300,
        minSize: 300,
        showSplitter: true,
        resizable: true,
        collapsible: false
    };

    constructor() {
        super();
        makeObservable(this);

        this.addReaction({
            track: () => XH.customerService.customers,
            run: customers => this.customerView.loadData(customers),
            fireImmediately: true
        });
    }

    get selectedCustomer(): Customer {
        return this.customerView.selectedRecord?.raw as Customer;
    }

    add() {
        this.editDialog.open(null);
    }

    edit() {
        this.editDialog.open(this.selectedCustomer);
    }

    async delete() {
        const confirm = await XH.confirm({
            title: 'Delete Customer',
            icon: Icon.questionCircle(),
            message: `Are you sure you want to delete ${this.selectedCustomer.firstName} ${this.selectedCustomer.lastName}?`
        });
        if (!confirm) return;

        await XH.customerService.deleteCustomerAsync(this.selectedCustomer.customerId);
    }

    private createViewModel(): DataViewModel {
        return new DataViewModel({
            store: {
                fields: ['customerId', 'firstName', 'lastName', 'middleInitial', 'orderCount'],
                idSpec: 'customerId'
            },
            renderer: (v, {record}) => customerCard(record.raw as Customer),
            onRowDoubleClicked: () => this.edit(),
            sortBy: 'lastName',
            itemHeight: 40,
            selModel: 'single',
            hideEmptyTextBeforeLoad: true,
            rowBorders: true,
            showHover: false,
            stripeRows: true
        });
    }
}
