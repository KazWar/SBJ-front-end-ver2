export default {

  /**
  * Returns a function which takes an id and returns all the
  * campaigns with a specific locale id from the campaigns store.
  * @example getLocaleIdByLocaleName(nl_nl)
  * @param description - Locale description of the select language.
  * @returns locater
  */
  findLocaleIdByLocaleDescription: (state:any):any  => (locale:string): Locale => {
    return state.items.filter((item:Locale) => item.description === locale)
  }
}