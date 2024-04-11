using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
namespace Admin.NET.Application;
/// <summary>
/// 出库单服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryOutService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventoryOut> _rep;
    public FlcInventoryOutService(SqlSugarRepository<FlcInventoryOut> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcInventoryOutOutput>> Page(FlcInventoryOutInput input)
    {
        var query = _rep.AsQueryable()
            .Where(x=>x.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
                || u.Remark.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OutType), u => u.OutType == input.OutType)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF(input.Reviewer>0, u => u.Reviewer == input.Reviewer)
             .WhereIF((input.Uid > 0 && input.Uid != 1300000000101 && input.Uid != 1300000000111), u => u.Operator == input.Uid)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, reviewer) => u.Reviewer == reviewer.Id )
            .LeftJoin<SysUser>((u, reviewer, Operator) => u.Operator == Operator.Id)
            .OrderBy(u => u.CreateTime)
            .Select((u, reviewer, Operator) => new FlcInventoryOutOutput
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                OutType = u.OutType.ToString(),
                OutTime = u.OutTime,
                Remark = u.Remark,
                Reviewer = u.Reviewer, 
                ReviewerRealName = reviewer.RealName,
                OperatorRealName= Operator.RealName,
                ReviewerTime = u.ReviewerTime,
                State = u.State,
            });
        if(input.OutTimeRange != null && input.OutTimeRange.Count >0)
        {
            DateTime? start= input.OutTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.OutTime > start);
            if (input.OutTimeRange.Count >1 && input.OutTimeRange[1].HasValue)
            {
                var end = input.OutTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.OutTime < end);
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
    /// 小程序分页查询出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "MiniPage")]
    public async Task<SqlSugarPagedList<FlcInventoryOutOutput>> MiniPage(FlcInventoryOutInput input)
    {
        var query = _rep.AsQueryable()
            .Where(x => x.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
            )
            .WhereIF(input.Uid!=null,u=>u.Operator==input.Uid)
            .WhereIF((!string.IsNullOrEmpty(input.State) && input.State!="0" ), u => u.State == input.State)
            .WhereIF(!string.IsNullOrWhiteSpace(input.OutType)&& input.OutType!="0", u => u.OutType == input.OutType)
            .WhereIF(input.Reviewer > 0, u => u.Reviewer == input.Reviewer)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, reviewer) => u.Reviewer == reviewer.Id)
            .LeftJoin<SysUser>((u, reviewer, Operator) => u.Operator == Operator.Id)
            .OrderBy(u => u.CreateTime)
            .Select((u, reviewer, Operator) => new FlcInventoryOutOutput
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                OutType = u.OutType.ToString(),
                OutTime = u.OutTime,
                Remark = u.Remark,
                Reviewer = u.Reviewer,
                ReviewerRealName = reviewer.RealName,
                ReviewerTime = u.ReviewerTime,
                State = u.State,
                OperatorRealName = Operator.RealName,
                CreateTime = u.CreateTime,
                CreateUserName = u.CreateUserName
            });
        if (input.OutTimeRange != null && input.OutTimeRange.Count > 0)
        {
            DateTime? start = input.OutTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.OutTime > start);
            if (input.OutTimeRange.Count > 1 && input.OutTimeRange[1].HasValue)
            {
                var end = input.OutTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.OutTime < end);
            }
        }
        var list = query.ToList();
        _rep.Context.ThenMapper(list, obj =>
        {
            var deta = _rep.Context.Queryable<FlcInventoryOutDetail>().Where(x => x.IsDelete == false)
           .SetContext(x => x.OutId, () => obj.Id, obj)
           .Select(x => new
           {
               number = x.OutNum,
               TotalAmount = x.TotalAmount
           }).ToList();
            int num = 0;
            decimal price = 0;
            foreach (var x in deta)
            {
                num +=(int)x.number;
                price +=(decimal)x.TotalAmount;
            }
            obj.OutNum = num;
            obj.TotalAmount = price;
        });
        return list.ToPagedList(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcInventoryOutInput input)
    {
        var entity = input.Adapt<FlcInventoryOut>();
        entity.DocNumber = "CK" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + new Random().Next(1000, 9999).ToString();
        entity.State = "100";
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 删除出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcInventoryOutInput input)
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
    public async Task Examine(ExamineInvOutInput input)
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
        var list = _rep.Context.Queryable<FlcInventoryOutDetail>()
            .Where(x => x.OutId == input.Id && x.IsDelete == false)
            .ToList();
        foreach (var item in list)
        {
            var row = _rep.Context.Queryable<FlcInventory>()
                .Where(x => x.SkuId == item.SkuId).First();
            row.Number = (int)(row.Number - item.OutNum);
            row.TotalAmount = (int)(row.TotalAmount - item.TotalAmount)<0?0: (int)(row.TotalAmount - item.TotalAmount);
            await _rep.Context.Updateable(row).ExecuteCommandAsync();
        }
        var detail = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        detail.State = "102";
        await _rep.AsUpdateable(detail).ExecuteCommandAsync(); ;
    }
    /// <summary>
    /// 更新出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcInventoryOutInput input)
    {
        var entity = input.Adapt<FlcInventoryOut>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcInventoryOut> Detail([FromQuery] QueryByIdFlcInventoryOutInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }
    /// <summary>
    /// 小程序获取出库单列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "MiniDetail")]
    public async Task<dynamic> MiniDetail([FromQuery] QueryByIdFlcInventoryOutInput input)
    {
        var quer = _rep.AsQueryable()
            .LeftJoin<SysUser>((x, p) => x.Operator == p.Id)
            .LeftJoin<SysUser>((x, p, r) => x.Reviewer == r.Id)
            .Where(x => x.Id == input.Id)
            .Select((x, p, r) => new
            {
                docNumber = x.DocNumber,
                state = x.State,
                createUserName = x.CreateUserName,
                createTime = x.CreateTime,
                Operator = p.RealName,
                OutTime = x.OutTime,
                reviewer = r.RealName,
                ReviewerTime = x.ReviewerTime,
                remark = x.Remark,
            })
            .FirstAsync();
        return await quer;
    }
    /// <summary>
    /// 获取出库单列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcInventoryOutOutput>> List([FromQuery] FlcInventoryOutInput input)
    {
        return await _rep.AsQueryable().Select<FlcInventoryOutOutput>().ToListAsync();
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

