import {tabContainer} from '@xh/hoist/cmp/tab';
import {hoistCmp, uses} from '@xh/hoist/core';
import {appBar} from '@xh/hoist/desktop/cmp/appbar';
import {tabSwitcher} from '@xh/hoist/desktop/cmp/tab';
import {Icon} from '@xh/hoist/icon';

import { AppModel } from './AppModel';
import { panel } from '@xh/hoist/desktop/cmp/panel';

export const App = hoistCmp({
    displayName: 'Hoist App',
    model: uses(AppModel),

    render() {
        return panel({
            tbar: appBar({
                icon: Icon.rocket({size: '2x', prefix: 'fal'}),
                title: null,
                leftItems: [tabSwitcher()],
                hideWhatsNewButton: true,
                appMenuButtonProps: {
                    hideLogoutItem: false
                }
            }),
            item: tabContainer()
        });
    }
});