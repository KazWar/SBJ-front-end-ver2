import { StateTree } from 'pinia'

export default {
  /**
  * Returns a computed function which takes a campaign code and returns
  * the corresponding campaign from the campaigns list.
  * @example getCampaignByCode(16050)
  * @param {number} campaignCode - Code of the select campaign
  * @yields {function(number):Campaign} locater
  */
  getCampaignByCode: (state:StateTree) => (campaignCode:number):Campaign => {
    return state.items.find((item:Campaign) => item.code = campaignCode)
  }
}