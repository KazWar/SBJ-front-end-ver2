import { FieldDTO } from "@/common";
export class BlockDTO {
    name;
    title;
    icon;
    purchaseRegistration;
    exportFields;
    constructor(data) {
        Object.assign(this, data);
    }
    /**
     * Parses the section object into a block DTO object
     *
     * @param {Section} section - Form section object
     * @returns {DTOBlock} DTO version of the section object
     */
    static toDTO(section) {
        const { name, purchaseRegistration, fields, title, icon } = section;
        return new BlockDTO({
            name: name,
            purchaseRegistration: purchaseRegistration,
            title: title,
            icon: icon,
            exportFields: [...fields.map(block => { return FieldDTO.fromField(block); })]
        });
    }
}
//# sourceMappingURL=dto-block.js.map