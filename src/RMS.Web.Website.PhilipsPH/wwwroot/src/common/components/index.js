import ToolTip from './tooltip.vue';
import LoadingBar from './loading-bar.vue';
/**
 * Installs UI components into the application
 * @param application - Vue application instance
 */
export const useComponents = async (application) => {
    const plugin = {
        install(application) {
            const items = [
                { tag: 'tool-tip', component: ToolTip },
                { tag: 'loading-bar', component: LoadingBar }
            ];
            for (const { tag, component } of items) {
                application.component(tag, component);
            }
        }
    };
    application.use(plugin);
};
//# sourceMappingURL=index.js.map