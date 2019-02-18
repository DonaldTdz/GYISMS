export class GrowerAreaRecord implements IGrowerAreaRecord {
    id: string;
    growerId: number;
    scheduleDetailId: string;
    imgPath: string;
    longitude: number;
    latitude: number;
    location: string;
    employeeName: string;
    employeeId: string;
    collectionTime: Date;
    area: number;
    remark: string;
    scheduleName: string;
    imgPaths: string[];
    imgTop: string;

    constructor(data?: IGrowerAreaRecord) {
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
            this.growerId = data["growerId"];
            this.scheduleDetailId = data["scheduleDetailId"];
            this.imgPath = data["imgPath"];
            this.longitude = data["longitude"];
            this.latitude = data["latitude"];
            this.location = data["location"];
            this.employeeName = data["employeeName"];
            this.employeeId = data["employeeId"];
            this.collectionTime = data["collectionTime"];
            this.area = data["area"];
            this.remark = data["remark"];
            this.scheduleName = data["scheduleName"];
            this.imgPaths = data["imgPaths"];
            this.imgTop = data["imgTop"];
        }
    }

    static fromJS(data: any): GrowerAreaRecord {
        let result = new GrowerAreaRecord();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): GrowerAreaRecord[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new GrowerAreaRecord();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["growerId"] = this.growerId;
        data["scheduleDetailId"] = this.scheduleDetailId;
        data["imgPath"] = this.imgPath;
        data["longitude"] = this.longitude;
        data["latitude"] = this.latitude;
        data["location"] = this.location;
        data["employeeName"] = this.employeeName;
        data["employeeId"] = this.employeeId;
        data["collectionTime"] = this.collectionTime;
        data["area"] = this.area;
        data["remark"] = this.remark;
        data["imgPaths"] = this.imgPaths;
        data["imgTop"] = this.imgTop;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new GrowerAreaRecord();
        result.init(json);
        return result;
    }
}
export interface IGrowerAreaRecord {
    id: string;
    growerId: number;
    scheduleDetailId: string;
    imgPath: string;
    longitude: number;
    latitude: number;
    location: string;
    employeeName: string;
    employeeId: string;
    collectionTime: Date;
    area: number;
    remark: string;
    scheduleName: string;
    imgPaths: string[];
    imgTop: string;
}