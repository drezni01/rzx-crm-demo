import {creates, hoistCmp} from '@xh/hoist/core';
import {EmployeesTabModel} from './EmployeesTabModel';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {grid} from '@xh/hoist/cmp/grid';

export const employeesTab = hoistCmp.factory({
    model: creates(EmployeesTabModel),
    render({model}) {
        return panel({
            item: grid(),
            mask: 'onLoad'
        });
    }
});
