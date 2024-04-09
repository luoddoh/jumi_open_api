using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 商品单位服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcGoodsUnitService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcGoodsUnit> _rep;
    public FlcGoodsUnitService(SqlSugarRepository<FlcGoodsUnit> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询商品单位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcGoodsUnitOutput>> Page(FlcGoodsUnitInput input)
    {
        var query = _rep.AsQueryable()
            .Where(u=>u.IsDelete==false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.UnitName.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.UnitName), u => u.UnitName.Contains(input.UnitName.Trim()))
            .Select<FlcGoodsUnitOutput>();
        return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加商品单位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcGoodsUnitInput input)
    {
        var entity = input.Adapt<FlcGoodsUnit>();
        var unitlist=_rep.AsQueryable().Where(u=>u.IsDelete==false).ToList();
        bool ok=true;
        foreach (var unit in unitlist)
        {
            if (unit.UnitName == entity.UnitName)
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
    /// 删除商品单位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcGoodsUnitInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新商品单位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcGoodsUnitInput input)
    {
        var entity = input.Adapt<FlcGoodsUnit>();
        var unitlist = _rep.AsQueryable().Where(u => u.IsDelete == false&&u.Id!=entity.Id).ToList();
        bool ok = true;
        foreach (var unit in unitlist)
        {
            if (unit.UnitName == entity.UnitName)
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
    /// 获取商品单位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcGoodsUnit> Detail([FromQuery] QueryByIdFlcGoodsUnitInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取商品单位列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcGoodsUnitOutput>> List([FromQuery] FlcGoodsUnitInput input)
    {
        return await _rep.AsQueryable().Select<FlcGoodsUnitOutput>().ToListAsync();
    }





}

