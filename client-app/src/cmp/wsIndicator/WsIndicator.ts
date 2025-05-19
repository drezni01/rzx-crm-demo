import {div, hbox, span} from '@xh/hoist/cmp/layout';
import {hoistCmp, XH} from '@xh/hoist/core';
import classNames from 'classnames';
import './WsIndicator.scss';

export const wsIndicator = hoistCmp.factory(({showDescription, status}) => {
    const wsStatus = status || XH.messageHub.wsStatus,
        className = 'ws-notify';
    let colorClass = 'green',
        description: string = null;

    if (wsStatus === 'ERR') {
        colorClass = 'red';
        description: 'Disconnected';
    } else if (wsStatus === 'RCV') {
        colorClass = 'green-blink';
        description = 'Receiving';
    } else {
        description = 'Connected';
    }

    return hbox(
        div({
            className: classNames(className, `${className}__${colorClass}`),
            title: 'WebSocket status'
        }),
        span({omit: !showDescription, item: description, style: {marginLeft: 8}})
    );
});
