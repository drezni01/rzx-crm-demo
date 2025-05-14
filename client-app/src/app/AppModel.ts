import {HoistAppModel, managed, XH} from '@xh/hoist/core';
import {TabContainerModel} from '@xh/hoist/cmp/tab';
import {Icon} from '@xh/hoist/icon';
import {homeTab} from './tabs/home/HomeTab';
import {infoTab} from './tabs/info/InfoTab';

export class AppModel extends HoistAppModel {
    @managed
    tabModel;

    override async initAsync() {
        this.tabModel = new TabContainerModel({
            route: 'default',
            track: true,
            switcher: false,
            tabs: [
                {id: 'home', icon: Icon.home(), content: homeTab},
                {id: 'info', icon: Icon.info(), content: infoTab}
            ]
        });
    }

    override async doLoadAsync() {}

    override getRoutes() {
        return [
            {
                name: 'default',
                path: '/app',
                children: [
                    {name: 'home', path: '/home'},
                    {name: 'info', path: '/info'}
                ]
            }
        ];
    }
}
