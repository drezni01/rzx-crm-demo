import {hoistCmp, uses, XH} from '@xh/hoist/core';
import {CrmAppModel} from './CrmAppModel';
import {appBar, appBarSeparator} from '@xh/hoist/desktop/cmp/appbar';
import {Icon} from '@xh/hoist/icon';
import {tabSwitcher} from '@xh/hoist/desktop/cmp/tab';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {wsIndicator} from './cmp/wsIndicator/WsIndicator';
import {tabContainer} from '@xh/hoist/cmp/tab';
import {fakeLogin} from './cmp/login/FakeLogin';
import './CrmApp.scss';

export const CrmApp = hoistCmp({
    displayName: 'crmApp',
    model: uses(CrmAppModel),

    render({model}) {
        return model.showLogin
            ? fakeLogin({userId: model.userId, loginFn: () => model.login()})
            : panel({
                  tbar: appBar({
                      title: 'CRM Application',
                      icon: Icon.userCircle({size: '2x'}),
                      leftItems: [tabSwitcher()],
                      rightItems: [user(model.userId), appBarSeparator(), wsIndicator()],
                      hideRefreshButton: false
                  }),
                  item: tabContainer()
              });
    }
});

const user = hoistCmp.factory<CrmAppModel>(({model}) => {
    const user = XH.employeeService.getEmployeeById(model.userId);
    return user ? `${user.lastName}, ${user.firstName}` : null;
});
