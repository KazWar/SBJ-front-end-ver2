//* Base imports
import { createApp } from 'vue'
import app from './App.vue'

//* Quasar import
import { Quasar } from 'quasar'
import 'quasar/src/css/index.sass'
import '@quasar/extras/material-icons/material-icons.css'

//* Common components import
import { useComponents } from '@/common/components'

//* Plugins folder import
import {
  pinia,
  router,
  i18n,
  useSentry
} from '@/plugins'

//* Create app instance
const App = createApp(app)

//* Initialize the components
await useComponents(App)
await useSentry(App, router)

//* Bind plugins to vue instance
App
  .use(pinia)
  .use(Quasar)
  .use(router)
  .use(i18n)

//* Mount the app instance
App.mount('#app')


