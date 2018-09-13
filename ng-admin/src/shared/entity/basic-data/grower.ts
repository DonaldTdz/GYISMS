export class Grower implements IGrower {
    id: string;
    year: number;
    unitCode: string;
    unitName: string;
    name: string;
    countyCode: number;
    employeeId: string;
    contractNo: string;
    villageGroup: string;
    tel: string;
    address: string;
    type: number;
    plantingArea: number;
    longitude: number;
    latitude: number;
    contractTime: string;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    constructor(data?: IGrower) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["id"];
            this.year = data["year"];
            this.unitCode = data["unitCode"];
            this.unitName = data["unitName"];
            this.name = data["name"];
            this.countyCode = data["countyCode"];
            this.employeeId = data["employeeId"];
            this.contractNo = data["contractNo"];
            this.villageGroup = data["villageGroup"];
            this.tel = data["tel"];
            this.address = data["address"];
            this.type = data["type"];
            this.plantingArea = data["plantingArea"];
            this.longitude = data["longitude"];
            this.latitude = data["latitude"];
            this.contractTime = data["contractTime"];
            this.isDeleted = data["isDeleted"];
            this.creationTime = data["creationTime"];
            this.creatorUserId = data["creatorUserId"];
            this.lastModificationTime = data["lastModificationTime"];
            this.lastModifierUserId = data["lastModifierUserId"];
            this.deletionTime = data["deletionTime"];
            this.deleterUserId = data["deleterUserId"];
        }
    }

    static fromJS(data: any): Grower {
        let result = new Grower();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): Grower[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new Grower();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["year"] = this.year;
        data["unitCode"] = this.unitCode;
        data["unitName"] = this.unitName;
        data["name"] = this.name;
        data["countyCode"] = this.countyCode;
        data["employeeId"] = this.employeeId;
        data["contractNo"] = this.contractNo;
        data["villageGroup"] = this.villageGroup;
        data["tel"] = this.tel;
        data["address"] = this.address;
        data["type"] = this.type;
        data["plantingArea"] = this.plantingArea;
        data["longitude"] = this.longitude;
        data["latitude"] = this.latitude;
        data["contractTime"] = this.contractTime;
        data["isDeleted"] = this.isDeleted;
        data["creationTime"] = this.creationTime;
        data["creatorUserId"] = this.creatorUserId;
        data["lastModificationTime"] = this.lastModificationTime;
        data["lastModifierUserId"] = this.lastModifierUserId;
        data["deletionTime"] = this.deletionTime;
        data["deleterUserId"] = this.deleterUserId;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Grower();
        result.init(json);
        return result;
    }
}
export interface IGrower {
    id: string;
    year: number;
    unitCode: string;
    unitName: string;
    name: string;
    countyCode: number;
    employeeId: string;
    contractNo: string;
    villageGroup: string;
    tel: string;
    address: string;
    type: number;
    plantingArea: number;
    longitude: number;
    latitude: number;
    contractTime: string;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
}