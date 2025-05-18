import {hoistCmp, XH} from '@xh/hoist/core';
import {EditCustomerDialogModel} from './EditCustomerDialogModel';
import {dialog} from '@xh/hoist/kit/blueprint';
import {panel} from '@xh/hoist/desktop/cmp/panel';
import {Icon} from '@xh/hoist/icon';
import {button} from '@xh/hoist/desktop/cmp/button';
import {toolbar} from '@xh/hoist/desktop/cmp/toolbar';
import {filler} from '@xh/hoist/cmp/layout';
import {form} from '@xh/hoist/cmp/form';
import {formField} from '@xh/hoist/desktop/cmp/form';
import {textInput} from '@xh/hoist/desktop/cmp/input';

export const editCustomerDialog = hoistCmp.factory<EditCustomerDialogModel>(({model}) =>
    dialog({
        style: {width: 410, height: 'fit-content'},
        isOpen: model.isOpen,
        canOutsideClickClose: false,
        canEscapeKeyClose: true,
        onClose: () => model.close(),
        item: panel({
            title: model.isEditMode ? 'Edit Customer' : 'Add Customer',
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
            mask: XH.customerService.updateTask
        })
    })
);

const bbar = hoistCmp.factory<EditCustomerDialogModel>(({model}) =>
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

const editForm = hoistCmp.factory<EditCustomerDialogModel>(({model}) =>
    form({
        fieldDefaults: {
            minimal: false,
            commitOnChange: true
        },
        items: [
            formField({field: 'firstName', item: textInput()}),
            formField({field: 'middleInitial', item: textInput({width: 40})}),
            formField({field: 'lastName', item: textInput()})
        ]
    })
);
