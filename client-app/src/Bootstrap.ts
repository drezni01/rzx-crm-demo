declare module '@xh/hoist/core' {
    // Merge interface with XHApi class to include injected services.

    // @ts-ignore - Help IntelliJ recognize uses of injected service methods on the `XH` singleton.
    export const XH: XHApi;

    export interface HoistUser {
        profilePicUrl: string;
    }
}

//import {ModuleRegistry, AllCommunityModule} from 'ag-grid-community';

//ModuleRegistry.registerModules([AllCommunityModule]);
