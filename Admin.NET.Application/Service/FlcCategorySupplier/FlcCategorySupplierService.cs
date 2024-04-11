using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 供应商分类服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcCategorySupplierService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcCategorySupplier> _rep;
    public FlcCategorySupplierService(SqlSugarRepository<FlcCategorySupplier> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询供应商分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcCategorySupplierOutput>> Page(FlcCategorySupplierInput input)
    {
        var query = _rep.AsQueryable().Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.CategoryName.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryName), u => u.CategoryName.Contains(input.CategoryName.Trim()))
            .WhereIF(input.SuperiorId>0, u => u.SuperiorId == input.SuperiorId)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<FlcCategorySupplier>((u, superiorid) => u.SuperiorId == superiorid.Id )
            .OrderBy(u => u.CreateTime)
            .Select((u, superiorid) => new FlcCategorySupplierOutput
            {
                Id = u.Id,
                CategoryName = u.CategoryName,
                SuperiorId = u.SuperiorId,  
                SuperiorIdCategoryName = superiorid.CategoryName,
            });
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加供应商分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcCategorySupplierInput input)
    {
        var entity = input.Adapt<FlcCategorySupplier>();
        if (entity.SuperiorId == null)
        {
            entity.SuperiorId = 0;
        }
        var clist = _rep.AsQueryable().Where(u => u.IsDelete == false).ToList();
        bool ok = true;
        foreach (var c in clist)
        {
            if (c.CategoryName == entity.CategoryName)
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
    /// 删除供应商分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcCategorySupplierInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        var zlist = _rep.AsQueryable().Where(u => u.SuperiorId == entity.Id && u.IsDelete == false).First();
        var Supp = _rep.Context.Queryable<FlcSupplierInfo>().Where(u => u.IsDelete == false && u.CategoryId == entity.Id).First();
        if (zlist == null && Supp ==null)
        {
            //await _rep.FakeDeleteAsync(entity);   //假删除
            await _rep.DeleteAsync(entity);   //真删除
        }
        else
        {
            throw Oops.Oh(ErrorCodeEnum.D1007);
        }
    }

    /// <summary>
    /// 更新供应商分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcCategorySupplierInput input)
    {
        var entity = input.Adapt<FlcCategorySupplier>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取供应商分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcCategorySupplier> Detail([FromQuery] QueryByIdFlcCategorySupplierInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取供应商分类列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcCategorySupplierOutput>> List([FromQuery] FlcCategorySupplierInput input)
    {
        return await _rep.AsQueryable().Where(u => u.IsDelete == false).Select<FlcCategorySupplierOutput>().ToListAsync();
    }




    [HttpGet]
    [ApiDescriptionSettings(Name = "FlcCategorySupplierTree")]
    public async Task<dynamic> FlcCategorySupplierTree()
    {
        return await _rep.Context.Queryable<FlcCategorySupplier>().Where(u => u.IsDelete == false).ToTreeAsync(u => u.Children, u => u.SuperiorId, 0);
    }

}

