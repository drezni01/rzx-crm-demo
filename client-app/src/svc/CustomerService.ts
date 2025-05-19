import {HoistService, LoadSpec, SelectOption, TaskObserver, XH} from '@xh/hoist/core';
import {Customer} from '../data/DataTypes';
import {action, makeObservable, observable, runInAction} from '@xh/hoist/mobx';
import {csApiUrl} from './Defaults';
import {MessagePayload, NotificationMessage} from '../data/MessageTypes';

export class CustomerService extends HoistService {
    loadTask = TaskObserver.trackLast();
    updateTask = TaskObserver.trackLast();
    deleteTask = TaskObserver.trackLast();
    @observable.ref customers: Customer[] = [];
    private customerMap: Map<number, Customer> = new Map();

    constructor() {
        super();
        makeObservable(this);
    }

    override async initAsync(): Promise<void> {
        XH.messageHub.subscribe('customer', this.xhId, (message: NotificationMessage) => {
            const {payload} = message,
                {eventType, data} = payload,
                customer = data as Customer;
                
            switch (eventType) {
                case 'ADD':
                    this.addInPlace(customer);
                    break;
                case 'UPDATE':
                    this.updateInPlace(customer);
                    break;
                case 'DELETE':
                    this.deleteInPlace(customer.customerId);
                    break;
                case 'RELOAD':
                    this.loadAsync();
                    return;
            }
        });

        await this.loadAsync();
    }

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        const customers = await this.fetchCustomersImpl().linkTo(this.loadTask);
        runInAction(() => {
            this.customers = customers;
            customers.forEach(customer => {
                this.customerMap.set(customer.customerId, customer);
            });
        });
    }

    get customersList(): Customer[] {
        return Array.from(this.customers.values());
    }

    getCustomerById(customerId: number): Customer {
        return this.customerMap.get(customerId);
    }

    get customerOptions(): SelectOption[] {
        return this.customersList.map(customer => ({
            value: customer.customerId,
            label: `${customer.lastName}, ${customer.firstName}`
        }));
    }

    async addCustomerAsync(customer: Customer): Promise<void> {
        customer = await this.addCustomerImpl(customer).linkTo(this.updateTask);
        this.addInPlace(customer);
    }

    async updateCustomerAsync(customer: Customer): Promise<void> {
        customer = await this.updateCustomerImpl(customer).linkTo(this.updateTask);
        this.updateInPlace(customer);
    }

    async deleteCustomerAsync(customerId: number): Promise<void> {
        await this.deleteCustomerImpl(customerId).linkTo(this.deleteTask);
        this.deleteInPlace(customerId);
    }

    @action
    private addInPlace(customer: Customer) {
        this.customers = [...this.customers, customer];
        this.customerMap.set(customer.customerId, customer);
    }

    @action
    private updateInPlace(customer: Customer) {
        this.customers = [
            ...this.customers.filter(c => c.customerId !== customer.customerId),
            customer
        ];
        this.customerMap.set(customer.customerId, customer);
    }

    @action
    private deleteInPlace(customerId: number) {
        this.customers = this.customers.filter(c => c.customerId !== customerId);
        this.customerMap.delete(customerId);
    }

    private async fetchCustomersImpl(): Promise<Customer[]> {
        return XH.fetchJson({
            url: `${csApiUrl}/customers`
        });
    }

    private async addCustomerImpl(customer: Customer): Promise<Customer> {
        return XH.fetchService.postJson({
            url: `${csApiUrl}/customers`,
            body: customer
        });
    }

    private async updateCustomerImpl(customer: Customer): Promise<Customer> {
        return XH.fetchService.putJson({
            url: `${csApiUrl}/customers/${customer.customerId}`,
            body: customer
        });
    }

    private async deleteCustomerImpl(id: number): Promise<void> {
        return XH.fetchService.deleteJson({
            url: `${csApiUrl}/customers/${id}`
        });
    }
}
