export class VisitRecord implements IVisitRecord {
    id: string;
    scheduleDetailId: string;
    employeeId: string;
    growerId: number;
    signTime: Date;
    location: string;
    longitude: number;
    latitude: number;
    desc: string;
    imgPath: string;
    creationTime: Date;
    employeeName: string;
    taskName: string;
    examinesName: string;
    imgPaths: string[];
    imgTop: string;
    imgPathArry: string[];

    constructor(data?: IVisitRecord) {
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
            this.scheduleDetailId = data["scheduleDetailId"];
            this.employeeId = data["employeeId"];
            this.growerId = data["growerId"];
            this.signTime = data["signTime"];
            this.location = data["location"];
            this.longitude = data["longitude"];
            this.latitude = data["latitude"];
            this.desc = data["desc"];
            this.imgPath = data["imgPath"];
            this.creationTime = data["creationTime"];
            this.employeeName = data["employeeName"];
            this.taskName = data["taskName"];
            this.examinesName = data["examinesName"];
            this.imgPaths = data["imgPaths"];
            this.imgTop = data["imgTop"];
            this.imgPathArry = data["imgPathArry"];
        }
    }

    static fromJS(data: any): VisitRecord {
        let result = new VisitRecord();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): VisitRecord[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new VisitRecord();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["scheduleDetailId"] = this.scheduleDetailId;
        data["employeeId"] = this.employeeId;
        data["growerId"] = this.growerId;
        data["signTime"] = this.signTime;
        data["location"] = this.location;
        data["longitude"] = this.longitude;
        data["latitude"] = this.latitude;
        data["desc"] = this.desc;
        data["imgPath"] = this.imgPath;
        data["creationTime"] = this.creationTime;
        data["imgPaths"] = this.imgPaths;
        data["imgTop"] = this.imgTop;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new VisitRecord();
        result.init(json);
        return result;
    }
}
export interface IVisitRecord {
    id: string;
    scheduleDetailId: string;
    employeeId: string;
    growerId: number;
    signTime: Date;
    location: string;
    longitude: number;
    latitude: number;
    desc: string;
    imgPath: string;
    creationTime: Date;
    employeeName: string;
    taskName: string;
    examinesName: string;
    imgPaths: string[];
    imgTop: string;
    imgPathArry: string[];
}