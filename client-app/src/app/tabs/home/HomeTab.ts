import {hoistCmp} from '@xh/hoist/core';
import {creates} from '@xh/hoist/core';
import {vframe} from '@xh/hoist/cmp/layout';
import {span} from '@xh/hoist/cmp/layout';
import { HomeTabModel } from './HomeTabModel';

export const homeTab = hoistCmp.factory({
    model: creates(HomeTabModel),
    render({model}) {
        return vframe({
            items: [
                span('Some home content')
            ]
        });
    }
});