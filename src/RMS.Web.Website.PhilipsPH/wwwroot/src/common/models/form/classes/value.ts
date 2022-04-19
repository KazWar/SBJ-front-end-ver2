import { ValueDTO } from "@/common"

/**
 * Form data value type
 */
export class Value {
    key: string | number
    value: string | number
    handlingLine: any
    

    /**
     * Create a Value instance
     * 
     * @constructor
     * @param {string|number} key - A valid integer or string value
     * @param {string|number} value - A valid integer or string value
     * @returns A Value object instance
     */
    constructor(key: string | number, value: string | number, handlingLine:any) {
        this.key = key
        this.value = value
        this.handlingLine = handlingLine
    }

    /**
     * Parses JSON data to a Value object
     * 
     * @constructor
     * @param {ValueDTO} dto - Response content data as valid ValueDTO type
     * @returns {Value} A Value object instanced with the JSON data
     */
    static fromDTO = (dto:ValueDTO): Value => {
        const { ListValueTranslationKeyValue, ListValueTranslationDescription, HandlingLine } = dto

        return new Value(ListValueTranslationKeyValue, ListValueTranslationDescription, HandlingLine)
    }
}