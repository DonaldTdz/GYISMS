export class TaskExamine implements ITaskExamineom {
    id: number;
    taskId: number;
    name: string;
    desc: string;
    seq: number;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    isDeleted: boolean;
    examineOption: number;
    examineOptionDesc: string;
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
            this.creatorUserId = data["creatorUserId"];
            this.lastModificationTime = data["lastModificationTime"];
            this.lastModifierUserId = data["lastModifierUserId"];
            this.deletionTime = data["deletionTime"];
            this.deleterUserId = data["deleterUserId"];
            this.isDeleted = data["isDeleted"];
            this.examineOption = data["examineOption"];
            this.examineOptionDesc = data["examineOptionDesc"];
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
        data["creatorUserId"] = this.creatorUserId;
        data["lastModificationTime"] = this.lastModificationTime;
        data["lastModifierUserId"] = this.lastModifierUserId;
        data["deletionTime"] = this.deletionTime;
        data["deleterUserId"] = this.deleterUserId;
        data["isDeleted"] = this.isDeleted;
        data["examineOption"] = this.examineOption;
        data["examineOptionDesc"] = this.examineOptionDesc;
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
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    isDeleted: boolean;
    examineOption: number;
    examineOptionDesc: string;
}