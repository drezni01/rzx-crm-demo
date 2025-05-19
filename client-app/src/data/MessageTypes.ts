export type WsStatusType = 'OK' | 'RCV' | 'ERR';

export type MessageEventType = 'ADD' | 'UPDATE' | 'DELETE' | 'RELOAD';

export type MessageTopicType = 'customer' | 'order';

export type NotificationMessage = {
    envelope: MessageEnvelope;
    payload: MessagePayload;
};

export type MessageEnvelope = {
    id: number;
    timestamp: string;
    topic: MessageTopicType;
};

export type MessagePayload = {
    eventType: MessageEventType;
    data?: any;
};

export type NotificationMessageCallbackFn = (NotificationMessage) => void;

export type NotificationMessageSubscription = {
    id: string;
    callbackFn: NotificationMessageCallbackFn;
};
