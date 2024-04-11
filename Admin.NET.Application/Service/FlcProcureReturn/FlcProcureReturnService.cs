using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 采购退货列表服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcProcureReturnService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcProcureReturn> _rep;
    public FlcProcureReturnService(SqlSugarRepository<FlcProcureReturn> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcProcureReturnOutput>> Page(FlcProcureReturnInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
            )
                .Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(input.SupplierId>0, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.Returner>0, u => u.Returner == input.Returner)
            .WhereIF((input.uid > 0 && input.uid != 1300000000101 && input.uid != 1300000000111 && (input.Isinventory == null || input.Isinventory == false)), u => u.Returner == input.uid)
            .WhereIF((input.Isinventory == true), u => u.SupplierId == input.userSupplierId)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, returner) => u.Returner == returner.Id )
            .LeftJoin<SysUser>((u, returner, reviewer) => u.Reviewer == reviewer.Id )
            .LeftJoin<FlcSupplierInfo>((u, returner, reviewer, suppl) => u.SupplierId == suppl.Id)
             .LeftJoin<SysOrg>((u, returner, reviewer, suppl,org) => returner.OrgId == org.Id)
            .OrderBy(u => u.CreateTime)
            .Select((u, returner, reviewer, suppl,org) => new FlcProcureReturnOutput()
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                SupplierId = u.SupplierId,
                SupplierIdSuplName=suppl.SupName,
                Returner = u.Returner, 
                ReturnerRealName = returner.RealName,
                department=org.Name,
                ReturnTime = u.ReturnTime,
                Reviewer = u.Reviewer, 
                ReviewerRealName = reviewer.RealName,
                AuditTime = u.AuditTime,
                TotalAmount = u.TotalAmount,
                Remark = u.Remark,
                State=u.State
            });
        if(input.ReturnTimeRange != null && input.ReturnTimeRange.Count >0)
        {
            DateTime? start= input.ReturnTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.ReturnTime > start);
            if (input.ReturnTimeRange.Count >1 && input.ReturnTimeRange[1].HasValue)
            {
                var end = input.ReturnTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReturnTime < end);
            }
        } 
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcProcureReturnInput input)
    {
        var entity = input.Adapt<FlcProcureReturn>();
        entity.DocNumber = "TD" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + new Random().Next(1000, 9999).ToString();
        entity.State = 100;
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 删除采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcProcureReturnInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcProcureReturnInput input)
    {
        var entity = input.Adapt<FlcProcureReturn>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }


    [HttpPost]
    [ApiDescriptionSettings(Name = "Examine")]
    public async Task Examine(ExamineInput input)
    {
        var row = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        row.State = input.state;
        if (input.Reviewer != null)
        {
            row.Reviewer = input.Reviewer;
            row.AuditTime = DateTime.Now;
        }
        if (input.state == 100)
        {
            row.Reviewer = null;
            row.AuditTime = null;
        }
        await _rep.AsUpdateable(row).ExecuteCommandAsync(); ;
    }
    [HttpGet]
    [ApiDescriptionSettings(Name = "Confirm")]
    public async Task Confirm([FromQuery] ConfirmInput input)
    {
        var list = _rep.Context.Queryable<FlcProcureReturnDetail>()
            .Where(x=>x.ReturnId==input.Id&&x.IsDelete==false)
            .ToList();
        foreach (var item in list)
        {
            var row = _rep.Context.Queryable<FlcInventory>()
                .Where(x => x.SkuId == item.SkuId).First();
            row.Number=row.Number-item.ReturnNum;
            await _rep.Context.Updateable(row).ExecuteCommandAsync();
        }
        var detail = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        detail.State = 300;
        await _rep.AsUpdateable(detail).ExecuteCommandAsync(); ;
    }

    /// <summary>
    /// 获取采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcProcureReturn> Detail([FromQuery] QueryByIdFlcProcureReturnInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取采购退货列表列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcProcureReturnOutput>> List([FromQuery] FlcProcureReturnInput input)
    {
        return await _rep.AsQueryable().Select<FlcProcureReturnOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取退货员列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SysUserReturnerDropdown"), HttpGet]
    public async Task<dynamic> SysUserReturnerDropdown()
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

