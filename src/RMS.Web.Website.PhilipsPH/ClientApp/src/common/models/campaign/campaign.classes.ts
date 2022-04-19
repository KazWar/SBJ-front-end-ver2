export class Campaign  {
    id: number
    code: number
    name: string
    description: string
    thumbnail: string
    startDate: Date
    endDate: Date
    localeId: number
    version:number

    constructor(data = {}) {
		Object.assign(this, data)
	}
}