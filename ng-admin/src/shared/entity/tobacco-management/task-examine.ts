export class TaskExamine implements ITaskExamineom {
    id: number;
    taskId: number;
    name: string;
    desc: string;
    seq: number;
    creationTime: Date;
    constructor(data?: ITaskExamineom) {
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
            this.name = data["name"];
            this.desc = data["desc"];
            this.seq = data["seq"];
            this.creationTime = data["creationTime"];
        }
    }

    static fromJS(data: any): TaskExamine {
        let result = new TaskExamine();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): TaskExamine[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new TaskExamine();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["taskId"] = this.taskId;
        data["name"] = this.name;
        data["desc"] = this.desc;
        data["seq"] = this.seq;
        data["creationTime"] = this.creationTime;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new TaskExamine();
        result.init(json);
        return result;
    }
}
export interface ITaskExamineom {
    id: number;
    taskId: number;
    name: string;
    desc: string;
    seq: number;
    creationTime: Date;
}