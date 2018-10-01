using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Abp.Data;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using GYISMS.GYEnums;
using GYISMS.ScheduleDetails;
using Microsoft.EntityFrameworkCore;

namespace GYISMS.EntityFrameworkCore.Repositories
{
    public class SheduleDetailRepository : GYISMSRepositoryBase<ScheduleDetail, Guid>, ISheduleDetailRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        public SheduleDetailRepository(IDbContextProvider<GYISMSDbContext> dbContextProvider
            , IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }

        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = Context.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }
        private void EnsureConnectionOpen()
        {
            var connection = Context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
                 {
                    {"ContextType", typeof(GYISMSDbContext) },
                    {"MultiTenancySide", MultiTenancySide }
                 });
        }

        /// <summary>
        /// 获取任务完成情况的统计（按月份统计）
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<List<SheduleStatisticalDto>> GetSheduleStatisticalDtosByMothAsync(DateTime startTime,DateTime endTime)
        {
            EnsureConnectionOpen();
            SqlParameter[] sql = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime)
            };
            var command = CreateCommand(@"select cast(t1.[Year] as varchar(4)) + '-' + (case when t1.[Month] > 9 then cast(t1.[Month] as varchar(2)) else '0' + cast(t1.[Month] as varchar(2)) end ) Months,
                     t1.VisitNum, t1.CompleteNum, isnull(t2.ExpireNum, 0) as ExpireNum
                     from(
                     select[Year],[Month], sum(CompleteNum) as CompleteNum, sum(VisitNum) as VisitNum
                     from(
                     select year(s.BeginTime) as [Year], month(s.BeginTime) as [Month], sd.CompleteNum, sd.VisitNum from[dbo].[ScheduleDetails] sd
                     inner join[dbo].[Schedules] s on sd.ScheduleId = s.Id
                    where s.BeginTime >=@startTime and  s.BeginTime < @endTime
                    ) temp group by[Year], [Month]
                    ) t1 left join(
                    select[Year],[Month], sum(ExpireNum) as ExpireNum
                    from(
                    select year(s.BeginTime) as [Year], month(s.BeginTime) as [Month], sd.VisitNum - sd.CompleteNum as ExpireNum from[dbo].[ScheduleDetails] sd
                    inner join[dbo].[Schedules] s on sd.ScheduleId = s.Id
                    where s.BeginTime >= @startTime  and  s.BeginTime < @endTime
                    and sd.[Status] = 0
                    ) temp group by[Year], [Month]) t2 on t1.[Year] = t2.[Year] and t1.[Month] = t2.[Month]
                    ", CommandType.Text, sql);
            using (command)
            {
                using (var dataReader=await command.ExecuteReaderAsync())
                {
                    var result = new List<SheduleStatisticalDto>();
                    while (dataReader.Read())
                    {
                        var sheduleStatis = new SheduleStatisticalDto();
                        sheduleStatis.GroupName = dataReader["Months"].ToString();
                        sheduleStatis.Total =(int)dataReader["VisitNum"];
                        sheduleStatis.Completed = (int)dataReader["CompleteNum"];
                        sheduleStatis.Expired = (int)dataReader["ExpireNum"];
                        result.Add(sheduleStatis);
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// 获取任务汇总数据
        /// </summary>
        /// <param name="Area">区域</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="TaskName">任务名</param>
        /// <returns></returns>
        public async  Task<List<SheduleSumDto>> GetSheduleSum(AreaTypeEnum? Area, DateTime? StartTime, DateTime? EndTime, string TaskName)
        {
            EnsureConnectionOpen();
            SqlParameter[] sql = new SqlParameter[]
            {
                new SqlParameter("@startTime",StartTime),
                new SqlParameter("@endTime",EndTime),
                new SqlParameter("@area",Area),
                new SqlParameter("@taskName",TaskName)
            };
            var codeStr = "";
            if (Area.HasValue)
            {
                codeStr = " and g.CountyCode=@area ";
            }
            var command = CreateCommand(@"select * from
                            (select sd.Id,s.BeginTime, g.CountyCode,t.Name,t.Type,sd.VisitNum,sd.CompleteNum from [dbo].[ScheduleDetails] sd
                            inner join [dbo].[Schedules] s on sd.ScheduleId=s.Id
                            inner join [dbo].[VisitTasks] t on sd.TaskId=t.Id
                            left join [dbo].[Growers] g on sd.GrowerId=g.Id
                            where s.BeginTime>'2018-09-01' and s.BeginTime<='2018-09-30' and t.Name like '%@taskName%'" + codeStr + @" ) t1
                            left join
                            (select sd.Id,(sd.VisitNum-sd.CompleteNum) as ExpiredNum from [dbo].[ScheduleDetails] sd
                            inner join [dbo].[Schedules] s on sd.ScheduleId=s.Id
                            inner join [dbo].[VisitTasks] t on sd.TaskId=t.Id
                            left join [dbo].[Growers] g on sd.GrowerId=g.Id
                            where s.BeginTime>'2018-09-01' and s.BeginTime<='2018-09-30' and t.Name like '%@taskName%'" + codeStr + @" and sd.[Status] = 0) t2 on t1.Id=t2.Id", CommandType.Text, sql);
            using (command)
            {
                using (var dataReader=await command.ExecuteReaderAsync())
                {
                    var result = new List<SheduleSumDto>();
                    while (dataReader.Read())
                    {
                        var sheduleSum = new SheduleSumDto();
                        sheduleSum.AreaCode = (AreaTypeEnum)dataReader["CountyCode"];
                        sheduleSum.TaskName = dataReader["Name"].ToString();
                        sheduleSum.TaskType = (TaskTypeEnum)dataReader["Type"];
                        sheduleSum.Complete = (int)dataReader["CompleteNum"];
                        sheduleSum.Total = (int)dataReader["VisitNum"];
                        sheduleSum.Expired = (int)dataReader["ExpiredNum"];
                        sheduleSum.BeginTime = (DateTime)dataReader["BeginTime"];
                        result.Add(sheduleSum);
                    }
                    return result;
                }
            }
        }
    }
}
