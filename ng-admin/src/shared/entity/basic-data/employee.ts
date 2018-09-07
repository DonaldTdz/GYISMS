export class Employee implements IEmployee {
    id: string;
    openId: string;
    name: string;
    mobile: string;
    email: string;
    active: boolean;
    isAdmin: boolean;
    isBoss: boolean;
    department: string;
    position: string;
    avatar: string;
    hiredDate: string;
    roles: string;
    roleId: number;
    remark: string;
    constructor(data?: IEmployee) {
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
            this.openId = data["openId"];
            this.name = data["name"];
            this.mobile = data["mobile"];
            this.email = data["email"];
            this.active = data["active"];
            this.isAdmin = data["isAdmin"];
            this.isBoss = data["isBoss"];
            this.department = data["department"];
            this.position = data["position"];
            this.avatar = data["avatar"];
            this.hiredDate = data["hiredDate"];
            this.roles = data["roles"];
            this.roleId = data["roleId"];
            this.remark = data["remark"];
        }
    }

    static fromJS(data: any): Employee {
        let result = new Employee();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): Employee[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new Employee();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["openId"] = this.openId;
        data["name"] = this.name;
        data["mobile"] = this.mobile;
        data["email"] = this.email;
        data["active"] = this.active;
        data["isAdmin"] = this.isAdmin;
        data["isBoss"] = this.isBoss;
        data["department"] = this.department;
        data["position"] = this.position;
        data["avatar"] = this.avatar;
        data["hiredDate"] = this.hiredDate;
        data["roles"] = this.roles;
        data["roleId"] = this.roleId;
        data["remark"] = this.remark;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Employee();
        result.init(json);
        return result;
    }
}
export interface IEmployee {
    id: string;
    openId: string;
    name: string;
    mobile: string;
    email: string;
    active: boolean;
    isAdmin: boolean;
    isBoss: boolean;
    department: string;
    position: string;
    avatar: string;
    hiredDate: string;
    roles: string;
    roleId: number;
    remark: string;
}