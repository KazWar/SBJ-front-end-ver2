export class Locale {
    id:number
    name:string
    description:string

    constructor(data = {}) {
        Object.assign(this, data)
    }
}