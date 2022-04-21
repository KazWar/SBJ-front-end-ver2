export class FieldDTO {
    fieldType;
    name;
    label;
    labelList;
    values;
    maxlength;
    required;
    readonly;
    inputmask;
    IsPurchaseRegistration;
    StartDate;
    EndDate;
    RegularExpression;
    RegistrationField;
    PurchaseRegistrationField;
    formFieldId;
    formFieldValueList;
    dropdownList;
    purchaseOptions;
    defaultValuesPurchaseRegistrations;
    constructor(data) {
        Object.assign(this, data);
    }
    static fromField(field) {
        const { id, name, type, label, mask, readonly, required, defaultValue } = field;
        return new FieldDTO({
            name: name,
            fieldType: type,
            label: label,
            labelList: undefined,
            values: defaultValue,
            maxlength: undefined,
            required: required,
            readonly: readonly,
            inputmask: mask,
            IsPurchaseRegistration: undefined,
            StartDate: undefined,
            EndDate: undefined,
            RegularExpression: undefined,
            RegistrationField: undefined,
            PurchaseRegistrationField: undefined,
            formFieldId: id,
            formFieldValueList: undefined,
            dropdownList: undefined,
            purchaseOptions: undefined,
            defaultValuesPurchaseRegistrations: undefined
        });
    }
}
//# sourceMappingURL=dto-field.js.map