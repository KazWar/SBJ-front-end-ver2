export default {
    /**
    * Returns a function which takes an id and returns all the
    * campaigns with a specific locale id from the campaigns store.
    * @example getLocaleIdByLocaleName(nl_nl)
    * @param {string} description - Locale description of the select language.
    * @return {function(string):Locale} locater
    */
    findLocaleIdByLocaleDescription: (state) => (locale) => {
        return state.items.filter((item) => item.description === locale);
    }
};
//# sourceMappingURL=getters.js.map