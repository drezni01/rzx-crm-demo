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

export function asServerException(exception) {
    if (exception.serverDetails) {
        const serverException = exception.serverDetails;
        if (serverException.error?.message) {
            return {message: serverException.error.message, details: serverException.error};
        } else if (serverException.errors) {
            return {
                message: JSON.stringify(serverException.errors),
                details: serverException.errors
            };
        }
    }

    return exception;
}
