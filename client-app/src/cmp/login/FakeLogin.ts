import {hoistCmp, XH} from '@xh/hoist/core';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {filler, vbox, viewport, vspacer} from '@xh/hoist/cmp/layout';
import {Icon} from '@xh/hoist/icon';
import {select, textInput} from '@xh/hoist/desktop/cmp/input';
import {button} from '@xh/hoist/desktop/cmp/button';
import './FakeLogin.scss';

export const fakeLogin = hoistCmp.factory(({userId, loginFn}) => {
    return viewport({
        alignItems: 'center',
        justifyContent: 'center',
        flexDirection: 'column',
        item: panel({
            title: 'Pre-set random login',
            icon: Icon.icon({iconName: 'shuffle', prefix: 'fal'}),
            className: 'fake-login',
            width: 300,

            items: [
                vspacer(10),
                vbox(
                    select({
                        options: XH.employeeService.employeeOptions,
                        value: userId,
                        width: null
                    }),
                    textInput({
                        type: 'password',
                        value: 'asdfsddsafds',
                        width: null
                    })
                ),
                vspacer(10)
            ],
            bbar: [
                filler(),
                button({
                    text: 'Login',
                    intent: 'primary',
                    icon: Icon.user(),
                    minimal: false,
                    autoFocus: true,
                    onClick: loginFn
                })
            ]
        })
    });
});

export function getRandomUser(): number {
    const users = XH.employeeService.employeesList,
        idx = Math.floor(Math.random() * users.length);
    return users[idx].employeeId;
}
