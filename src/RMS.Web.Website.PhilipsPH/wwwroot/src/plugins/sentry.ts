import { App } from "vue"
import { Router } from "vue-router"
import * as Sentry from "@sentry/vue"
import { BrowserTracing } from "@sentry/tracing"

export const useSentry = async (app:App, router:Router) => { 
    Sentry.init({
        app,
        dsn: "https://087dc179bffc4c9e91ba9aeee14b2384@o1200929.ingest.sentry.io/6325122",
        integrations: [
            new BrowserTracing({
                routingInstrumentation: Sentry.vueRouterInstrumentation(router),
                tracingOrigins: ["localhost", "my-site-url.com", /^\//],
            }),
        ],
        // Set tracesSampleRate to 1.0 to capture 100%
        // of transactions for performance monitoring.
        // We recommend adjusting this value in production
        tracesSampleRate: 1.0,
    })
}