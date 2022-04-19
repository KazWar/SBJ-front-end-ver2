//* import all the routes
import HomeRoutes from './home/home.routes.js'
import AboutRoutes from './error/error.routes.js'
import ErrorRoutes from './about/about.routes.js'
import LanguageSelectRoutes from './language-select/language-select.routes.js'
import CampaignsRoutes from './campaigns/campaigns.routes.js'
import RegistrationRoutes from './registration/registration.routes.js'
import ThankYouRoutes from './thank-you/thank-you.routes.js'

//* Compile all the routes for ready to use importing.
export const views:Array<any> = [
	...HomeRoutes,
	...AboutRoutes,
	...ErrorRoutes,
	...LanguageSelectRoutes,
	...CampaignsRoutes,
	...RegistrationRoutes,
	...ThankYouRoutes
]