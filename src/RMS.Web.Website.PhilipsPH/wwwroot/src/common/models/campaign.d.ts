type Campaign = {
    Name:string
    Description:string | undefined
    StartDate:Date
    EndDate:Date
    ThumbnailUrl:string | undefined
    ConditionsUrl:string | undefined
    BannerUrl:string | undefined
    Category: Category
}

type Category = {
    id:number
    name:string
    description:string
}