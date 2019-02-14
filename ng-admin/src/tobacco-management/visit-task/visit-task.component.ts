import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { VisitTask } from '@shared/entity/tobacco-management';
import { Router } from '@angular/router';
import { VisitTaskServiceProxy, PagedResultDtoOfVisitTask } from '@shared/service-proxies/tobacco-management';

@Component({
    moduleId: module.id,
    selector: 'visit-task',
    templateUrl: 'visit-task.component.html',
    styleUrls: ['visit-task.component.scss']
})
export class VisitTaskComponent extends AppComponentBase implements OnInit {
    search: any = {};
    loading = false;
    taskList: VisitTask[] = [];
    taskTypes = [
        { text: '技术服务', value: 1 },
        { text: '生产管理', value: 2 },
        { text: '政策宣传', value: 3 },
        { text: '面积落实', value: 5 },
        { text: '临时任务', value: 4 }];
    constructor(injector: Injector, private taskService: VisitTaskServiceProxy,
        private router: Router) {
        super(injector);
    }

    ngOnInit(): void {
        this.refreshData();
    }
    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = {};
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.Name = this.search.name;
        params.TaskType = this.search.taskType;
        this.taskService.getVisitTaskList(params).subscribe((result: PagedResultDtoOfVisitTask) => {
            this.loading = false;
            this.taskList = result.items;
            this.query.total = result.totalCount;
        });
    }

    createTask() {
        this.router.navigate(['app/task/task-detail'])
    }

    goDetail(id: number) {
        this.router.navigate(['app/task/task-detail', id])
    }
}
