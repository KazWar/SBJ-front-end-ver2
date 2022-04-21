import { Field } from "@/common";
export class Section {
    name;
    title;
    icon;
    purchaseRegistration;
    fields;
    constructor(data) {
        Object.assign(this, data);
    }
    /**
     * Parses JSON data to a Section object
     *
     * @param {DTOBlock} dto - Form block data object from the form
     * @returns {Section} Form object
     */
    static fromDTO(dto) {
        const { name, purchaseRegistration, exportFields, title, icon } = dto;
        return new Section({
            name: name,
            title: title,
            icon: icon,
            purchaseRegistration: purchaseRegistration,
            fields: [...exportFields.map(block => { return Field.fromDTO(block); })]
        });
    }
}
//# sourceMappingURL=section.js.map