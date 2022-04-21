/**
 * Form data value type
 */
export class Value {
    key;
    value;
    handlingLine;
    /**
     * Create a Value instance
     *
     * @constructor
     * @param {string|number} key - A valid integer or string value
     * @param {string|number} value - A valid integer or string value
     * @returns A Value object instance
     */
    constructor(key, value, handlingLine) {
        this.key = key;
        this.value = value;
        this.handlingLine = handlingLine;
    }
    /**
     * Parses JSON data to a Value object
     *
     * @constructor
     * @param {ValueDTO} dto - Response content data as valid ValueDTO type
     * @returns {Value} A Value object instanced with the JSON data
     */
    static fromDTO = (dto) => {
        const { ListValueTranslationKeyValue, ListValueTranslationDescription, HandlingLine } = dto;
        return new Value(ListValueTranslationKeyValue, ListValueTranslationDescription, HandlingLine);
    };
}
//# sourceMappingURL=value.js.map