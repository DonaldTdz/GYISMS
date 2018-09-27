export class ScheduleDetail implements IScheduleDetail {
    id: string;
    taskId: number;
    scheduleId: string;
    employeeId: string;
    growerId: any;
    visitNum: number;
    completeNum: number;
    creationTime: Date;
    status: number;
    scheduleTaskId: string;
    employeeName: string;
    growerName: string;
    checked: boolean;
    constructor(data?: IScheduleDetail) {
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
            this.employeeId = data["employeeId"];
            this.growerId = data["growerId"];
            this.visitNum = data["visitNum"];
            this.completeNum = data["completeNum"];
            this.creationTime = data["creationTime"];
            this.status = data["status"];
            this.scheduleTaskId = data["scheduleTaskId"];
            this.employeeName = data["employeeName"];
            this.growerName = data["growerName"];
            this.checked = data["checked"];
        }
    }

    static fromJS(data: any): ScheduleDetail {
        let result = new ScheduleDetail();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): ScheduleDetail[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ScheduleDetail();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    static fromJSArrayByGrower(dataArray: any[], param: any): ScheduleDetail[] {
        // static fromJSArrayByGrower(dataArray: any[],taskId: number, scheduleTaskId: string, scheduleId: string): ScheduleDetail[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ScheduleDetail();
            let grower = new ScheduleDetail();
            grower.init(result);
            // item.init(result);
            item.taskId = param.taskId;
            item.growerId = result.id;
            item.scheduleId = param.scheduleId;
            item.scheduleTaskId = param.id;
            item.growerName = result.name;
            item.completeNum = 0;
            item.status = 1;
            item.employeeId = "999";
            item.employeeName = "测试烟技员"
            item.visitNum = grower.visitNum;
            item.id;
            item.creationTime;
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["taskId"] = this.taskId;
        data["scheduleId"] = this.scheduleId;
        data["employeeId"] = this.employeeId;
        data["growerId"] = this.growerId;
        data["visitNum"] = this.visitNum;
        data["completeNum"] = this.completeNum;
        data["creationTime"] = this.creationTime;
        data["status"] = this.status;
        data["scheduleTaskId"] = this.scheduleTaskId;
        data["employeeName"] = this.employeeName;
        data["growerName"] = this.growerName;
        data["checked"] = this.checked;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ScheduleDetail();
        result.init(json);
        return result;
    }
}
export interface IScheduleDetail {
    id: string;
    taskId: number;
    scheduleId: string;
    employeeId: string;
    growerId: any;
    visitNum: number;
    completeNum: number;
    creationTime: Date;
    status: number;
    scheduleTaskId: string;
    employeeName: string;
    growerName: string;
    checked: boolean;
}