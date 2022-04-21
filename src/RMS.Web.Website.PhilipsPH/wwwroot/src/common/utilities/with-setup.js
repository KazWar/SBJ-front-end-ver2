import { createApp } from 'vue';
export function withSetup(composable) {
    let result;
    const app = createApp({
        setup() {
            result = composable();
            // eslint-disable-next-line @typescript-eslint/no-empty-function
            return () => { };
        }
    });
    app.mount(document.createElement('div'));
    // return the result and the app instance
    // for testing provide / unmount
    return [result, app];
}
//# sourceMappingURL=with-setup.js.map