//* Router initialization
import { router } from '@/plugins';
/**
 * Navigates the user to the destination page
 * @param {RouteRecordRaw} Route configuration
 */
export function Navigate(to) {
    router.push({
        ...to
    });
}
//# sourceMappingURL=navigate.js.map