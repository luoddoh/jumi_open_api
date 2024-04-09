using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 盘点单服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryCheckService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventoryCheck> _rep;
    public FlcInventoryCheckService(SqlSugarRepository<FlcInventoryCheck> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询盘点单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcInventoryCheckOutput>> Page(FlcInventoryCheckInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
                || u.Remark.Contains(input.SearchKey.Trim())
            )
                .Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.State), u => u.State == input.State)
            .WhereIF(input.CheckPeople>0, u => u.CheckPeople == input.CheckPeople)
            .WhereIF(input.Reviewer>0, u => u.Reviewer == input.Reviewer)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
             .WhereIF((input.uid > 0 && input.uid != 1300000000101 && input.uid != 1300000000111), u => u.CheckPeople == input.uid)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, checkpeople) => u.CheckPeople == checkpeople.Id )
            .LeftJoin<SysUser>((u, checkpeople, reviewer) => u.Reviewer == reviewer.Id )
            .OrderBy(u => u.CreateTime)
            .Select((u, checkpeople, reviewer) => new FlcInventoryCheckOutput
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                State = u.State,
                CheckPeople = u.CheckPeople, 
                CheckPeopleRealName = checkpeople.RealName,
                CheckTime = u.CheckTime,
                Reviewer = u.Reviewer, 
                ReviewerRealName = reviewer.RealName,
                ReviewerTime = u.ReviewerTime,
                Remark = u.Remark,
            });
        if(input.CheckTimeRange != null && input.CheckTimeRange.Count >0)
        {
            DateTime? start= input.CheckTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.CheckTime > start);
            if (input.CheckTimeRange.Count >1 && input.CheckTimeRange[1].HasValue)
            {
                var end = input.CheckTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CheckTime < end);
            }
        } 
        if(input.ReviewerTimeRange != null && input.ReviewerTimeRange.Count >0)
        {
            DateTime? start= input.ReviewerTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.ReviewerTime > start);
            if (input.ReviewerTimeRange.Count >1 && input.ReviewerTimeRange[1].HasValue)
            {
                var end = input.ReviewerTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReviewerTime < end);
            }
        } 
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 分页查询盘点单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "MiniPage")]
    public async Task<SqlSugarPagedList<FlcInventoryCheckOutput>> MiniPage(FlcInventoryCheckInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
                || u.Remark.Contains(input.SearchKey.Trim())
            )
            .Where(u => u.IsDelete == false)
            .WhereIF((!string.IsNullOrEmpty(input.State) && input.State != "0"), u => u.State == input.State)
            .WhereIF(input.uid > 0, u => u.CheckPeople == input.uid)
            .WhereIF(input.CheckPeople > 0, u => u.CheckPeople == input.CheckPeople)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, checkpeople) => u.CheckPeople == checkpeople.Id)
            .LeftJoin<SysUser>((u, checkpeople, reviewer) => u.Reviewer == reviewer.Id)
            .OrderBy(u => u.CreateTime)
            .Select((u, checkpeople, reviewer) => new FlcInventoryCheckOutput
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                State = u.State,
                CheckPeople = u.CheckPeople,
                CheckPeopleRealName = checkpeople.RealName,
                CheckTime = u.CheckTime,
                Reviewer = u.Reviewer,
                ReviewerRealName = reviewer.RealName,
                ReviewerTime = u.ReviewerTime,
                Remark = u.Remark,
                CreateTime = u.CreateTime,
                CreateUserName=u.CreateUserName
            });
        if (input.CheckTimeRange != null && input.CheckTimeRange.Count > 0)
        {
            DateTime? start = input.CheckTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CheckTime > start);
            if (input.CheckTimeRange.Count > 1 && input.CheckTimeRange[1].HasValue)
            {
                var end = input.CheckTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CheckTime < end);
            }
        }
        var list = query.ToList();
        _rep.Context.ThenMapper(list, obj =>
        {
            var deta = _rep.Context.Queryable<FlcInventoryCheckDetail>().Where(x => x.IsDelete == false)
           .SetContext(x => x.CheckId, () => obj.Id, obj)
           .Select(x => new
           {
               number = x.DifferenceNum,
               DifferencePrice=x.DifferencePrice
           }).ToList();
            int num = 0;
            decimal price = 0;
            foreach (var x in deta)
            {
                num += x.number;
                price += x.DifferencePrice;
            }
            obj.DifferenceNum = num;
            obj.DifferencePrice = price;
        });
        return list.ToPagedList(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加盘点单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcInventoryCheckInput input)
    {
        var entity = input.Adapt<FlcInventoryCheck>();
        entity.DocNumber = "PD" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + new Random().Next(1000, 9999).ToString();
        entity.State = "100";
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 删除盘点单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcInventoryCheckInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }


    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Examine")]
    public async Task Examine(ExamineInvChecInput input)
    {
        var row = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        row.State = input.state;
        if (input.Reviewer != null)
        {
            row.Reviewer = input.Reviewer;
            row.ReviewerTime = DateTime.Now;
        }
        if (input.state == "100")
        {
            row.Reviewer = null;
            row.ReviewerTime = null;
        }
        await _rep.AsUpdateable(row).ExecuteCommandAsync(); ;
    }
    /// <summary>
    /// 确认出库
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Confirm")]
    public async Task Confirm(ConfirmInput input)
    {
        var list = _rep.Context.Queryable<FlcInventoryCheckDetail>()
            .Where(x => x.CheckId == input.Id && x.IsDelete == false)
            .ToList();
        foreach (var item in list)
        {
            var row = _rep.Context.Queryable<FlcInventory>()
                .Where(x => x.SkuId == item.SkuId).First();
            row.Number = item.CheckNum;
            row.TotalAmount = item.CheckNum*item.Price;
            await _rep.Context.Updateable(row).ExecuteCommandAsync();
        }
        var detail = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        detail.State = "102";
        await _rep.AsUpdateable(detail).ExecuteCommandAsync(); ;
    }




    /// <summary>
    /// 更新盘点单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcInventoryCheckInput input)
    {
        var entity = input.Adapt<FlcInventoryCheck>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取盘点单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcInventoryCheck> Detail([FromQuery] QueryByIdFlcInventoryCheckInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取盘点单列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcInventoryCheckOutput>> List([FromQuery] FlcInventoryCheckInput input)
    {
        return await _rep.AsQueryable().Select<FlcInventoryCheckOutput>().ToListAsync();
    }
    /// <summary>
    /// 小程序获取盘点单列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "MiniDetail")]
    public async Task<dynamic> MiniDetail([FromQuery] QueryByIdFlcInventoryCheckInput input)
    {
        var quer = _rep.AsQueryable()
            .LeftJoin<SysUser>((x, p) => x.CheckPeople == p.Id)
            .LeftJoin<SysUser>((x, p, r) => x.Reviewer == r.Id)
            .Where(x => x.Id == input.Id)
            .Select((x, p, r) => new
            {
                docNumber = x.DocNumber,
                state = x.State,
                createUserName = x.CreateUserName,
                createTime = x.CreateTime,
                CheckPeople = p.RealName,
                CheckTime = x.CheckTime,
                reviewer = r.RealName,
                ReviewerTime = x.ReviewerTime,
                remark = x.Remark,
            })
            .FirstAsync();
        return await quer;
    }
    /// <summary>
    /// 获取盘点人列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SysUserCheckPeopleDropdown"), HttpGet]
    public async Task<dynamic> SysUserCheckPeopleDropdown()
    {
        return await _rep.Context.Queryable<SysUser>()
                .Select(u => new
                {
                    Label = u.RealName,
                    Value = u.Id
                }
                ).ToListAsync();
    }
    /// <summary>
    /// 获取审核人列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SysUserReviewerDropdown"), HttpGet]
    public async Task<dynamic> SysUserReviewerDropdown()
    {
        return await _rep.Context.Queryable<SysUser>()
                .Select(u => new
                {
                    Label = u.RealName,
                    Value = u.Id
                }
                ).ToListAsync();
    }




}

