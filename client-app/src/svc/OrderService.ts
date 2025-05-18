import {HoistService, LoadSpec, managed, TaskObserver, XH} from '@xh/hoist/core';
import {Order} from '../data/DataTypes';
import {csApiUrl} from './Defaults';
import {MessagePayload, NotificationMessage} from '../data/MessageTypes';
import {Store} from '@xh/hoist/data';

export class OrderService extends HoistService {
    loadTask = TaskObserver.trackLast();
    updateTask = TaskObserver.trackLast();
    deleteTask = TaskObserver.trackLast();
    private orderMap: Map<number, Order> = new Map();
    private msgBuffer: MessagePayload[] = [];
    private isLoading: boolean;
    private stores: Map<string, Store> = new Map();

    override async initAsync(): Promise<void> {
        XH.messageHub.subscribe('order', this.xhId, (message: NotificationMessage) => {
            if (this.isLoading) this.msgBuffer.push(message.payload);
            else this.processMessages([message.payload]);
        });

        await this.loadAsync();
    }

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        console.log('enter load mode');
        this.isLoading = true;
        const orders = await this.fetchOrders().linkTo(this.loadTask);
        console.log('fetched orders');
        this.orderMap.clear();
        orders.forEach(order => this.orderMap.set(order.orderId, order));
        this.stores.forEach(store => store.loadData(orders));
        console.log('loaded stores');

        this.isLoading = false;
        console.log('exit load mode');
        const pendingMessages = [...this.msgBuffer];
        this.msgBuffer = [];
        console.log(`process ${pendingMessages.length} pending msgs`);
        this.processMessages(pendingMessages);
    }

    registerStore(store: Store) {
        console.log(`registering store ${store.xhId}`);
        this.stores.set(store.xhId, store);
        store.loadData(Array.from(this.orderMap.values()));
    }

    private processMessages(messages: MessagePayload[]) {
        messages.forEach(message => {
            const {eventType, entity} = message;
            switch (eventType) {
                case 'ADD':
                case 'UPDATE':
                    this.processAddOrUpdate(entity);
                    break;
                case 'DELETE':
                    this.processDelete(entity.orderId);
                    break;
                case 'RELOAD':
                    this.loadAsync();
                    break;
            }
        });
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
        order = await this.addOrderImpl(order).linkTo(this.updateTask);
        this.processAddOrUpdate(order);
    }

    async updateOrderAsync(order: Order): Promise<void> {
        order = await this.updateOrderImpl(order).linkTo(this.updateTask);
        this.processAddOrUpdate(order);
    }

    async deleteOrderAsync(orderId: number) {
        await this.deleteOrderImpl(orderId).linkTo(this.deleteTask);
        this.processDelete(orderId);
    }

    private async fetchOrders(): Promise<Order[]> {
        return XH.fetchJson({
            url: `${csApiUrl}/orders`
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
