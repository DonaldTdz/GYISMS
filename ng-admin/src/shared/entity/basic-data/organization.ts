export class Organization implements IOrganization {
    id: number;
    departmentName: string;
    parentId: number;
    order: number;
    deptHiding: boolean;
    orgDeptOwner: string;
    creationTime: Date;
    constructor(data?: IOrganization) {
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
            this.departmentName = data["departmentName"];
            this.parentId = data["parentId"];
            this.order = data["order"];
            this.deptHiding = data["deptHiding"];
            this.orgDeptOwner = data["orgDeptOwner"];
            this.creationTime = data["creationTime"];
        }
    }

    static fromJS(data: any): Organization {
        let result = new Organization();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): Organization[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new Organization();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["departmentName"] = this.departmentName;
        data["parentId"] = this.parentId;
        data["order"] = this.order;
        data["deptHiding"] = this.deptHiding;
        data["orgDeptOwner"] = this.orgDeptOwner;
        data["creationTime"] = this.creationTime;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Organization();
        result.init(json);
        return result;
    }
}
export interface IOrganization {
    id: number;
    departmentName: string;
    parentId: number;
    order: number;
    deptHiding: boolean;
    orgDeptOwner: string;
    creationTime: Date;
}

export class TreeNode implements ITreeNode {
    title: string;
    key: string;
    children: TreeNode[]
    constructor(data?: ITreeNode) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.title = data["title"];
            this.key = data["key"];
            this.children = data["children"];
        }
    }

    static fromJS(data: any): TreeNode {
        let result = new TreeNode();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): TreeNode[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new TreeNode();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["title"] = this.title;
        data["key"] = this.key;
        data["children"] = this.children;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new TreeNode();
        result.init(json);
        return result;
    }
}
export interface ITreeNode {
    title: string;
    key: string;
    children: TreeNode[]
}



