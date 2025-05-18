import {hoistCmp, XH} from '@xh/hoist/core';
import {dialog} from '@xh/hoist/kit/blueprint';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {Icon} from '@xh/hoist/icon';
import {button} from '@xh/hoist/desktop/cmp/button';
import {toolbar} from '@xh/hoist/desktop/cmp/toolbar';
import {filler} from '@xh/hoist/cmp/layout';
import {form} from '@xh/hoist/cmp/form';
import {formField} from '@xh/hoist/desktop/cmp/form';
import {numberInput, select, textInput} from '@xh/hoist/desktop/cmp/input';
import {EditOrderDialogModel} from './EditOrderDialogModel';

export const editOrderDialog = hoistCmp.factory<EditOrderDialogModel>(({model}) =>
    dialog({
        style: {width: 410, height: 'fit-content'},
        isOpen: model.isOpen,
        canOutsideClickClose: false,
        canEscapeKeyClose: true,
        onClose: () => model.close(),
        item: panel({
            title: model.isEditMode ? 'Edit Order' : 'Add Order',
            icon: model.isEditMode ? Icon.edit() : Icon.add(),
            compactHeader: true,
            headerItems: [
                button({
                    icon: Icon.close(),
                    onClick: () => model.close()
                })
            ],
            item: editForm(),
            bbar: bbar(),
            mask: XH.orderService.updateTask
        })
    })
);

const bbar = hoistCmp.factory<EditOrderDialogModel>(({model}) =>
    toolbar({
        items: [
            filler(),
            button({text: 'Cancel', icon: Icon.x(), outlined: true, onClick: () => model.close()}),
            button({
                text: 'Save',
                icon: Icon.save(),
                outlined: true,
                intent: 'success',
                disabled: !model.formModel.isValid,
                onClick: () => model.saveAsync()
            })
        ]
    })
);

const editForm = hoistCmp.factory<EditOrderDialogModel>(({model}) =>
    form({
        fieldDefaults: {
            minimal: false,
            commitOnChange: true
        },
        items: [
            formField({
                field: 'salesPersonId',
                item: select({options: XH.employeeService.employeeOptions})
            }),
            formField({
                field: 'customerId',
                item: select({options: XH.customerService.customerOptions})
            }),
            formField({
                field: 'productId',
                item: select({options: XH.productService.productOptions})
            }),

            formField({
                field: 'quantity',
                item: numberInput({
                    min: 1,
                    leftIcon: Icon.icon({iconName: 'input-numeric', prefix: 'fal'})
                })
            })
        ]
    })
);
