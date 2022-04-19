import { FieldDTO, Section } from "@/common"

export class BlockDTO {
    name:string
    title?: string | undefined
    icon?: string | undefined
    purchaseRegistration:boolean
    exportFields:Array<FieldDTO>

    constructor(data:BlockDTO) {
        Object.assign(this, data)
    }

    /**
     * Parses the section object into a block DTO object
     * 
     * @param {Section} section - Form section object
     * @returns {DTOBlock} DTO version of the section object
     */
     static toDTO(section:Section): BlockDTO {
        const {name, purchaseRegistration, fields, title, icon} = section

        return new BlockDTO({
            name:name,
            purchaseRegistration: purchaseRegistration,
            title:title,
            icon:icon,
            exportFields: [...fields.map(block => { return FieldDTO.fromField(block)})]
        }) 
    }
}
