export class Campaign {
    name:string
    formId: number
    code:number | undefined
    description:string | undefined
    startDate:Date
    endDate:Date
    thumbnailUrl:string | undefined
    conditionsUrl:string | undefined
    bannerUrl:string | undefined
    category: Category

    constructor(data:Campaign) {
        this.name = data.name
        this.formId = data.formId
        this.code = data.code
        this.description = data.description
        this.startDate = ParseDate(data.endDate)
        this.endDate = ParseDate(data.endDate)
        this.thumbnailUrl = data.thumbnailUrl
        this.conditionsUrl = data.conditionsUrl
        this.bannerUrl = data.bannerUrl
        this.category = data.category
    }
}

const ParseDate = (date:any):Date => {
    return new Date(date)
}

