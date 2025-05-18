import {HoistService, LoadSpec, SelectOption, XH} from '@xh/hoist/core';
import {Employee} from '../data/DataTypes';
import {csApiUrl} from './Defaults';

export class EmployeeService extends HoistService {
    private employees: Map<number, Employee> = new Map();

    override async initAsync(): Promise<void> {
        await this.loadAsync();
    }

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        const employees = await this.fetchEmployees();
        employees.forEach(employee => {
            this.employees.set(employee.employeeId, employee);
        });
    }

    get employeesList(): Employee[] {
        return Array.from(this.employees.values());
    }

    getEmployeeById(employeeId: number): Employee {
        return this.employees.get(employeeId);
    }

    get employeeOptions(): SelectOption[] {
        return this.employeesList.map(employee => ({
            value: employee.employeeId,
            label: `${employee.lastName}, ${employee.firstName}`
        }));
    }

    private async fetchEmployees(): Promise<Employee[]> {
        return XH.fetchJson({
            url: `${csApiUrl}/employees`
        });
    }
}
