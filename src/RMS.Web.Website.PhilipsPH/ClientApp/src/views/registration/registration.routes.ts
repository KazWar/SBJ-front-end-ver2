import Registration from './registration.vue'
import RegistrationEdit from './registration-edit.vue'

export default [
	{
        name: 'PurchaseRegistration',
        path: '/:locale/campaigns/:campaignCode',
        component: Registration,
        props: true
    },
	{
        name: 'PurchaseRegistrationEdit',
        path: '/:locale/campaigns/:campaignCode/registration/:registrationId/edit',
        component: RegistrationEdit,
    }
]

    

    