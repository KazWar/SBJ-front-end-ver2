import Registration from './registration.vue'

export default [
	{
        name: 'PurchaseRegistration',
        path: '/:locale/campaigns/:campaignCode',
        component: Registration,
        props: true
    }
]

    

    