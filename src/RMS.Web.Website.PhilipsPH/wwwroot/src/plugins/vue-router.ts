import { createRouter, createWebHistory, RouterOptions } from 'vue-router'

//* Import all the routes
import {views as routes} from '@/views'

//* Define router options
const options: RouterOptions = {
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [...routes]
}

export const router = createRouter(options)

