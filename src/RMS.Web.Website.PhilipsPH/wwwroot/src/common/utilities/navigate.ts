//* Router initialization
import { router } from '@/plugins'

/**
 * Navigates the user to the destination page
 * 
 * @param Route - configuration
 */
export function Navigate (to:any):void {
    router.push({
            ...to
        }
    )
}

