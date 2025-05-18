import {GridModel} from '@xh/hoist/cmp/grid';
import {HoistModel, LoadSpec, managed, XH} from '@xh/hoist/core';
import {fmtDateTimeSec} from '@xh/hoist/format';

export class ProductsTabModel extends HoistModel {
    @managed gridModel = this.createGridModel();

    override async doLoadAsync(loadSpec: LoadSpec): Promise<void> {
        this.gridModel.clear();

        const products = XH.productService.productsList;
        this.gridModel.loadData(products);
    }

    private createGridModel(): GridModel {
        return new GridModel({
            store: {
                idSpec: 'productId'
            },
            contextMenu: null,
            columns: [
                {field: 'productId', headerName: 'ID'},
                {field: 'name', width: 250},
                {field: 'price', width: 120, align: 'right', renderer: v => `$${v.toFixed(2)}`},
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
