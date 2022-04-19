import { BlockDTO } from "@/common"

export class FormDTO {
    formLocaleId:number
    formId:number
    companyName:string
    countryCode:number
    languageCode:string
    version:string
    exportBlocks:Array<BlockDTO>
}