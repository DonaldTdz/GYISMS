import { Component, Injector, AfterViewInit, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { NzMessageService } from '../../../node_modules/ng-zorro-antd';
import { zip } from '../../../node_modules/rxjs';
import { HomeInfoServiceProxy } from '@shared/service-proxies/home/home-service';
import { HomeInfo, SheduleStatis } from '@shared/entity/home';
import { addDays } from 'date-fns';
import { AppSessionService } from '@shared/session/app-session.service';
import { Router } from '@angular/router';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],

  animations: [appModuleAnimation()],
})
export class HomeComponent extends AppComponentBase implements OnInit {

  homeInfo: HomeInfo = new HomeInfo();
  sheduleArea: SheduleStatis[] = [];
  sheduleMoth: SheduleStatis[] = [];
  searchArea = { startTime: null, endtime: null }
  searchAreasDate = [];
  searchMoth = 1;
  sheduAreaData = [];
  sheduMothData = [];
  tags = [
    { value: 1, text: '近半年' },
    { value: 2, text: '近一年' },
  ];
  AreaSum = { Satotal: 0, SaComplete: 0, SaExpired: 0 }
  MothSum = { Mototal: 0, MoComplete: 0, MoExpired: 0 }
  shedateFormat = 'yyyy-MM-dd';
  colors = ['#1890ff', '#52c41a', '#eb2f96'];
  roleName: string = '';
  constructor(
    injector: Injector,
    public msg: NzMessageService,
    private homeService: HomeInfoServiceProxy,
    private _appSessionService: AppSessionService,
    private router: Router
  ) {
    super(injector);
  }
  loading = true;

  ngOnInit(): void {
    let roles = this._appSessionService.roles;
    if (roles.length == 1 && roles[0] == 'EnterpriseAdmin') { //如果用户只是企管人员 将跳转到资料库
      this.router.navigate(['app/doc/document']);
    }
    else {
      this.getNowDate();
      this.getHomeInfo();
      this.getSheduleStatisByArea();
      this.getShduleStatisByMoth();
      if (roles) {
        if (roles.includes('Admin')) {
          this.roleName += '系统管理员 ';
        }

        if (roles.includes('CityAdmin')) {
          this.roleName += '市级管理员 ';
        }

        if (roles.includes('DistrictAdmin')) {
          this.roleName += '区县管理员';
        }

        if (roles.includes('EnterpriseAdmin')) {
          this.roleName += '企业标准管理员';
        }
      }
    }
  }
  getHomeInfo() {
    this.homeService.getHomeInfo().subscribe(data => {
      this.homeInfo = data;
    });
  }

  /**
   * 更具区域分组获取任务统计
   */
  getSheduleStatisByArea() {
    this.homeService.getSheduleStatisByArea(this.searchArea).subscribe(data => {
      this.sheduleArea = data.map(i => {
        i.completed = i.completed == null ? 0 : i.completed;
        i.total = i.total == null ? 0 : i.total;
        i.expired = i.expired == null ? 0 : i.expired;
        return i;
      });
      //计算任务情况总数
      this.AreaSum = { Satotal: 0, SaComplete: 0, SaExpired: 0 }
      data.forEach(item => {
        this.AreaSum.Satotal += item.total;
        this.AreaSum.SaComplete += item.completed;
        this.AreaSum.SaExpired += item.expired;
      });


      const AreaData = [];
      this.sheduleArea.forEach(item => {
        AreaData.push({
          name: "计划",
          x: item.groupName,
          y: item.total
        });

        AreaData.push({
          name: "完成",
          x: item.groupName,
          y: item.completed
        });

        AreaData.push({
          name: "逾期",
          x: item.groupName,
          y: item.expired
        });
      });
      this.sheduAreaData = AreaData;
    });
  }

  /**
   * 更具月份分组获取任务统计的结果
   */
  getShduleStatisByMoth() {
    this.homeService.getSheduleStatisByMoth({ searchMoth: this.searchMoth }).subscribe(data => {
      this.sheduleMoth = data.map(i => {
        i.completed = i.completed == null ? 0 : i.completed;
        i.total = i.total == null ? 0 : i.total;
        i.expired = i.expired == null ? 0 : i.expired;
        return i;
      });
      this.MothSum = { Mototal: 0, MoComplete: 0, MoExpired: 0 }
      data.forEach(item => {
        this.MothSum.Mototal += item.total;
        this.MothSum.MoComplete += item.completed;
        this.MothSum.MoExpired += item.expired;
      });

      let mothData = [];
      this.sheduleMoth.forEach(item => {
        mothData.push({
          name: '计划',
          x: item.groupName,
          y: item.total,
        });

        mothData.push({
          name: '完成',
          x: item.groupName,
          y: item.completed,
        });

        mothData.push({
          name: '逾期',
          x: item.groupName,
          y: item.expired,
        });
      });
      this.sheduMothData = mothData;
    });
  }

  /**
   * 半年、一年
   * @param id 
   */
  changeCategory() {
    this.getShduleStatisByMoth();
  }

  /**
   * 时间段发生改变
   */
  changeTime(times) {
    if (times.length > 0) {
      this.searchArea.startTime = this.dateFormat(times[0]);
      this.searchArea.endtime = this.dateFormat(times[1]);
    }
    this.getSheduleStatisByArea();
  }

  getNowDate() {
    var nowDate = new Date();
    var year = nowDate.getFullYear();
    var moth = nowDate.getMonth();
    var firstDay = new Date(year, moth, 1);
    // this.firstDay = new Date(nowDate.setDate(1));
    var lastDay = addDays(new Date(year, moth + 1, 1), -1);
    this.searchAreasDate = [firstDay, lastDay]
  }
}
