import {hoistCmp, XH} from '@xh/hoist/core';
import {CustomersModel} from './CustomersModel';
import {div, filler, fragment, hbox} from '@xh/hoist/cmp/layout';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {editCustomerDialog} from './EditCustomerDialog';
import {toolbar} from '@xh/hoist/desktop/cmp/toolbar';
import {storeFilterField} from '@xh/hoist/cmp/store';
import {button} from '@xh/hoist/desktop/cmp/button';
import {Icon} from '@xh/hoist/icon';
import {Customer} from '../../../data/DataTypes';
import {badge} from '@xh/hoist/cmp/badge';
import {dataView} from '@xh/hoist/cmp/dataview';
import './Customers.scss';

export const customers = hoistCmp.factory<CustomersModel>(({model}) =>
    fragment(
        panel({
            modelConfig: model.panelConfig,
            tbar: tbar(),
            item: dataView(),
            mask: [XH.customerService.loadTask, XH.customerService.deleteTask]
        }),
        editCustomerDialog({model: model.editDialog})
    )
);

export const tbar = hoistCmp.factory<CustomersModel>(({model}) =>
    toolbar(
        storeFilterField({store: model.customerView.store, matchMode: 'any'}),
        button({
            icon: Icon.icon({iconName: 'user-plus', prefix: 'fal'}),
            tooltip: 'Add new',
            intent: 'success',
            onClick: () => model.add()
        }),
        button({
            icon: Icon.icon({iconName: 'user-pen', prefix: 'fal'}),
            tooltip: 'Edit',
            intent: 'primary',
            disabled: !model.selectedCustomer,
            onClick: () => model.edit()
        }),
        button({
            icon: Icon.icon({iconName: 'user-xmark', prefix: 'fal'}),
            tooltip: 'Delete',
            disabled: !model.selectedCustomer,
            intent: 'danger',
            onClick: () => model.delete()
        })
    )
);

export const customerCard = (c: Customer) =>
    hbox({
        className: 'customer-card',
        items: [name(c), filler(), count(c)]
    });

const name = (c: Customer) =>
    div(`${c.lastName}, ${c.firstName} ${c.middleInitial ? c.middleInitial + '.' : ''}`);

const count = (c: Customer) => badge({omit: !c.orderCount, item: c.orderCount});
