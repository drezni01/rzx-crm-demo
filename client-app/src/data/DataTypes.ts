export type Customer = {
    customerId: number;
    firstName: string;
    lastName: string;
    middleInitial: string;
    orderCount: number;
    timestamp: string;
};

export type Employee = {
    employeeId: number;
    firstName: string;
    lastName: string;
    middleInitial: string;
    timestamp: string;
};

export type Product = {
    productId: number;
    name: string;
    price: number;
    timestamp: string;
};

export type Order = {
    orderId: number;
    customerId: number;
    salesPersonId: number;
    productId: number;
    quantity: number;
    timestamp?: string;
};
