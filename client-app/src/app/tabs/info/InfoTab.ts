import {hoistCmp} from '@xh/hoist/core';
import {creates} from '@xh/hoist/core';
import {vframe} from '@xh/hoist/cmp/layout';
import {span} from '@xh/hoist/cmp/layout';
import { InfoTabModel } from './InfoTabModel';

export const infoTab = hoistCmp.factory({
    model: creates(InfoTabModel),
    render({model}) {
        return vframe({
            items: [
                span('Some info content')
            ]
        });
    }
});