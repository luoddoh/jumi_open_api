using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 供应商信息服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcSupplierInfoService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcSupplierInfo> _rep;
    public FlcSupplierInfoService(SqlSugarRepository<FlcSupplierInfo> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询供应商信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcSupplierInfoOutput>> Page(FlcSupplierInfoInput input)
    {
        var query = _rep.AsQueryable()
            .Where(u=>u.IsDelete==false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.SupName.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.SupName), u => u.SupName.Contains(input.SupName.Trim()))
            .WhereIF(input.CategoryId>0, u => u.CategoryId == input.CategoryId)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<FlcCategorySupplier>((u, categoryid) => u.CategoryId == categoryid.Id )
            .OrderBy(u => u.CreateTime)
            .Select((u, categoryid) => new FlcSupplierInfoOutput
            {
                Id = u.Id,
                SupName = u.SupName,
                CategoryId = u.CategoryId,  
                CategoryIdCategoryName = categoryid.CategoryName,
            });
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加供应商信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcSupplierInfoInput input)
    {
        var entity = input.Adapt<FlcSupplierInfo>();
        var clist = _rep.AsQueryable().Where(u => u.IsDelete == false).ToList();
        bool ok = true;
        foreach (var c in clist)
        {
            if (c.SupName == entity.SupName)
            {
                ok = false;
                break;
            }
        }
        if (ok)
        {
            await _rep.InsertAsync(entity);
            return entity.Id;
        }
        else
        {
            throw Oops.Oh(ErrorCodeEnum.D1006);
        }
    }

    /// <summary>
    /// 删除供应商信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcSupplierInfoInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新供应商信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcSupplierInfoInput input)
    {
        var entity = input.Adapt<FlcSupplierInfo>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取供应商信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcSupplierInfo> Detail([FromQuery] QueryByIdFlcSupplierInfoInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取供应商信息列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcSupplierInfoOutput>> List([FromQuery] FlcSupplierInfoInput input)
    {
        return await _rep.AsQueryable().Where(u => u.IsDelete == false).Select<FlcSupplierInfoOutput>().ToListAsync();
    }




   

}

