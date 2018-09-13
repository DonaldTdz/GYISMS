export class MeetingRoom implements IMeetingRoom {
    id: number;
    name: string;
    photo: string;
    num: number;
    roomType: number;
    address: string;
    buildDesc: string;
    isApprove: boolean;
    managerId: string;
    managerName: string;
    row: number;
    column: number;
    layoutPattern: number;
    planPath: string;
    remark: string;
    devices: string;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    showPhoto: string;
    layoutName: string;
    typeName: string;

    constructor(data?: IMeetingRoom) {
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
            this.photo = data["photo"];
            this.num = data["num"];
            this.roomType = data["roomType"];
            this.address = data["address"];
            this.buildDesc = data["buildDesc"];
            this.isApprove = data["isApprove"];
            this.managerId = data["managerId"];
            this.managerName = data["managerName"];
            this.row = data["row"];
            this.column = data["column"];
            this.layoutPattern = data["layoutPattern"];
            this.planPath = data["planPath"];
            this.remark = data["remark"];
            this.devices = data["devices"];
            this.isDeleted = data["isDeleted"];
            this.creationTime = data["creationTime"];
            this.creatorUserId = data["creatorUserId"];
            this.lastModificationTime = data["lastModificationTime"];
            this.lastModifierUserId = data["lastModifierUserId"];
            this.deletionTime = data["deletionTime"];
            this.deleterUserId = data["deleterUserId"];
            this.layoutName = data["layoutName"];
            this.typeName = data["typeName"];
        }
    }

    static fromJS(data: any): MeetingRoom {
        let result = new MeetingRoom();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): MeetingRoom[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new MeetingRoom();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["photo"] = this.photo;
        data["num"] = this.num;
        data["roomType"] = this.roomType;
        data["address"] = this.address;
        data["buildDesc"] = this.buildDesc;
        data["isApprove"] = this.isApprove;
        data["managerId"] = this.managerId;
        data["managerName"] = this.managerName;
        data["row"] = this.row;
        data["column"] = this.column;
        data["layoutPattern"] = this.layoutPattern;
        data["planPath"] = this.planPath;
        data["remark"] = this.remark;
        data["devices"] = this.devices;
        data["isDeleted"] = this.isDeleted;
        data["creationTime"] = this.creationTime;
        data["creatorUserId"] = this.creatorUserId;
        data["lastModificationTime"] = this.lastModificationTime;
        data["lastModifierUserId"] = this.lastModifierUserId;
        data["deletionTime"] = this.deletionTime;
        data["deleterUserId"] = this.deleterUserId;
        data["layoutName"] = this.layoutName;
        data["typeName"] = this.typeName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new MeetingRoom();
        result.init(json);
        return result;
    }
}
export interface IMeetingRoom {
    id: number;
    name: string;
    photo: string;
    num: number;
    roomType: number;
    address: string;
    buildDesc: string;
    isApprove: boolean;
    managerId: string;
    managerName: string;
    row: number;
    column: number;
    layoutPattern: number;
    planPath: string;
    remark: string;
    devices: string;
    isDeleted: boolean;
    creationTime: Date;
    creatorUserId: number;
    lastModificationTime: Date;
    lastModifierUserId: number;
    deletionTime: Date;
    deleterUserId: number;
    showPhoto: string;
    layoutName: string;
    typeName: string;
}

export class CheckBoxList implements ICheckBoxList {
    label: string;
    value: string;
    checked: boolean;
    constructor(data?: ICheckBoxList) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.label = data["label"];
            this.value = data["value"];
        }
    }

    static fromJS(data: any): CheckBoxList {
        let result = new CheckBoxList();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): CheckBoxList[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new CheckBoxList();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["label"] = this.label;
        data["value"] = this.value;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new CheckBoxList();
        result.init(json);
        return result;
    }
}
export interface ICheckBoxList {
    label: string;
    value: string;
    checked: boolean;
}