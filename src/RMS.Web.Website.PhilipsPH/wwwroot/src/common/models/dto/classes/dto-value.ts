export class ValueDTO {
    ListValueTranslationKeyValue:string
    ListValueTranslationDescription:string
    HandlingLine?:Array<string> | undefined

    constructor (
        listValueTranslationKeyValue:string,
        listValueTranslationDescription:string, 
        handlingLine:Array<string>)
        {
            this.ListValueTranslationKeyValue = listValueTranslationKeyValue
            this.ListValueTranslationDescription = listValueTranslationDescription
            this.HandlingLine = handlingLine
    }
}
