/* eslint-disable @typescript-eslint/no-unused-vars */
import { FieldDTO, Value, DropdownListDto } from "@/common"

export class Field {
    id:number
    name:string
    type: string
    label?: string | undefined
    mask?: string | undefined
    readonly: boolean
    required: boolean
    defaultValue?: string | undefined
    options?: Array<Value> | Array<DropdownListDto> | undefined
    PO?: Array<any> | undefined

    constructor(data:Field) {
        Object.assign(this, data)
    }

    /**
     * Parses JSON data to a Field object
     * 
     * @param {DTO} dto - Response content data as valid DTOField type
     * @returns {Field} Field object
     */
    static fromDTO = (dto:FieldDTO): Field => {
        //* Not all of these are required, but just filler for the moment
        const { fieldType, name, label, labelList, values, maxlength, 
            required, readonly, inputmask, IsPurchaseRegistration, StartDate, 
            EndDate, RegularExpression, RegistrationField, PurchaseRegistrationField, 
            formFieldId, formFieldValueList, dropdownList, purchaseOptions,
            defaultValuesPurchaseRegistrations } = dto
        
        return new Field({
            id:formFieldId,
            name:name,
            type: fieldType,
            label: label,
            mask:inputmask,
            readonly: readonly,
            required: required,
            defaultValue: values,
            options: (dto.formFieldValueList?.length || [].length ) > 0 ?
                    [...formFieldValueList?.map(value => { return Value.fromDTO(value)})|| []] :
                    [...dropdownList?.map((retailer:any) => {
                        return new DropdownListDto(retailer)
                    }) || []],
            PO: ((dto.formFieldValueList?.length || [].length) > 0 || dto.formFieldValueList === undefined)? purchaseOptions?.map((value:any) => { return [...value.formFieldValueList[0].HandlingLine]})[0] : []
        })
    }
}

