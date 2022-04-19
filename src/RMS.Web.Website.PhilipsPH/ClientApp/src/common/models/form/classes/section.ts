import { Field, BlockDTO } from "@/common"

export class Section {
    name:string
    title?: string | undefined
    icon?: string | undefined
    purchaseRegistration:boolean
    fields:Array<Field>

    constructor(data:Section) {
        Object.assign(this, data)
    }

    /**
     * Parses JSON data to a Section object
     * 
     * @param {DTOBlock} dto - Form block data object from the form
     * @returns {Section} Form object
     */
    static fromDTO(dto:BlockDTO): Section {
        const {name, purchaseRegistration, exportFields, title, icon} = dto

        return new Section({
            name:name,
            title: title,
            icon:icon,
            purchaseRegistration: purchaseRegistration,
            fields: [...exportFields.map(block => { return Field.fromDTO(block)})]
        }) 
    }
}