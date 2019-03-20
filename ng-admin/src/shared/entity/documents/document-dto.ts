import { Advise } from "./advise";

export class DocumentDto implements IDocumentDto {
    id: string;
    name: string;
    categoryId: number;
    categoryDesc: string;
    deptIds: string;
    employeeIds: string;
    summary: string;
    content: string;
    releaseDate: Date;
    qrCodeUrl: string;
    creationTime: Date;
    deptDesc: string;
    employeeDes: string;
    isAllUser: boolean;
    docRoleIds: string;
    docRoleDesc: string;
    constructor(data?: IDocumentDto) {
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
            this.categoryId = data["categoryId"];
            this.categoryDesc = data["categoryDesc"];
            this.deptIds = data["deptIds"];
            this.employeeIds = data["employeeIds"];
            this.summary = data["summary"];
            this.content = data["content"];
            this.releaseDate = data["releaseDate"];
            this.qrCodeUrl = data["qrCodeUrl"];
            this.creationTime = data["creationTime"];
            this.deptDesc = data["deptDesc"];
            this.employeeDes = data["employeeDes"];
            this.isAllUser = data["isAllUser"];
            this.docRoleIds = data["docRoleIds"];
            this.docRoleDesc = data["docRoleDesc"];
        }
    }

    static fromJS(data: any): DocumentDto {
        let result = new DocumentDto();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): DocumentDto[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new DocumentDto();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["categoryId"] = this.categoryId;
        data["categoryDesc"] = this.categoryDesc;
        data["deptIds"] = this.deptIds;
        data["employeeIds"] = this.employeeIds;
        data["summary"] = this.summary;
        data["content"] = this.content;
        data["releaseDate"] = this.releaseDate;
        data["qrCodeUrl"] = this.qrCodeUrl;
        data["creationTime"] = this.creationTime;
        data["deptDesc"] = this.deptDesc;
        data["employeeDes"] = this.employeeDes;
        data["isAllUser"] = this.isAllUser;
        data["docRoleIds"] = this.docRoleIds;
        data["docRoleDesc"] = this.docRoleDesc;
        return data;
    }

    getDepts(): any[] {
        let depts = [];
        let ids = this.deptIds.split(',');
        let names = this.deptDesc.split(',');
        let i = 0;
        for (let id of ids) {
            if (id) {
                depts.push({ id: id, name: names[i] });
            }
            i++;
        }
        return depts;
    }

    getUsers(): any[] {
        let users = [];
        let ids = this.employeeIds.split(',');
        let names = this.employeeDes.split(',');
        let i = 0;
        for (let id of ids) {
            if (id) {
                users.push({ id: id, name: names[i] });
            }
            i++;
        }
        return users;
    }

    getRoles(): any[] {
        let roles = [];
        let ids = this.docRoleIds.split(',');
        let labels = this.docRoleDesc.split(',');
        let i = 0;
        for (let id of ids) {
            if (id) {
                roles.push({ value: id, label: labels[i] });
            }
            i++;
        }
        return roles;
    }

    clone() {
        const json = this.toJSON();
        let result = new DocumentDto();
        result.init(json);
        return result;
    }
}
export interface IDocumentDto {
    id: string;
    name: string;
    categoryId: number;
    categoryDesc: string;
    deptIds: string;
    employeeIds: string;
    summary: string;
    content: string;
    releaseDate: Date;
    qrCodeUrl: string;
    creationTime: Date;
    deptDesc: string;
    employeeDes: string;
    isAllUser: boolean;
    docRoleIds: string;
    docRoleDesc: string;
}

export class PagedResultDtoOfDocument implements IPagedResultDtoOfDocument {
    totalCount: number;
    items: DocumentDto[];

    constructor(data?: IPagedResultDtoOfDocument) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(DocumentDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfDocument {
        let result = new PagedResultDtoOfDocument();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfDocument();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfDocument {
    totalCount: number;
    items: DocumentDto[];
}

export class PagedResultDtoOfAdvise implements IPagedResultDtoOfAdvise {
    totalCount: number;
    items: Advise[];

    constructor(data?: IPagedResultDtoOfAdvise) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(Advise.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfAdvise {
        let result = new PagedResultDtoOfAdvise();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfAdvise();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfAdvise {
    totalCount: number;
    items: Advise[];
}