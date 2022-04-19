import { ValueDTO, Field } from "@/common"
import { retailer } from "./dto-retailer"

export class FieldDTO {
    fieldType:string
    name:string
    label?:string | undefined
    labelList?:Array<string> | undefined
    values?:string | undefined
    maxlength?:number | undefined
    required:boolean
    readonly:boolean
    inputmask?:string | undefined
    IsPurchaseRegistration?:boolean | undefined
    StartDate?:Date | undefined
    EndDate?:Date | undefined
    RegularExpression?:string | undefined 
    RegistrationField?:string | undefined
    PurchaseRegistrationField?:string | undefined
    formFieldId:number
    formFieldValueList?:Array<ValueDTO> | undefined
    dropdownList?:Array<retailer> | undefined
    purchaseOptions?:Array<object> | undefined
    defaultValuesPurchaseRegistrations?:Array<string> | undefined

    constructor(data: FieldDTO) {
        Object.assign(this, data)
    }
    
    static fromField(field:Field): FieldDTO {
        const { id, name, type, label, mask, 
            readonly, required, defaultValue } = field

        return new FieldDTO({
            name: name,
            fieldType: type,
            label: label,
            labelList : undefined,
            values: defaultValue,
            maxlength: undefined,
            required: required,
            readonly: readonly,
            inputmask: mask,
            IsPurchaseRegistration: undefined,
            StartDate: undefined,
            EndDate: undefined,
            RegularExpression :undefined,
            RegistrationField:undefined,
            PurchaseRegistrationField:undefined,
            formFieldId: id,
            formFieldValueList:undefined,
            dropdownList:undefined,
            purchaseOptions:undefined,
            defaultValuesPurchaseRegistrations:undefined
        }) 
    }
}
