export class ScheduleTask implements IScheduleTask {
    id: string;
    taskId: number;
    scheduleId: string;
    visitNum: number;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    taskName: string;
    isExamine: boolean;
    typeName: string;
    constructor(data?: IScheduleTask) {
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
            this.taskId = data["taskId"];
            this.scheduleId = data["scheduleId"];
            this.visitNum = data["visitNum"];
            this.isDeleted = data["isDeleted"];
            this.creationTime = data["creationTime"];
            this.creatorUserId = data["creatorUserId"];
            this.lastModificationTime = data["lastModificationTime"];
            this.lastModifierUserId = data["lastModifierUserId"];
            this.deletionTime = data["deletionTime"];
            this.deleterUserId = data["deleterUserId"];
            this.taskName = data["taskName"];
            this.isExamine = data["isExamine"];
            this.typeName = data["typeName"];
        }
    }

    static fromJS(data: any): ScheduleTask {
        let result = new ScheduleTask();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): ScheduleTask[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ScheduleTask();
            item.init(result);
            array.push(item);
        });

        return array;
    }
    static fromVisitTaskJSArray(dataArray: any[], sid?: string): ScheduleTask[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ScheduleTask();
            item.init(result);
            if (sid) {
                item.scheduleId = sid;
            }
            item.taskId = result.id;
            item.id = result.scheduleTaskId;
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["taskId"] = this.taskId;
        data["scheduleId"] = this.scheduleId;
        data["visitNum"] = this.visitNum;
        data["isDeleted"] = this.isDeleted;
        data["creationTime"] = this.creationTime;
        data["creatorUserId"] = this.creatorUserId;
        data["lastModificationTime"] = this.lastModificationTime;
        data["lastModifierUserId"] = this.lastModifierUserId;
        data["deletionTime"] = this.deletionTime;
        data["deleterUserId"] = this.deleterUserId;
        data["taskName"] = this.taskName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ScheduleTask();
        result.init(json);
        return result;
    }
}
export interface IScheduleTask {
    id: string;
    taskId: number;
    scheduleId: string;
    visitNum: number;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    taskName: string;
    isExamine: boolean;
    typeName: string;
}