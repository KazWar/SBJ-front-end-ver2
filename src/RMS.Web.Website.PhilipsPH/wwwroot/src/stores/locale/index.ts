import { defineStore } from "pinia"

//* Import store components
import state from './state'
import getters from './getters'
import actions from './actions'

//* Create store instance
export const useLocaleStore = defineStore('locales',{
	state: () => { 
        return  { 
            ...state
        }
    },

    getters,
    actions
})
