export class Attachment implements IAttachment {
    id: string;
    name: string;
    fileType: number;
    fileSize: number;
    path: string;
    docId: string;
    creationTime: string;
    fileTypeName: string;

    constructor(data?: IAttachment) {
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
            this.fileType = data["fileType"];
            this.fileSize = data["fileSize"];
            this.path = data["path"];
            this.docId = data["docId"];
            this.creationTime = data["creationTime"];
            this.fileTypeName = data["fileTypeName"];
        }
    }

    static fromJS(data: any): Attachment {
        let result = new Attachment();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): Attachment[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new Attachment();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["fileType"] = this.fileType;
        data["fileSize"] = this.fileSize;
        data["path"] = this.path;
        data["docId"] = this.docId;
        data["creationTime"] = this.creationTime;
        data["fileTypeName"] = this.fileTypeName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Attachment();
        result.init(json);
        return result;
    }
}
export interface IAttachment {
    id: string;
    name: string;
    fileType: number;
    fileSize: number;
    path: string;
    docId: string;
    fileTypeName: string;
}