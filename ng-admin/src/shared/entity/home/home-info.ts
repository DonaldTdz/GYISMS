export class HomeInfo implements IHomeInfo {
    total: number;
    completed: number;
    completedRate: string;
    expired: number;
    constructor(data?: IHomeInfo) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.total = data["total"];
            this.completed = data["completed"];
            this.completedRate = data["completedRate"];
            this.expired = data["expired"];
        }
    }

    static fromJS(data: any): HomeInfo {
        let result = new HomeInfo();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): HomeInfo[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new HomeInfo();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["total"] = this.total;
        data["completed"] = this.completed;
        data["completedRate"] = this.completedRate;
        data["expired"] = this.expired;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new HomeInfo();
        result.init(json);
        return result;
    }
}
export interface IHomeInfo {
    total: number;
    completed: number;
    completedRate: string;
    expired: number;
}







