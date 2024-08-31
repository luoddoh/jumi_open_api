using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Admin.NET.Core;
using Nest;
using System.Linq;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
namespace Admin.NET.Application;
/// <summary>
/// 库存查询服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcProcureStatisticsService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcProcure> _rep;
    private readonly SqlSugarRepository<FlcGoodsSku> _sku;
    private readonly SqlSugarRepository<FlcSkuSpeValue> _rep_v;
    private readonly SqlSugarRepository<FlcCategory> _Category;
    public FlcProcureStatisticsService(SqlSugarRepository<FlcProcure> rep, SqlSugarRepository<FlcSkuSpeValue> rep_v, SqlSugarRepository<FlcGoodsSku> sku, SqlSugarRepository<FlcCategory> category)
    {
        _rep = rep;
        _rep_v = rep_v;
        _sku = sku;
        _Category = category;

    }

    /// <summary>
    /// 分页查询库存查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcProcureStatisticsOutput>> Page(FlcProcureStatisticsInput input)
    {
        var categoryData=_Category.AsQueryable().Where(u=>u.IsDelete==false).ToList();
        var query = _rep.AsQueryable()
            .WhereIF(input.docTimeRange != null, u => u.ProcurementTime >= input.docTimeRange[0] && u.ProcurementTime <= input.docTimeRange[1])
            .LeftJoin<FlcProcureDetail>((u,d)=>u.Id==d.ProcureId)
            .Where((u,d)=>u.IsDelete==false&&d.IsDelete==false)
            .LeftJoin<FlcSupplierInfo>((u, d, sup) => u.SupplierId==sup.Id&&sup.IsDelete==false)
            .WhereIF(input.supplierId!=null,(u,d)=>u.SupplierId==input.supplierId)
            .LeftJoin<FlcGoodsSku>((u, d, sup, sku)=>d.SkuId==sku.Id)
            .LeftJoin<FlcGoods>((u, d, sup, sku, goods) => sku.GoodsId == goods.Id)
            .WhereIF(!string.IsNullOrWhiteSpace(input.goodsName), (u, d, sup, sku, goods) => goods.GoodsName.Contains(input.goodsName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.goodsCode), (u, d, sup, sku, goods) => goods.ProductCode == input.goodsCode)
            .WhereIF(input.categoryId != null && input.categoryId.Count > 0, (u, d, sup, sku, goods) => input.categoryId.Contains(goods.CategoryId))
            .WhereIF(!string.IsNullOrWhiteSpace(input.barCode), (u, d, sup, sku, goods) => sku.BarCode == input.barCode)
            .GroupBy((u,d, sup) =>new
            {
                d.SkuId,
                sup.Id,
            })
            .Select((u, d, sup, sku, goods) => new FlcProcureStatisticsOutput
            {
                Id = d.SkuId,
                ProcureTotalAmount= SqlFunc.AggregateSumNoNull(d.totalAmount),
                supplierId= sup.Id,
                supplierName = SqlFunc.AggregateMax(sup.SupName),
                ProcureNumber = SqlFunc.AggregateSumNoNull(d.purchaseNum),
                GoodsName = SqlFunc.AggregateMax(goods.GoodsName),
                GoodsCode = SqlFunc.AggregateMax(goods.ProductCode) ,
                GoodsImg = SqlFunc.AggregateMax(goods.CoverImage) ,
                SkuBarCode = SqlFunc.AggregateMax(sku.BarCode),
                Remark = SqlFunc.AggregateMax(sku.PrintCustom) ,
                categoryId = SqlFunc.AggregateMax(goods.CategoryId),
            })
            .ToList();
        int totalNumber = 0;
        decimal totalAmount = 0;
        _sku.Context.ThenMapper(query, sku =>
        {
            sku.SkuName = string.Join("/", _rep_v.AsQueryable().Includes(x => x.FlcSpecificationValue)
            .Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => sku.Id, sku)
            .Select(x => x.FlcSpecificationValue.SpeValue).ToList());
            totalNumber += sku.ProcureNumber;
            totalAmount += sku.ProcureTotalAmount;

        });
        query = query
            .WhereIF(!string.IsNullOrWhiteSpace(input.skuName),u=>u.SkuName.Contains(input.skuName))
            .ToList();
        int count= query.Count();
        query = query.Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToList();
        foreach (var sku in query)
        {
            sku.CategoryName = getTreeCategoryName(categoryData, sku.categoryId);
        }
        var reuslt= new FlcOutputpage<FlcProcureStatisticsOutput>
        {
            Page = input.Page,
            PageSize = input.PageSize,
            Items = query,
            Total = count,
            TotalAmount = totalAmount,
            TotalNumber = totalNumber,
        };
        return reuslt;
    }
    public static string getTreeCategoryName(List<FlcCategory> data,long categoryId)
    {
        var query= data.Where(u=>u.Id==categoryId).FirstOrDefault();
        if (query != null)
        {
            if (query.SuperiorId > 0)
            {
                return getTreeCategoryName(data, (long)query.SuperiorId) + "/" + query.CategoryName;
            }
            else
            {
                return query.CategoryName;
            }
        }
        else
        {
            return "";
        }
    }
}

