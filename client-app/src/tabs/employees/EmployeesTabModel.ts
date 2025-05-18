import {GridModel} from '@xh/hoist/cmp/grid';
import {HoistModel, LoadSpec, managed, XH} from '@xh/hoist/core';
import {fmtDateTimeSec} from '@xh/hoist/format';

export class EmployeesTabModel extends HoistModel {
    @managed gridModel = this.createGridModel();

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        this.gridModel.clear();

        const employees = XH.employeeService.employeesList;
        this.gridModel.loadData(employees);
    }

    private createGridModel(): GridModel {
        return new GridModel({
            store: {
                idSpec: 'employeeId'
            },
            contextMenu: null,
            columns: [
                {field: 'employeeId', headerName: 'ID'},
                {field: 'firstName', width: 150},
                {field: 'lastName', width: 150},
                {
                    field: 'timestamp',
                    headerName: 'Updated',
                    width: 200,
                    renderer: v => fmtDateTimeSec(v)
                }
            ]
        });
    }
}
