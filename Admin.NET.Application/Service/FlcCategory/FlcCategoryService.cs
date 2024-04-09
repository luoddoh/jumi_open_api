using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 商品分类服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcCategoryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcCategory> _rep;
    public FlcCategoryService(SqlSugarRepository<FlcCategory> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询商品分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcCategoryOutput>> Page(FlcCategoryInput input)
    {
        var query = _rep.AsQueryable()
             .Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.CategoryName.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.CategoryName), u => u.CategoryName.Contains(input.CategoryName.Trim()))
            .WhereIF(input.SuperiorId>0, u => u.SuperiorId == input.SuperiorId)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<FlcCategory>((u, superiorid) => u.SuperiorId == superiorid.Id )
            .OrderBy(u => u.CreateTime)
            .Select((u, superiorid) => new FlcCategoryOutput
            {
                CategoryName = u.CategoryName,
                CreateOrgId = u.CreateOrgId,
                CreateOrgName = u.CreateOrgName,
                Id = u.Id,
                SuperiorId = u.SuperiorId,  
                SuperiorIdCategoryName = superiorid.CategoryName,
            });
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加商品分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcCategoryInput input)
    {
        var entity = input.Adapt<FlcCategory>();
        var clist=_rep.AsQueryable().Where(u=>u.IsDelete==false).ToList();
        bool ok=true;
        foreach (var c in clist)
        {
            if(c.CategoryName== entity.CategoryName)
            {
                ok=false;
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
    /// 删除商品分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcCategoryInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        var zlist=_rep.AsQueryable().Where(u=>u.SuperiorId==entity.Id&&u.IsDelete==false).First();
        if (zlist == null)
        {
            await _rep.FakeDeleteAsync(entity);   //假删除
            //await _rep.DeleteAsync(entity);   //真删除
        }
        else
        {
            throw Oops.Oh(ErrorCodeEnum.D1007);
        }

    }

    /// <summary>
    /// 更新商品分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcCategoryInput input)
    {
        var entity = input.Adapt<FlcCategory>();
        var clist = _rep.AsQueryable().Where(u => u.IsDelete == false&&u.Id!=entity.Id).ToList();
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
            await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }
        else
        {
            throw Oops.Oh(ErrorCodeEnum.D1006);
        }


    }

    /// <summary>
    /// 获取商品分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcCategory> Detail([FromQuery] QueryByIdFlcCategoryInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取商品分类列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcCategoryOutput>> List([FromQuery] FlcCategoryInput input)
    {
        return await _rep.AsQueryable().Select<FlcCategoryOutput>().ToListAsync();
    }




    [HttpGet]
    [ApiDescriptionSettings(Name = "FlcCategoryTree")]
    public async Task<dynamic> FlcCategoryTree()
    {
        return await _rep.Context.Queryable<FlcCategory>().ToTreeAsync(u => u.Children, u => u.SuperiorId, 0);
    }

}

