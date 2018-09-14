import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Employee } from '@shared/entity/basic-data';
import { EmployeeServiceProxy, PagedResultDtoOfEmployee } from '@shared/service-proxies/basic-data';

@Component({
    moduleId: module.id,
    selector: 'grower-employee-modal',
    templateUrl: 'grower-employee-modal.component.html',
})
export class GrowerEmployeeModalComponent implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();

    q: any = {
        pi: 0,
        ps: 10,
        name: ''
    };
    eloading = false;
    isVisible = false;
    employee: Employee[] = [];
    status = [
        { text: '启用', value: false, type: 'success' },
        { text: '禁用', value: false, type: 'default' }
    ];
    constructor(private employeeService: EmployeeServiceProxy) {
    }

    ngOnInit(): void {
    }
    //isManger用判断模态框是否只显示经理级的员工
    show() {
        this.employee = new Array<Employee>();
        this.isVisible = true;
        // this.refreshData();
    }

    /**
     * 获取
     */
    refreshData() {
        if (this.q.name != null && this.q.name.trim().length !== 0) {
            this.eloading = true;
            let params: any = {};
            params.SkipCount = this.q.pi;
            params.MaxResultCount = this.q.ps;
            params.Name = this.q.name;
            params.DepartId = '1';
            this.employeeService.getAll(params).subscribe((result: PagedResultDtoOfEmployee) => {
                this.eloading = false;
                this.employee = result.items;
                this.q.total = result.totalCount;
            });
        }
    }

    /**
     * 取消按钮事件
     */
    handleCancel = (e) => {
        this.isVisible = false;
        this.eloading = false;
        this.q.name = '';
    }
    /**
     * 
     * @param employee 选择事件（对选择的数据进行回传）
     */
    SelectEmployee(employee: Employee): void {
        this.q.name = '';
        this.modalSelect.emit(employee);
        this.isVisible = false;
    }
}
