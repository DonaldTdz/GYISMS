export class VisitTask implements IVisitTask {
    id: number;
    name: string;
    type: number;
    isExamine: boolean;
    desc: string;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    typeName: string;
    teName: string;
    teDesc: string;
    teSeq: number;
    constructor(data?: IVisitTask) {
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
            this.name = data["name"];
            this.type = data["type"];
            this.typeName = data["typeName"];
            this.isExamine = data["isExamine"];
            this.desc = data["desc"];
            this.isDeleted = data["isDeleted"];
            this.creationTime = data["creationTime"];
            this.creatorUserId = data["creatorUserId"];
            this.lastModificationTime = data["lastModificationTime"];
            this.lastModifierUserId = data["lastModifierUserId"];
            this.deletionTime = data["deletionTime"];
            this.deleterUserId = data["deleterUserId"];
            this.teName = data["teName"];
            this.teDesc = data["teDesc"];
            this.teSeq = data["teSeq"];
        }
    }

    static fromJS(data: any): VisitTask {
        let result = new VisitTask();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): VisitTask[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new VisitTask();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["type"] = this.type;
        data["typeName"] = this.typeName;
        data["isExamine"] = this.isExamine;
        data["desc"] = this.desc;
        data["isDeleted"] = this.isDeleted;
        data["creationTime"] = this.creationTime;
        data["creatorUserId"] = this.creatorUserId;
        data["lastModificationTime"] = this.lastModificationTime;
        data["lastModifierUserId"] = this.lastModifierUserId;
        data["deletionTime"] = this.deletionTime;
        data["deleterUserId"] = this.deleterUserId;
        data["teName"] = this.teName;
        data["teDesc"] = this.teDesc;
        data["teSeq"] = this.teSeq;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new VisitTask();
        result.init(json);
        return result;
    }
}
export interface IVisitTask {
    id: number;
    name: string;
    type: number;
    typeName: string;
    isExamine: boolean;
    desc: string;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    teName: string;
    teDesc: string;
    teSeq: number;
}