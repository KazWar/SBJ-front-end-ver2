export class DropdownListDto {
    retailerAddress:string
    retailerLocationId:string
    
    constructor(data:any) {
        Object.assign(this, data)
    }
}