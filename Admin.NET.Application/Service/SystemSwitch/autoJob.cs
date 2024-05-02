// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Admin.NET.Application.Entity;
using Furion.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.SystemSwitch;
/// <summary>
/// 自动审核
/// </summary>
[JobDetail("job_auto", Description = "自动审核", GroupName = "default", Concurrent = false)]
[Daily(TriggerId = "trigger_auto", Description = "自动审核")]
public class AutoJob : IJob
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AutoJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        using var serviceScope = _scopeFactory.CreateScope();

        var _rep = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysConfig>>();
        string auto = _rep.Context.Queryable<SysConfig>().Where(u => u.IsDelete == false && u.Code == "sys_auto_exmine").Select(u => u.Value).First();
        if (!string.IsNullOrWhiteSpace(auto) && auto == "True")
        {
            int day = _rep.Context.Queryable<SysConfig>().Where(u => u.IsDelete == false && u.Code == "sys_exmine_day").Select(u => u.Value).First().ToInt();
            var list = _rep.Context.Queryable<FlcProcure>().Where(u => u.IsDelete == false).ToList();
            var list_return = _rep.Context.Queryable<FlcProcureReturn>().Where(u => u.IsDelete == false).ToList();
            var list_out = _rep.Context.Queryable<FlcInventoryOut>().Where(u => u.IsDelete == false).ToList();
            var list_check = _rep.Context.Queryable<FlcInventoryCheck>().Where(u => u.IsDelete == false).ToList();
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    DateTime time = (DateTime)list[i].CreateTime;
                    if (DateTime.Compare(DateTime.Now, time.AddDays(day)) >= 0)
                    {
                        if (list[i].State == 100)
                        {
                            list[i].State = 200;
                            list[i].AuditTime = DateTime.Now;
                            await _rep.Context.Updateable(list[i]).ExecuteCommandAsync();
                        }
                    }
                }
            }
            if (list_return != null)
            {
                for (int i = 0; i < list_return.Count; i++)
                {
                    var item = list_return[i];
                    DateTime time = (DateTime)item.CreateTime;
                    if (DateTime.Compare(DateTime.Now, time.AddDays(day)) >= 0)
                    {
                        if (item.State == 100)
                        {
                            item.State = 200;
                            item.AuditTime = DateTime.Now;
                            await _rep.Context.Updateable(item).ExecuteCommandAsync();
                        }
                    }
                }
            }
            if (list_out != null)
            {
                for (int i = 0; i < list_out.Count; i++)
                {
                    var item = list_out[i];
                    DateTime time = (DateTime)item.CreateTime;
                    if (DateTime.Compare(DateTime.Now, time.AddDays(day)) >= 0)
                    {
                        if (item.State == "100")
                        {
                            item.State = "101";
                            item.ReviewerTime = DateTime.Now;
                            await _rep.Context.Updateable(item).ExecuteCommandAsync();
                        }
                    }
                }
            }
            if (list_check != null)
            {
                for (int i = 0; i < list_check.Count; i++)
                {
                    var item = list_check[i];
                    DateTime time = (DateTime)item.CreateTime;
                    if (DateTime.Compare(DateTime.Now, time.AddDays(day)) >= 0)
                    {
                        if (item.State == "100")
                        {
                            item.State = "101";
                            item.ReviewerTime = DateTime.Now;
                            await _rep.Context.Updateable(item).ExecuteCommandAsync();
                        }
                    }
                }
            }
        }
    }
}
