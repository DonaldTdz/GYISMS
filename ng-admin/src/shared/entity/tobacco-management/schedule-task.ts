export class ScheduleTask implements IScheduleTaskom {
    id: string;
    taskId: number;
    scheduleId: string;
    visitNum: number;
    creationTime: Date;
    taskName: string;
    constructor(data?: IScheduleTaskom) {
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
            this.creationTime = data["creationTime"];
            this.taskName = data["taskName"];
        }
    }

    static fromJS(data: any): ScheduleTask {
        let result = new ScheduleTask();
        result.init(data);
        return result;
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
            item.id = null;
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
        data["creationTime"] = this.creationTime;
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
export interface IScheduleTaskom {
    id: string;
    taskId: number;
    scheduleId: string;
    visitNum: number;
    creationTime: Date;
    taskName: string;
}