import {GridModel} from '@xh/hoist/cmp/grid';
import {HoistModel, LoadSpec, managed, XH} from '@xh/hoist/core';
import {makeObservable, observable} from '@xh/hoist/mobx';
import {asServerException, Customer, Order} from '../../../data/DataTypes';
import {fmtDateTimeSec} from '@xh/hoist/format';
import {NotificationMessage} from '../../../data/MessageTypes';
import {EditOrderDialogModel} from './EditOrderDialogModel';
import {Icon} from '@xh/hoist/icon';
import {isEmpty} from 'lodash';
import {HintTypeEnum, ordersHint} from './Orders';
import {CrmAppModel} from '../../../CrmAppModel';

export class OrdersModel extends HoistModel {
    @managed gridModel = this.createGridModel();
    @managed orderEditor = new EditOrderDialogModel();
    @observable customer: Customer;

    constructor() {
        super();
        makeObservable(this);

        XH.orderService.registerStore(this.gridModel.store);

        this.addReaction({
            track: () => this.customer,
            run: () => this.loadData()
        });

        this.addReaction({
            track: () => [XH.orderService.isLoading, this.gridModel.store.records],
            run: () => this.setHint()
        });

        // refresh rendered customer name when a customer changes
        XH.messageHub.subscribe('customer', this.xhId, (message: NotificationMessage) => {
            const {payload} = message,
                {eventType, data} = payload,
                customer = data as Customer;

            if (eventType == 'UPDATE' && customer.customerId === this.customer?.customerId) {
                this.gridModel.agApi.redrawRows();
            }
        });

        this.loadData();
        this.setHint();
    }

    get selectedOrder(): Order {
        return this.gridModel.selectedRecord?.raw as Order;
    }

    get customerId(): number {
        return this.customer?.customerId ?? -1;
    }

    addOrder() {
        this.orderEditor.open({
            orderId: 0,
            customerId: this.customerId,
            salesPersonId: (XH.appModel as CrmAppModel).userId,
            productId: null,
            quantity: 0
        });
    }

    cloneOrder() {
        const order = {...this.selectedOrder, orderId: 0};
        this.orderEditor.open(order);
    }

    editOrder() {
        this.orderEditor.open(this.selectedOrder);
    }

    async deleteOrder() {
        const confirm = await XH.confirm({
            title: 'Delete Order',
            icon: Icon.questionCircle(),
            message: `Are you sure you want to delete order # ${this.selectedOrder.orderId}?`
        });
        if (!confirm) return;

        try {
            await XH.orderService.deleteOrderAsync(this.selectedOrder.orderId);
        } catch (e) {
            XH.handleException(asServerException(e));
        }
    }

    private loadData() {
        XH.orderService.loadCustomer(this.customerId);
    }

    private updateFilter() {
        this.gridModel.store.setFilter({
            field: 'customerId',
            op: '=',
            value: this.customerId
        });
    }

    private setHint() {
        if (XH.orderService.isLoading) this.gridModel.emptyText = null;
        else if (this.customerId == -1)
            this.gridModel.emptyText = ordersHint(HintTypeEnum.NO_CUSTOMER);
        else if (isEmpty(this.gridModel.store.records))
            this.gridModel.emptyText = ordersHint(HintTypeEnum.NO_ORDERS);
    }

    private getSalesPersonName(salesPersonId: number): string {
        const salesPerson = XH.employeeService.getEmployeeById(salesPersonId);
        return salesPerson ? `${salesPerson.lastName}, ${salesPerson.firstName}` : '???';
    }

    private getProductName(productId: number): string {
        const product = XH.productService.getProductById(productId);
        return product ? product.name : '???';
    }

    private createGridModel(): GridModel {
        return new GridModel({
            store: {
                idSpec: 'orderId',
                processRawData: (order: Order) => {
                    return {
                        ...order,
                        product: this.getProductName(order.productId),
                        salesPerson: this.getSalesPersonName(order.salesPersonId)
                    };
                }
            },
            onRowDoubleClicked: () => this.editOrder(),
            columns: [
                {field: 'orderId', headerName: 'ID'},
                {
                    field: 'customerId',
                    headerName: 'Customer',
                    width: 150,
                    renderer: v => {
                        const customer = XH.customerService.getCustomerById(v);
                        return customer ? `${customer.lastName}, ${customer.firstName}` : '???';
                    }
                },
                {field: 'salesPerson', width: 150},
                {field: 'product', width: 250},
                {field: 'quantity', align: 'right', width: 120},
                {
                    field: 'timestamp',
                    headerName: 'Timestamp',
                    width: 200,
                    renderer: v => fmtDateTimeSec(v)
                }
            ]
        });
    }
}
