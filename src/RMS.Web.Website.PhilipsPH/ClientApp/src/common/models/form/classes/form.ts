import { Section, FormDTO } from "@/common"

/**
 * Registration form structure
 * 
 * @class
 * @classdesc Features 2 methods of creating a form object
 * @constructor 
 */
export class Form {
    id:number
    localeId:number
    languageCode:string
    countryCode:number
    name:string
    version:string
    sections:Array<Section>

    constructor(data:Form) {
        Object.assign(this, data)
    }

    /**
     * Parses JSON data to a Form object
     * @param {FormDTO} data - Response content data as valid FormDTO type
     * @returns {Form} Form object or never if data is empty
     */
    static fromDTO = (dto:FormDTO): Form => {
        return new Form({
            id: dto.formId,
            name: dto.companyName,
            localeId:dto.formLocaleId,
            languageCode:dto.languageCode,
            countryCode:dto.countryCode,
            version:dto.version,
            sections: [...dto.exportBlocks.map(block => { return Section.fromDTO(block)})]
        })
    }
}