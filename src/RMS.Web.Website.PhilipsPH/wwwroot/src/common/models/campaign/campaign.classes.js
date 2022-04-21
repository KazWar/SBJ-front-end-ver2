export class Campaign {
    id;
    code;
    name;
    description;
    thumbnail;
    startDate;
    endDate;
    localeId;
    version;
    constructor(data = {}) {
        Object.assign(this, data);
    }
}
//# sourceMappingURL=campaign.classes.js.map