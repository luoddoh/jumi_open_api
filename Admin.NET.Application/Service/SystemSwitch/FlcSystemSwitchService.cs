using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;
using Nest;
using COSXML.Model.Tag;
namespace Admin.NET.Application;
/// <summary>
/// 业务系统开关服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcSystemSwitchService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysDictData> _rep;
    private readonly SqlSugarRepository<FlcSwitchLinkUser> _repLink;
    public FlcSystemSwitchService(SqlSugarRepository<SysDictData> rep, SqlSugarRepository<FlcSwitchLinkUser> repLink)
    {
        _rep = rep;
        _repLink = repLink;

    }

    /// <summary>
    /// 业务系统开关列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<List<SystemSwitchOut>>> List()
    {
        var quer= await _rep.Context.Queryable<SysDictType>()
            .LeftJoin<SysDictData>((type,data)=>type.Id==data.DictTypeId&&data.IsDelete==false)
            .Where((type, data)=>type.IsDelete==false&&type.Code=="system_switch")
            .OrderBy((type,data)=>data.OrderNo)
            .Select((type,data)=>new SystemSwitchOut
            {
                switchId=data.Id,
                switchName=data.Value,
                switchValue=data.Code,
            })
            .ToListAsync();
        _rep.Context.ThenMapper(quer, item =>
        {
            var switchId=item.switchId;
            var list = _rep.Context.Queryable<SysUser>()
            .InnerJoin<FlcSwitchLinkUser>((user, link) => user.Id == link.UserId && link.IsDelete == false)
            .InnerJoin<SysDictData>((user, link, data) => link.SwitchId == data.Id)
            .Where((user, link, data) => data.Id==switchId)
            .Select((user, link, data) => new User
            {
                UserId = user.Id,
                UserName = user.RealName,
            })
            .ToList();

            item.children = list;
        });
        List<List<SystemSwitchOut>> result = new List<List<SystemSwitchOut>>();
        for(int i=0;i< quer.Count/4+(quer.Count%4>0?1:0); i++)
        {
            List < SystemSwitchOut > obj= new List<SystemSwitchOut >();
            for (int j = 0; j <= 3&&((i*4+j)<quer.Count); j++)
            {
                obj.Add(quer[(i * 4 + j)]);
            }
            result.Add(obj);
        }
        return result;
    }
    /// <summary>
    /// 获取所有用户（除管理员）
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Users")]
    public async Task<dynamic> Users()
    {

        return await _rep.Context.Queryable<SysUser>()
            .Where(user => user.IsDelete==false&&user.Id!= 1300000000111&& user.Id != 1300000000101)
            .Select(user => new
            {
                UserId = user.Id,
                UserName = user.RealName,
            })
            .ToListAsync();
    }

    /// <summary>
    /// 保存权限设置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Save")]
    public async Task Save(SaveSetUpInput input)
    {
        var quer= _rep.Context.Queryable<FlcSwitchLinkUser>()
            .Where(link=> link.SwitchId==input.switchId)
            .ToList();

        foreach(var item in quer)
        {
            bool isDelete = true;
            foreach(var ele in input.children)
            {
                if (ele == item.UserId)
                {
                    isDelete = false; break;
                }
            }
            if (isDelete)
            {
                await _repLink.DeleteAsync(item);   //真删除
            }
        }
        foreach (var ele in input.children)
        {
            bool isAdd = true;
            foreach (var item in quer)
            {
                if (ele == item.UserId)
                {
                    isAdd = false; break;
                }
            }
            if (isAdd)
            {
                FlcSwitchLinkUser ent = new FlcSwitchLinkUser();
                ent.SwitchId = input.switchId;
                ent.UserId=ele;
                await _repLink.InsertAsync(ent);
            }
        }
    }

    [HttpGet]
    [ApiDescriptionSettings(Name = "HavePower")]
    public async Task<dynamic> HavePower([FromQuery]long userId)
    {
        return await _rep.Context.Queryable<SysUser>()
            .InnerJoin<FlcSwitchLinkUser>((user, link) => user.Id == link.UserId && link.IsDelete == false)
            .InnerJoin<SysDictData>((user, link, data) => link.SwitchId == data.Id)
            .Where(user => user.Id == userId)
            .Select((user, link, data) => data.Code)
            .ToListAsync();
    }
    [HttpGet]
    [ApiDescriptionSettings(Name = "SystemPower")]
    public async Task<dynamic> SystemPower()
    {
        return await _rep.Context.Queryable<SysConfig>()
            .Where(u=>u.IsDelete==false&&u.GroupCode=="jumi")
            .Select(u => new
            {
                u.Code,
                u.Value,
                u.Name,
                u.Id,
            })
            .ToListAsync();
    }
    [HttpPost]
    [ApiDescriptionSettings(Name = "SaveSystemPower")]
    public async Task SaveSystemPower(SaveSystemInput input)
    {
        var entity = _rep.Context.Queryable<SysConfig>().First(u=>u.Id== input.Id);
        entity.Value = input.Value;
        await _rep.Context.Updateable<SysConfig>(entity).ExecuteCommandAsync();
    }
    [HttpGet]
    [ApiDescriptionSettings(Name = "AutoExmine")]
    public async Task AutoExmine()
    {
        string auto = _rep.Context.Queryable<SysConfig>().Where(u => u.IsDelete == false && u.Code == "sys_auto_exmine").Select(u => u.Value).First();
        if (!string.IsNullOrWhiteSpace(auto) && auto == "True")
        {
            int day = _rep.Context.Queryable<SysConfig>().Where(u => u.IsDelete == false && u.Code == "sys_exmine_day").Select(u => u.Value).First().ToInt();
            var list = _rep.Context.Queryable<FlcProcure>().Where(u => u.IsDelete == false).ToList();
            var list_return= _rep.Context.Queryable<FlcProcureReturn>().Where(u => u.IsDelete == false).ToList();
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
                    var item= list_return[i];
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

