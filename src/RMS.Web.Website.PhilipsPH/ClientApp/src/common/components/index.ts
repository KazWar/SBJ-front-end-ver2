import { App } from 'vue'
import ToolTip from './tooltip.vue'
import LoadingBar from './loading-bar.vue'

/**
 * Installs UI components into the application
 * @param application - Vue application instance
 */
export const useComponents = async (application:App):Promise<void> =>{
  const plugin = {
    install (application:App) {
      const items = [
        { tag: 'tool-tip', component: ToolTip },
        { tag: 'loading-bar', component: LoadingBar }
      ]

      for (const { tag, component } of items) {
        application.component(tag, component)
      }
    }
  }

  application.use(plugin)
}