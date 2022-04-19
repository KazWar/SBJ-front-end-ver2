import ThankYou from './thank-you.vue'

export default [
	{
        name: 'ThankYou',
        path: '/:locale/thank-you',
        component: ThankYou
    },
    {
        name:'company',
        path: '/company',
        beforeEnter() { location.href = 'https://www.philips.nl' }
    }
]

