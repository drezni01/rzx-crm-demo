import {HoistService, LoadSpec, managed, TaskObserver, XH} from '@xh/hoist/core';
import {Order} from '../data/DataTypes';
import {csApiUrl} from './Defaults';
import {MessagePayload, NotificationMessage} from '../data/MessageTypes';
import {Store} from '@xh/hoist/data';
import {makeObservable, observable} from '@xh/hoist/mobx';

export class OrderService extends HoistService {
    apiTask = TaskObserver.trackLast();
    @observable isLoading: boolean;
    private orderMap: Map<number, Order> = new Map();
    private msgBuffer: MessagePayload[] = [];
    private stores: Map<string, Store> = new Map();
    private customerId: number = -1;

    constructor() {
        super();
        makeObservable(this);
    }

    override async initAsync(): Promise<void> {
        XH.messageHub.subscribe('order', this.xhId, (message: NotificationMessage) => {
            if (this.isLoading) this.msgBuffer.push(message.payload);
            else this.processMessages([message.payload]);
        });

        await this.loadAsync();
    }

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        this.isLoading = true;
        this.orderMap.clear();
        this.stores.forEach(store => store.clear());

        if (this.customerId < 0) {
            this.isLoading = false;
            return;
        }

        const orders = await this.fetchOrdersForCustomer(this.customerId).linkTo(this.apiTask);
        orders.forEach(order => this.orderMap.set(order.orderId, order));
        this.stores.forEach(store => store.loadData(orders));

        this.isLoading = false;
        const pendingMessages = [...this.msgBuffer];
        this.msgBuffer = [];
        this.processMessages(pendingMessages);
    }

    registerStore(store: Store) {
        console.log(`registering store ${store.xhId}`);
        this.stores.set(store.xhId, store);
        store.loadData(Array.from(this.orderMap.values()));
    }

    async loadCustomer(customerId: number) {
        this.customerId = customerId;
        await this.loadAsync();
    }

    private processMessages(messages: MessagePayload[]) {
        for (const message of messages) {
            const {eventType, data} = message,
                order = data as Order;

            if (eventType == 'RELOAD') {
                this.loadAsync();
                break;
            }

            if (order.customerId !== this.customerId) continue;

            switch (eventType) {
                case 'ADD':
                case 'UPDATE':
                    this.processAddOrUpdate(order);
                    break;
                case 'DELETE':
                    this.processDelete(order.orderId);
                    break;
            }
        }
    }

    private processAddOrUpdate(order: Order) {
        const existing = this.orderMap.get(order.orderId);
        if (!existing) {
            console.log(`adding order #${order.orderId} to stores`);
            this.orderMap.set(order.orderId, order);
            this.stores.forEach(store => store.updateData({add: [order]}));
        } else if (order.timestamp > existing.timestamp) {
            console.log(`updating order #${order.orderId} in stores`);
            this.orderMap.set(order.orderId, order);
            this.stores.forEach(store => store.updateData({update: [order]}));
        }
    }

    private processDelete(orderId: number) {
        const existing = this.orderMap.get(orderId);
        if (existing) {
            console.log(`deleting order #${orderId} from stores`);
            this.orderMap.delete(orderId);
            this.stores.forEach(store => store.updateData({remove: [orderId]}));
        }
    }

    async addOrderAsync(order: Order): Promise<void> {
        order = await this.addOrderImpl(order).linkTo(this.apiTask);
        this.processAddOrUpdate(order);
    }

    async updateOrderAsync(order: Order): Promise<void> {
        order = await this.updateOrderImpl(order).linkTo(this.apiTask);
        this.processAddOrUpdate(order);
    }

    async deleteOrderAsync(orderId: number) {
        await this.deleteOrderImpl(orderId).linkTo(this.apiTask);
        this.processDelete(orderId);
    }

    private async fetchOrders(): Promise<Order[]> {
        return XH.fetchJson({
            url: `${csApiUrl}/orders`
        });
    }

    private async fetchOrdersForCustomer(customerId: number): Promise<Order[]> {
        return XH.fetchJson({
            url: `${csApiUrl}/orders/customer/${customerId}`
        });
    }

    private async addOrderImpl(order: Order): Promise<Order> {
        return XH.fetchService.postJson({
            url: `${csApiUrl}/orders`,
            body: order
        });
    }

    private async updateOrderImpl(order: Order): Promise<Order> {
        return XH.fetchService.putJson({
            url: `${csApiUrl}/orders/${order.orderId}`,
            body: order
        });
    }

    private async deleteOrderImpl(id: number): Promise<void> {
        return XH.fetchService.deleteJson({
            url: `${csApiUrl}/orders/${id}`
        });
    }
}
