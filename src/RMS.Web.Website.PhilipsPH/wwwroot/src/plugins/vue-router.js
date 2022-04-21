import { createRouter, createWebHistory } from 'vue-router';
//* Import all the routes
import { views as routes } from '@/views';
//* Define router options
const options = {
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [...routes]
};
export const router = createRouter(options);
//# sourceMappingURL=vue-router.js.map