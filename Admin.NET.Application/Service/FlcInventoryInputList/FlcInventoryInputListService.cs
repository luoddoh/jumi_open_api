using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 入库单服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryInputListService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventoryInputList> _rep;
    public FlcInventoryInputListService(SqlSugarRepository<FlcInventoryInputList> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询入库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcInventoryInputListOutput>> Page(FlcInventoryInputListInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
                || u.InputType.Contains(input.SearchKey.Trim())
                || u.Remark.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.InputType), u => u.InputType.Contains(input.InputType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF((input.Uid > 0 && input.Uid != 1300000000101 && input.Uid != 1300000000111), u => (u.Operator == input.Uid || u.CreateUserId == input.Uid))
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, operato) => u.Operator == operato.Id )
            .LeftJoin<SysUser>((u, operato, reviewer) => u.Reviewer == reviewer.Id )
            .OrderBy(u => u.CreateTime)
            .Select((u, operato, reviewer) => new FlcInventoryInputListOutput
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                InputTime = u.InputTime,
                State = u.State,
                Operator = u.Operator, 
                OperatorRealName = operato.RealName,
                InputType = u.InputType,
                Reviewer = u.Reviewer, 
                ReviewerRealName = reviewer.RealName,
                ReviewerTime = u.ReviewerTime,
                Remark = u.Remark,
            });
        if(input.InputTimeRange != null && input.InputTimeRange.Count >0)
        {
            DateTime? start= input.InputTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.InputTime > start);
            if (input.InputTimeRange.Count >1 && input.InputTimeRange[1].HasValue)
            {
                var end = input.InputTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.InputTime < end);
            }
        } 
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 小程序分页查询入库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "MiniPage")]
    public async Task<SqlSugarPagedList<FlcInventoryInputListOutput>> MiniPage(FlcInventoryInputListInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
                || u.InputType.Contains(input.SearchKey.Trim())
                || u.Remark.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.InputType), u => u.InputType.Contains(input.InputType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF((input.Uid > 0 && input.Uid != 1300000000101 && input.Uid != 1300000000111), u => (u.Operator == input.Uid || u.CreateUserId == input.Uid))
            .WhereIF(!string.IsNullOrWhiteSpace(input.State)&& input.State!="0", u=>u.State==input.State)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, operato) => u.Operator == operato.Id)
            .LeftJoin<SysUser>((u, operato, reviewer) => u.Reviewer == reviewer.Id)
            .OrderBy(u => u.CreateTime)
            .Select((u, operato, reviewer) => new FlcInventoryInputListOutput
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                InputTime = u.InputTime,
                State = u.State,
                Operator = u.Operator,
                OperatorRealName = operato.RealName,
                InputType = u.InputType,
                Reviewer = u.Reviewer,
                ReviewerRealName = reviewer.RealName,
                ReviewerTime = u.ReviewerTime,
                Remark = u.Remark,
                CreateTime = u.CreateTime,
                CreateUserName = u.CreateUserName,
            });
        if (input.InputTimeRange != null && input.InputTimeRange.Count > 0)
        {
            DateTime? start = input.InputTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.InputTime > start);
            if (input.InputTimeRange.Count > 1 && input.InputTimeRange[1].HasValue)
            {
                var end = input.InputTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.InputTime < end);
            }
        }
        var list = query.ToList();
        _rep.Context.ThenMapper(list, obj =>
        {
            var deta = _rep.Context.Queryable<FlcInventoryInputDetail>().Where(x => x.IsDelete == false)
           .SetContext(x => x.InputId, () => obj.Id, obj)
           .Select(x => new
           {
               number = x.InputNum,
               TotalAmount = x.TotalAmount
           }).ToList();
            int num = 0;
            decimal price = 0;
            foreach (var x in deta)
            {
                num += (int)x.number;
                price += (decimal)x.TotalAmount;
            }
            obj.InputNum = num;
            obj.TotalAmount = price;
        });
        return list.ToPagedList(input.Page, input.PageSize);
    }
    /// <summary>
    /// 小程序获取出库单列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "MiniDetail")]
    public async Task<dynamic> MiniDetail([FromQuery] QueryByIdFlcInventoryInputListInput input)
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
                InputTime = x.InputTime,
                reviewer = r.RealName,
                ReviewerTime = x.ReviewerTime,
                remark = x.Remark,
            })
            .FirstAsync();
        return await quer;
    }
    /// <summary>
    /// 导出详情查询数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "DetailList")]
    public async Task<SqlSugarPagedList<InventoryInputDetail>> DetailList(FlcInventoryInputListInput input)
    {
        var query = _rep.AsQueryable()
            .Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
                || u.Remark.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.InputType), u => u.InputType == input.InputType)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .WhereIF((input.Uid > 0 && input.Uid != 1300000000101 && input.Uid != 1300000000111), u => (u.Operator == input.Uid || u.CreateUserId == input.Uid))
            .LeftJoin<FlcInventoryInputDetail>((u, d) => u.Id == d.InputId && d.IsDelete == false)
            .LeftJoin<FlcGoodsSku>((u, d, s) => d.SkuId == s.Id && s.IsDelete == false)
            .LeftJoin<FlcGoodsUnit>((u, d, s, t) => s.UnitId == t.Id && t.IsDelete == false)
            .LeftJoin<FlcGoods>((u, d, s, t, g) => s.GoodsId == g.Id && g.IsDelete == false)
            .LeftJoin<SysUser>((u, d, s, t, g, purchaser) => u.Operator == purchaser.Id)
            .LeftJoin<SysUser>((u, d, s, t, g, purchaser, reviewer) => u.Reviewer == reviewer.Id)
            .Select((u, d, s, t, g, purchaser, reviewer) => new InventoryInputDetail
            {
                DocNumber = u.DocNumber,
                State = u.State,
                ReviewerTime = u.ReviewerTime,
                InputTime = u.InputTime,
                Remark = u.Remark,
                OperatorRealName = purchaser.RealName,
                ReviewerRealName = reviewer.RealName,
                SkuId = s.Id,
                GoodsName = g.GoodsName,
                UnitName = t.UnitName,
                Price = d.Price,
                InputNum = d.InputNum,
                totalAmount = (decimal)d.TotalAmount,
                DetailRemark = d.Remark
            });
        if (input.InputTimeRange != null && input.InputTimeRange.Count > 0)
        {
            DateTime? start = input.InputTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.InputTime > start);
            if (input.InputTimeRange.Count > 1 && input.InputTimeRange[1].HasValue)
            {
                var end = input.InputTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.InputTime < end);
            }
        }
        var list = query.ToList();
        _rep.Context.ThenMapper(list, item =>
        {
            var info = _rep.Context.Queryable<FlcSkuSpeValue>().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => item.SkuId, item)
            .Select(x => new labval
            {
                Id = x.SpeValueId,
                SpecificationId = x.FlcSpecificationValue.SpecificationId,
                SpeValue = x.FlcSpecificationValue.SpeValue
            }).ToList();
            string value = "";
            foreach (var ele in info)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = ele.SpeValue;
                }
                else
                {
                    value += "/" + ele.SpeValue;
                }
            }
            item.speValueList = value;
        });
        return list.ToPagedList(input.Page, input.PageSize);
    }
    /// <summary>
    /// 增加入库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcInventoryInputListInput input)
    {
        input.DocNumber = "RK" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + new Random().Next(1000, 9999).ToString();
        input.State = "100";
        var entity = input.Adapt<FlcInventoryInputList>();
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Examine")]
    public async Task Examine(ExamineInvInput input)
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
        await _rep.AsUpdateable(row).ExecuteCommandAsync();
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
        var list = _rep.Context.Queryable<FlcInventoryInputDetail>()
            .Where(x => x.InputId == input.Id && x.IsDelete == false)
            .ToList();
        foreach (var item in list)
        {
            var row = _rep.Context.Queryable<FlcInventory>()
                .Where(x => x.SkuId == item.SkuId).First();
            row.Number = (int)(row.Number + item.InputNum);
            row.TotalAmount = (int)(row.TotalAmount + item.TotalAmount) ;
            await _rep.Context.Updateable(row).ExecuteCommandAsync();
        }
        var detail = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        detail.State = "102";
        await _rep.AsUpdateable(detail).ExecuteCommandAsync(); ;
    }
    /// <summary>
    /// 删除入库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcInventoryInputListInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新入库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcInventoryInputListInput input)
    {
        var entity = input.Adapt<FlcInventoryInputList>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取入库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcInventoryInputList> Detail([FromQuery] QueryByIdFlcInventoryInputListInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取入库单列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcInventoryInputListOutput>> List([FromQuery] FlcInventoryInputListInput input)
    {
        return await _rep.AsQueryable().Select<FlcInventoryInputListOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取操作员列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SysUserOperatorDropdown"), HttpGet]
    public async Task<dynamic> SysUserOperatorDropdown()
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

