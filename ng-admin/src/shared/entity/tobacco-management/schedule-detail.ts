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
    taskName: string;
    percentage: string;
    statusName: string;
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
            this.taskName = data["taskName"];
            this.percentage = data["percentage"];
            this.statusName = data["statusName"];
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
    taskName: string;
    percentage: string;
    statusName: string;
}


export class ScheduleSum implements IScheduleSum {
    areaCode: number;
    taskName: string;
    taskType: number;
    total: number;
    complete: number;
    expired: number;
    beginTime: Date;
    areaName: string;
    taskTypeName: string;
    completeRate: string;
    time: string;
    sheduleName: string;
    constructor(data?: IScheduleSum) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.areaCode = data["areaCode"];
            this.taskName = data["taskName"];
            this.taskType = data["taskType"];
            this.total = data["total"];
            this.complete = data["complete"];
            this.expired = data["expired"];
            this.beginTime = data["beginTime"];
            this.areaName = data["areaName"];
            this.taskTypeName = data["taskTypeName"];
            this.completeRate = data["completeRate"];
            this.sheduleName = data["sheduleName"];
            this.time = data["time"];
        }
    }

    static fromJS(data: any): ScheduleSum {
        let result = new ScheduleSum();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): ScheduleSum[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ScheduleSum();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["areaCode"] = this.areaCode;
        data["taskName"] = this.taskName;
        data["taskType"] = this.taskType;
        data["total"] = this.total;
        data["complete"] = this.complete;
        data["expired"] = this.expired;
        data["beginTime"] = this.beginTime;
        data["areaName"] = this.areaName;
        data["taskTypeName"] = this.taskTypeName;
        data["completeRate"] = this.completeRate;
        data["sheduleName"] = this.sheduleName;
        data["time"] = this.time;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ScheduleSum();
        result.init(json);
        return result;
    }
}
export interface IScheduleSum {
    areaCode: number;
    taskName: string;
    taskType: number;
    total: number;
    complete: number;
    expired: number;
    beginTime: Date;
    areaName: string;
    taskTypeName: string;
    completeRate: string;
    time: string;
    sheduleName: string;
}


export class ScheduleDetailTask implements IScheduleDetailTask {
    id: string;
    areaCode: number;
    taskName: string;
    taskType: number;
    visitNum: number;
    completeNum: number;
    expired: number;
    status: number;
    employeeName: string;
    growerId: number;
    growerName: string;
    areaName: string;
    typeName: string;
    statusName: string;
    time: string;
    sheduleName: string;
    constructor(data?: IScheduleDetailTask) {
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
            this.areaCode = data["areaCode"];
            this.taskName = data["taskName"];
            this.taskType = data["taskType"];
            this.visitNum = data["visitNum"];
            this.completeNum = data["completeNum"];
            this.expired = data["expired"];
            this.status = data["status"];
            this.employeeName = data["employeeName"];
            this.growerName = data["growerName"];
            this.areaName = data["areaName"];
            this.typeName = data["typeName"];
            this.statusName = data["statusName"];
            this.growerId = data["growerId"];
            this.time = data["time"];
            this.sheduleName = data["sheduleName"];
        }
    }

    static fromJS(data: any): ScheduleDetailTask {
        let result = new ScheduleDetailTask();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): ScheduleDetailTask[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ScheduleDetailTask();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["areaCode"] = this.areaCode;
        data["taskName"] = this.taskName;
        data["taskType"] = this.taskType;
        data["visitNum"] = this.visitNum;
        data["completeNum"] = this.completeNum;
        data["expired"] = this.expired;
        data["status"] = this.status;
        data["employeeName"] = this.employeeName;
        data["growerName"] = this.growerName;
        data["areaName"] = this.areaName;
        data["typeName"] = this.typeName;
        data["statusName"] = this.statusName;
        data["growerId"] = this.growerId;
        data["time"] = this.time;
        data["sheduleName"] = this.sheduleName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ScheduleDetailTask();
        result.init(json);
        return result;
    }
}
export interface IScheduleDetailTask {
    id: string;
    areaCode: number;
    taskName: string;
    taskType: number;
    visitNum: number;
    completeNum: number;
    expired: number;
    status: number;
    employeeName: string;
    growerId: number;
    growerName: string;
    areaName: string;
    typeName: string;
    statusName: string;
    time: string;
}


