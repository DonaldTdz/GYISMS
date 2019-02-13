import { Component, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ScheduleServiceProxy, VisitTaskServiceProxy } from '@shared/service-proxies/tobacco-management';
import { AppComponentBase } from '@shared/app-component-base';
import { NzFormatEmitEvent, NzTreeNode, NzDropdownContextComponent } from 'ng-zorro-antd';
import { PagedResultDtoOfEmployee, EmployeeServiceProxy, OrganizationServiceProxy, GrowerServiceProxy } from '@shared/service-proxies/basic-data';
import { VisitTask, ScheduleDetail } from '@shared/entity/tobacco-management';
import { Employee, Grower } from '@shared/entity/basic-data';
import { ReturnStatement } from '@angular/compiler';

@Component({
    moduleId: module.id,
    selector: 'assign-task',
    templateUrl: 'assign-task.component.html',
    styleUrls: ['assign-task.component.scss']
})
export class AssignTaskComponent extends AppComponentBase implements OnInit {
    id: string;
    taskId: number;
    visitNum: number;
    scheduleId: string;
    successMsg = '';
    isConfirmLoading = false;
    loading = false;
    dropdown: NzDropdownContextComponent;

    //activedNode: NzTreeNode;
    employeeList: Employee[] = [];
    growerList: Grower[] = [];
    dragNodeElement;
    tempNode: string;
    nodes = [];
    taskList: VisitTask[] = [];
    scheduleDetailList: ScheduleDetail[] = [];
    scheduleDetail: ScheduleDetail = new ScheduleDetail();
    // allChecked = false;
    // disabledButton = true;
    // checkedNumber = 0;
    // indeterminate = false;
    isSelectedAll: boolean = false; // 是否全选
    checkboxCount: number = 0; // 所有Checkbox数量
    checkedLength: number = 0; // 已选中的数量
    checked = true;
    isPush: string = 'false';
    allPercentage: string = "0";
    //defaultSelectedKeys = ['1'];

    constructor(injector: Injector, private taskService: VisitTaskServiceProxy
        , private organizationService: OrganizationServiceProxy
        , private router: Router
        , private actRouter: ActivatedRoute
        , private growerService: GrowerServiceProxy) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
        this.taskId = this.actRouter.snapshot.params['taskId'];
        this.visitNum = this.actRouter.snapshot.params['visitNum'];
        this.scheduleId = this.actRouter.snapshot.params['scheduleId'];
        this.isPush = this.actRouter.snapshot.params['isPush'];
        this.allPercentage = this.actRouter.snapshot.params['allPercentage'];
    }

    ngOnInit(): void {
        this.getTrees();
        //this.refreshData('1');
    }

    isCancelCheck(x: any) {
        this.checkedLength = this.growerList.filter(v => v.checked).length;
        this.checkboxCount = this.growerList.length;
        if (this.checkboxCount - this.checkedLength > 0) {
            this.isSelectedAll = false;
        } else {
            this.isSelectedAll = true;
        }
    }

    checkAll(e) {
        var v = this.isSelectedAll;
        this.growerList.forEach(u => {
            u.checked = v;
        });
        if (this.isSelectedAll == false) {
            this.checkedLength == 0;
        } else {
            this.checkedLength == this.growerList.filter(v => v.checked).length;
        }
    }

    getTrees() {
        this.organizationService.GetEmployeeTreesAsync().subscribe((data) => {
            this.nodes = data;
            if (this.nodes.length > 0) {
                this.tempNode = this.nodes[0].key;
                this.refreshData(this.nodes[0].key);
            }
        });
    }
    /**
     * 获取
     */
    refreshData(employeeId: string) {
        let i: number = 0;
        this.loading = true;
        let params: any = {};
        // params.departId = departId;
        params.TaskId = this.taskId;
        params.ScheduleId = this.scheduleId;
        params.Id = this.id;
        params.EmployeeId = employeeId;
        this.growerService.getGrowerListNoPageAsync(params).subscribe((result: Grower[]) => {
            this.loading = false;
            this.isSelectedAll = false;
            this.growerList = result;

            this.growerList.map(v => {
                if (v.checked == true) {
                    i++;
                }
                if ((!v.visitNum) && (v.visitNum != 0)) v.visitNum = this.visitNum;
            });
            if ((this.growerList.length != 0) && (this.growerList.length == i)) {
                this.isSelectedAll = true;
            }
        });

    }
    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {
        /*/if (this.activedNode) {
            this.activedNode = null;
        }
        data.node.isSelected = true;
        this.activedNode = data.node;*/
        if (this.tempNode == data.node.key) {
            return;
        }
        this.tempNode = data.node.key;
        //data.node.isSelected = true;
        this.refreshData(data.node.key);
    }

    save() {
        // var list = this.growerList.filter(v => v.checked);
        var list = this.growerList;

        let params: any = {};
        params.taskId = this.taskId;
        params.id = this.id;
        params.scheduleId = this.scheduleId;
        // params.visitNum =list.
        // this.scheduleDetailList = ScheduleDetail.fromJSArrayByGrower(list, params);
        list.forEach(v => {
            if (v.scheduleDetailId) {
                this.scheduleDetail.id = v.scheduleDetailId;
            }
            this.scheduleDetail.visitNum = v.visitNum;
            this.scheduleDetail.growerName = v.name;
            this.scheduleDetail.growerId = v.id;
            this.scheduleDetail.taskId = this.taskId;
            this.scheduleDetail.scheduleId = this.scheduleId;
            this.scheduleDetail.scheduleTaskId = this.id;
            this.scheduleDetail.status = 1;
            this.scheduleDetail.completeNum = 0;
            this.scheduleDetail.employeeId = v.employeeId;
            this.scheduleDetail.employeeName = v.employeeName;
            this.scheduleDetail.checked = v.checked;
            this.scheduleDetailList.push(ScheduleDetail.fromJS(this.scheduleDetail));
            this.scheduleDetail = new ScheduleDetail();
        })
        this.isConfirmLoading = true;
        this.successMsg = '任务指派成功';
        this.saveAssignInfo();
    }
    saveAssignInfo() {
        this.taskService.updateScheduleDetail(this.scheduleDetailList).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: any) => {
                this.scheduleDetailList = [];
                result.forEach(x => {
                    this.growerList.forEach(v => {
                        if (v.id == x.growerId) {
                            v.scheduleDetailId = x.id;
                            return;
                        }
                    });
                });
                this.notify.info(this.successMsg, '');
            });
    }

    return() {
        this.router.navigate(['app/task/schedule-detail', this.scheduleId, this.allPercentage]);
    }
}
