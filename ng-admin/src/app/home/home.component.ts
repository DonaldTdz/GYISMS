import { Component, Injector, AfterViewInit, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { _HttpClient } from '@delon/theme';
import { NzMessageService } from '../../../node_modules/ng-zorro-antd';
import { zip } from '../../../node_modules/rxjs';
import { HomeInfoServiceProxy } from '@shared/service-proxies/home/home-service';
import { HomeInfo, SheduleStatis } from '@shared/entity/home';

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
  searchAreas = [];
  searchMoth = 1;
  sheduAreaData = [];
  sheduMothData = [{
    name: "计划",
    x: "2018-07",
    y: 85
  },
  {
    name: "完成",
    x: "2018-07",
    y: 80
  },
  {
    name: "逾期",
    x: "2018-07",
    y: 5
  },
  {
    name: "计划",
    x: "2018-08",
    y: 90
  },
  {
    name: "完成",
    x: "2018-08",
    y: 60
  },
  {
    name: "逾期",
    x: "2018-08",
    y: 10
  },
  {
    name: "计划",
    x: "2018-09",
    y: 80
  },
  {
    name: "完成",
    x: "2018-09",
    y: 70
  },
  {
    name: "逾期",
    x: "2018-09",
    y: 5
  }
  ];
  tags = [
    { value: 1, text: '半年' },
    { value: 2, text: '一年' },
  ];
  AreaSum = { Satotal: 0, SaComplete: 0, SaExpired: 0 }
  MothSum = { Mototal: 0, MoComplete: 0, MoExpired: 0 }
  shedateFormat = 'yyyy-MM-dd';
  constructor(
    injector: Injector,
    private http: _HttpClient,
    public msg: NzMessageService,
    private homeService: HomeInfoServiceProxy,
  ) {
    super(injector);
  }

  members = [
    {
      id: 'members-1',
      title: '52ABP社区',
      logo: 'https://avatars2.githubusercontent.com/u/33684174?s=200&v=4',
      link:
        // tslint:disable-next-line:max-line-length
        'https://www.52abp.com',
    },
    {
      id: 'members-2',
      title: '视频课程：网易云云课堂',
      logo: 'http://pp.myapp.com/ma_icon/0/icon_11357537_1529483301/256',
      link:
        'http://study.163.com/provider/400000000309007/course.htm?share=2&shareId=400000000309007',
    },
    {
      id: 'members-1',
      title: '微信公众号-角落的白板报',
      logo:
        'http://wx.qlogo.cn/mmhead/Q3auHgzwzM6PQV7JWIpJ2seavD5UuzCVWPyZs0SVqFkdYRyc3HQUkg/0',
      link:
        // tslint:disable-next-line:max-line-length
        'https://mp.weixin.qq.com/profile?src=3&timestamp=1532171698&ver=1&signature=hRm1TI4zh80GpKxR5LYIc9SyUcyUPiM1EE8qlUdm4dbNzP06DOfA0HKfgajY2Dyj2xku0anPcrOwE8f7mjlwxg==',
    },
    {
      id: 'members-3',
      title: '视频课程：腾讯课堂',
      logo:
        'http://is4.mzstatic.com/image/thumb/Purple111/v4/06/e9/d3/06e9d3e2-4e07-f556-a765-7c8749f09c12/source/1200x630bb.jpg',
      link: 'https://ke.qq.com/course/287301?tuin=2522cdf3',
    },

    {
      id: 'members-5',
      title: 'github源代码',
      logo: 'https://major.io/wp-content/uploads/2014/08/github.png',
      link: 'https://github.com/52ABP/LTMCompanyNameFree.GYISMS',
    },
    {
      id: 'members-5',
      title: '微软MVP',
      logo: 'https://mvp.microsoft.com/Content/Images/mvp-banner.png',
      link: 'https://mvp.microsoft.com/zh-CN/PublicProfile/5002741',
    },
    {
      id: 'members-4',
      title: '博客园-博文地址',
      logo: '',
      link: 'https://www.cnblogs.com/wer-ltm/',
    },

    {
      id: 'members-5',
      title: '博文地址:知乎专栏',
      logo:
        'http://wx.qlogo.cn/mmhead/Q3auHgzwzM6PQV7JWIpJ2seavD5UuzCVWPyZs0SVqFkdYRyc3HQUkg/0',
      link: 'https://zhuanlan.zhihu.com/52abp',
    },
    // 、、https://github.com/52ABP/LTMCompanyNameFree.GYISMS
  ];

  notice: any[];
  loading = true;

  ngOnInit(): void {
    /*zip(this.http.get('/api/notice')).subscribe(([chart]: [any]) => {
      this.notice = chart;

      this.loading = false;
    });*/
    //this.getHomeInfo();
    //this.getSheduleStatisByArea();
    //this.getShduleStatisByMoth();
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
      data.forEach(item => {
        this.AreaSum.Satotal += item.total;
        this.AreaSum.SaComplete += item.completed;
        this.AreaSum.SaExpired += item.expired;
      });
      const totals = [];
      const completes = [];
      const expireds = [];
      this.sheduleArea.forEach(item => {
        totals.push({
          x: item.groupName,
          y: item.total,
          "name": "total"
        });
        completes.push({
          x: item.groupName,
          y: item.completed,
          "name": "completed"
        });
        expireds.push({
          x: item.groupName,
          y: item.expired,
          "name": "expired"
        });
      });
      this.sheduAreaData.push(totals);
      this.sheduAreaData.push(completes);
      this.sheduAreaData.push(expireds);
      console.log("sheduAreaData:");
      console.log(this.sheduAreaData);
      console.log("sheduleArea:");
      console.log(this.sheduleArea);

    });
  }

  /**
   * 更具月份分组获取任务统计的结果
   */
  getShduleStatisByMoth() {
    this.homeService.getSheduleStatisByMoth({ searchMoth: this.searchMoth }).subscribe(data => {
      console.log("data2:")
      console.log(data);
      this.sheduleMoth = data.map(i => {
        i.completed = i.completed == null ? 0 : i.completed;
        i.total = i.total == null ? 0 : i.total;
        i.expired = i.expired == null ? 0 : i.expired;
        return i;
      });
      data.forEach(item => {
        this.MothSum.Mototal += item.total;
        this.MothSum.MoComplete += item.completed;
        this.MothSum.MoExpired += item.expired;
      });
      /*this.sheduleMoth.forEach(item => {
        this.sheduMothData.push({
          x: item.groupName,
          y: item.total,
        })
      });*/
    });
    console.log("sheduleMoth:")
    console.log(this.sheduleMoth);
  }

  /**
   * 半年、一年
   * @param id 
   */
  changeCategory() {
    // this.searchMoth = id;
    console.log("searchMoth:");
    console.log(this.searchMoth);
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
}
