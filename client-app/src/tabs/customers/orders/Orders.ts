import {hoistCmp, XH} from '@xh/hoist/core';
import {OrdersModel} from './OrdersModel';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {grid} from '@xh/hoist/cmp/grid';
import {toolbar} from '@xh/hoist/desktop/cmp/toolbar';
import {button} from '@xh/hoist/desktop/cmp/button';
import {Icon} from '@xh/hoist/icon';
import {fragment, hbox, hspacer, span} from '@xh/hoist/cmp/layout';
import {editOrderDialog} from './EditOrderDialog';
import './Orders.scss';

export const orders = hoistCmp.factory<OrdersModel>(({model}) =>
    fragment(
        panel({
            tbar: tbar(),
            item: grid(),
            mask: [XH.orderService.loadTask, XH.orderService.deleteTask]
        }),
        editOrderDialog({model: model.orderEditor})
    )
);

const tbar = hoistCmp.factory<OrdersModel>(({model}) =>
    toolbar(
        button({
            text: 'New',
            icon: Icon.plus(),
            intent: 'success',
            disabled: !model.customer,
            onClick: () => model.addOrder()
        }),
        button({
            text: 'Clone',
            icon: Icon.copy(),
            disabled: !model.selectedOrder,
            onClick: () => model.cloneOrder()
        }),
        button({
            text: 'Edit',
            icon: Icon.edit(),
            intent: 'primary',
            disabled: !model.selectedOrder,
            onClick: () => model.editOrder()
        }),
        button({
            text: 'Delete',
            icon: Icon.trash(),
            intent: 'danger',
            disabled: !model.selectedOrder,
            onClick: () => model.deleteOrder()
        })
    )
);

export enum HintTypeEnum {
    NO_CUSTOMER,
    NO_ORDERS
}

export const ordersHint = (type: HintTypeEnum): React.ReactNode => {
    if (type == HintTypeEnum.NO_CUSTOMER)
        return hbox({
            className: 'orders-hint',
            items: [Icon.user(), hspacer(5), span('Select customer...')]
        });
    if (type == HintTypeEnum.NO_ORDERS)
        return hbox({
            className: 'orders-hint',
            items: [
                Icon.icon({iconName: 'grid-2-plus', prefix: 'fal'}),
                hspacer(5),
                span('Add orders...')
            ]
        });
};
