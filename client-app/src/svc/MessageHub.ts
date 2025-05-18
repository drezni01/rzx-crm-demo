import {AppState, HoistService, XH} from '@xh/hoist/core';
import {LogLevel, HubConnectionBuilder, HubConnection} from '@microsoft/signalr';

import {isEmpty} from 'lodash';
import {action, makeObservable, observable} from '@xh/hoist/mobx';
import {
    MessageTopicType,
    NotificationMessage,
    NotificationMessageCallbackFn,
    NotificationMessageSubscription,
    WsStatusType
} from '../data/MessageTypes';
import {csApiUrl} from './Defaults';

export class MessageHub extends HoistService {
    private connection: HubConnection;
    private subscriptions: Map<MessageTopicType, NotificationMessageSubscription[]>;
    @observable wsStatus: WsStatusType;

    constructor() {
        super();
        makeObservable(this);
    }

    override async initAsync() {
        console.log('initializing MessageHub');
        this.subscriptions = new Map();

        this.connection = new HubConnectionBuilder()
            .withUrl(`${csApiUrl}/ws`)
            .configureLogging(LogLevel.Information)
            .build();

        this.registerListen('customer');
        this.registerListen('order');

        this.connection.onclose(() => this.startWs(false));

        await this.startWs(false);

        this.addReaction({
            track: () => XH.appIsRunning,
            run: isRunning => {
                if (!isRunning) {
                    console.warn('MessageHub: suspending WS svc updates');
                    this.connection.stop();
                }
            }
        });
    }

    private registerListen(topic: MessageTopicType) {
        this.connection.on(topic, message => {
            this.notify(message);
        });
    }

    private async startWs(isRestart) {
        if (XH.appState == AppState.SUSPENDED) return;

        try {
            await this.connection.start();
            console.log('MessageHub: signalR connected');
            this.updateWsStatus('OK');
            if (isRestart) {
                this.notifyReload();
            }
        } catch (err) {
            console.log(err);
            this.updateWsStatus('ERR');
            if (!XH.appIsRunning) {
                console.warn('MessageHub: suspending WS svc updates');
                return;
            }
            setTimeout(() => this.startWs(true), 5000);
        }
    }

    subscribe(topic: MessageTopicType, id: string, callbackFn: NotificationMessageCallbackFn) {
        const callbacks = this.subscriptions.get(topic);
        if (isEmpty(callbacks)) {
            this.subscriptions.set(topic, []);
        }

        console.log(`MessageHub: ${id} subscribing to [${topic}]`);
        this.subscriptions.get(topic).push({id, callbackFn});
    }

    unSubscribe(topic: MessageTopicType, id: string) {
        let subscriptions = this.subscriptions.get(topic);
        if (isEmpty(subscriptions)) return;

        subscriptions = subscriptions.filter(s => s.id != id);
        console.log(`MessageHub: ${id} un-subscribing from [${topic}]`);
        this.subscriptions.set(topic, subscriptions);
    }

    private notify(message: NotificationMessage) {
        const subscriptions = this.subscriptions.get(message.envelope.topic);
        if (isEmpty(subscriptions)) return;

        this.updateWsStatus('RCV');
        subscriptions.forEach(subscription => {
            try {
                subscription.callbackFn?.(message);
            } catch (e) {
                console.error(e);
            }
        });
    }

    @action private updateWsStatus(status: WsStatusType) {
        if (status === this.wsStatus) return;

        this.wsStatus = status;
        if (status === 'RCV') {
            setTimeout(() => this.updateWsStatus('OK'), 250);
        }
    }

    private notifyReload() {
        const reloadMessage: NotificationMessage = {
            envelope: {id: -1, timestamp: null, topic: null},
            payload: {eventType: 'RELOAD'}
        };
        try {
            for (const subscriptions of this.subscriptions.values()) {
                for (const subscription of subscriptions) {
                    subscription.callbackFn?.(reloadMessage);
                }
            }
        } catch (err) {
            console.error(err);
        }
    }
}
