export class SheduleStatis implements ISheduleStatis {
    groupName: string;
    total: number;
    completed: number;
    expired: number;
    constructor(data?: ISheduleStatis) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.groupName = data["groupName"];
            this.total = data["total"];
            this.completed = data["completed"];
            this.expired = data["expired"];
        }
    }

    static fromJS(data: any): SheduleStatis {
        let result = new SheduleStatis();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): SheduleStatis[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new SheduleStatis();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["groupName"] = this.groupName;
        data["total"] = this.total;
        data["completed"] = this.completed;
        data["expired"] = this.expired;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new SheduleStatis();
        result.init(json);
        return result;
    }
}
export interface ISheduleStatis {
    groupName: string;
    total: number;
    completed: number;
    expired: number;
}







