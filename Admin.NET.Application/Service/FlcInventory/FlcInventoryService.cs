using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Admin.NET.Core;
using Nest;
namespace Admin.NET.Application;
/// <summary>
/// 库存查询服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventory> _rep;
    public FlcInventoryService(SqlSugarRepository<FlcInventory> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询库存查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcInventoryOutput>> Page(FlcInventoryInput input)
    {
        var query = _rep.AsQueryable()
                //处理外键和TreeSelector相关字段的连接
                .Where(u => u.IsDelete == false)
            .LeftJoin<FlcGoodsSku>((u, skuid) => u.SkuId == skuid.Id )
            .LeftJoin<FlcGoods>((u, skuid,goods) => skuid.GoodsId==goods.Id)
            .LeftJoin<FlcGoodsUnit>((u, skuid, goods,unit) => skuid.UnitId == unit.Id)
            .LeftJoin<FlcSkuSpeValue>((u, skuid, goods, unit,line)=>skuid.Id==line.SkuId&&line.IsDelete==false)
            .LeftJoin<FlcSpecificationValue>((u, skuid, goods, unit, line,value) => line.SpeValueId == value.Id && value.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), (u, skuid, goods, unit, line, value) => goods.GoodsName.Contains(input.SearchKey.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.spevalue), (u, skuid, goods, unit, line, value) => value.SpeValue.Contains(input.spevalue.Trim()))
            .WhereIF(input.MinTotalAmount!=null,u=>u.TotalAmount<= input.MinTotalAmount)
            .WhereIF(input.MaxTotalAmount != null, u => u.TotalAmount >= input.MaxTotalAmount)
            .WhereIF(input.minNumber != null, u => u.Number <= input.minNumber)
            .WhereIF(input.maxNumber != null, u => u.Number >= input.maxNumber)
            .Where((u, skuid, goods)=>skuid.IsDelete==false&&goods.IsDelete==false)
            //.OrderBy(u => u.CreateTime)
            .Select((u, skuid, goods, unit, line, value) => new FlcInventoryOutput
            {
                Id = u.Id,
                SkuId = u.SkuId, 
                GoodesName=goods.GoodsName,
                UnitName=unit.UnitName,
                SkuIdBarCode = skuid.BarCode,
                Number = u.Number,
                TotalAmount = u.TotalAmount,
                speValue= value.SpeValue
            }).ToList();
        int totlenum = 0;
        decimal totleamont = 0;
        foreach(var item in query)
        {
            totlenum+=item.Number;
            totleamont += item.TotalAmount;
        }
        var a = query.ToPagedList(input.Page, input.PageSize);
        FlcOutputpage<FlcInventoryOutput> result = new FlcOutputpage<FlcInventoryOutput>();
        result.Page = a.Page;
        result.PageSize = a.PageSize;
        result.Items=a.Items;
        result.Total = a.Total;
        result.TotalPages = a.TotalPages;
        result.HasPrevPage = a.HasPrevPage;
        result.HasNextPage = a.HasNextPage;
        result.TotalNumber=totlenum;
        result.TotalAmount=totleamont;

        return result;
    }

    /// <summary>
    /// 增加库存查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcInventoryInput input)
    {
        var entity = input.Adapt<FlcInventory>();
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 删除库存查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcInventoryInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新库存查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcInventoryInput input)
    {
        var entity = input.Adapt<FlcInventory>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取库存查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcInventory> Detail([FromQuery] QueryByIdFlcInventoryInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取库存查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcInventoryOutput>> List([FromQuery] FlcInventoryInput input)
    {
        return await _rep.AsQueryable().Select<FlcInventoryOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取sku表id列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "FlcGoodsSkuSkuIdDropdown"), HttpGet]
    public async Task<dynamic> FlcGoodsSkuSkuIdDropdown()
    {
        return await _rep.Context.Queryable<FlcGoodsSku>()
                .Select(u => new
                {
                    Label = u.BarCode,
                    Value = u.Id
                }
                ).ToListAsync();
    }




}

