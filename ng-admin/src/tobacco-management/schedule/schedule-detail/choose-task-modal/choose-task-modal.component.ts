import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { VisitTaskServiceProxy, PagedResultDtoOfVisitTask } from '@shared/service-proxies/tobacco-management';
import { VisitTask } from '@shared/entity/tobacco-management';
@Component({
    moduleId: module.id,
    selector: 'choose-task-modal',
    templateUrl: 'choose-task-modal.component.html',
    styleUrls: ['choose-task-modal.component.scss']
})

export class ChooseTaskModalComponent implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    q: any = {
        pi: 0,
        ps: 5,
        name: ''
    };
    eloading = false;
    isVisible = false;
    taskList: VisitTask[] = [];
    allChecked = false;

    constructor(private taskService: VisitTaskServiceProxy) {
    }

    ngOnInit(): void {
    }
    show() {
        this.taskList = new Array<VisitTask>();
        this.isVisible = true;
        this.refreshData();
    }

    /**
     * 获取
     */
    refreshData() {
        this.eloading = true;
        let params: any = {};
        params.SkipCount = this.q.pi;
        params.MaxResultCount = this.q.ps;
        params.Name = this.q.name;
        this.taskService.getVisitTaskList(params).subscribe((result: PagedResultDtoOfVisitTask) => {
            this.eloading = false;
            this.taskList = result.items;
            this.q.total = result.totalCount;
        });
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
     * @param organization 选择事件（对选择的数据进行回传）
     */
    SelectOrganization(visitTask: VisitTask): void {
        this.q.name = '';
        this.modalSelect.emit(visitTask);
        this.isVisible = false;
    }
}
