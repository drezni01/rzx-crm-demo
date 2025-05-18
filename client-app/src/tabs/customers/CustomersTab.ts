import {creates, hoistCmp} from '@xh/hoist/core';
import {CustomersTabModel} from './CustomersTabModel';
import {hframe} from '@xh/hoist/cmp/layout';
import {customers} from './customers/Customers';
import {orders} from './orders/Orders';

export const customersTab = hoistCmp.factory<CustomersTabModel>({
    model: creates(CustomersTabModel),
    render({model}) {
        return hframe(customers({model: model.customersModel}), orders({model: model.ordersModel}));
    }
});
