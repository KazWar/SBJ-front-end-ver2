import { createI18n } from "vue-i18n"

//* If translations are used outside of the views, uncomment here and in vite config.
//import messages from '@intlify/vite-plugin-vue-i18n/messages'

//* Messages are defined in the views themselves as translations.json
export const i18n = createI18n({
    //* Required setting for the composition API version of vue-i18n
    legacy: false,
    globalInjection: true,
    locale: 'en_gb',
    fallbackLocale: 'en_gb'
})