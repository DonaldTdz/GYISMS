export class Advise implements IAdvise {
    documentName: string;
    categoryName: string;
    employeeName: number;
    content: number;
    creationTime: string;

    constructor(data?: IAdvise) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.documentName = data["documentName"];
            this.categoryName = data["categoryName"];
            this.employeeName = data["employeeName"];
            this.content = data["content"];
            this.creationTime = data["creationTime"];
        }
    }

    static fromJS(data: any): Advise {
        let result = new Advise();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): Advise[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new Advise();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["documentName"] = this.documentName;
        data["categoryName"] = this.categoryName;
        data["employeeName"] = this.employeeName;
        data["content"] = this.content;
        data["creationTime"] = this.creationTime;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Advise();
        result.init(json);
        return result;
    }
}
export interface IAdvise {
    documentName: string;
    categoryName: string;
    employeeName: number;
    content: number;
    creationTime: string;
}