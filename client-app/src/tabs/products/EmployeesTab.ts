import {creates, hoistCmp} from '@xh/hoist/core';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {grid} from '@xh/hoist/cmp/grid';
import {ProductsTabModel} from './ProductsTabModel';

export const productsTab = hoistCmp.factory({
    model: creates(ProductsTabModel),
    render({model}) {
        return panel({
            item: grid(),
            mask: 'onLoad'
        });
    }
});
