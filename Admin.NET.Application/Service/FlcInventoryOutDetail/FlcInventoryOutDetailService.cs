using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using Admin.NET.Application.Service.FlcProcureReturnDetail.Dto;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ScanProductAddV2Request.Types.Product.Types;
using NPOI.SS.Formula.Functions;
namespace Admin.NET.Application;
/// <summary>
/// 出库明细服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryOutDetailService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventoryOutDetail> _rep;
    public FlcInventoryOutDetailService(SqlSugarRepository<FlcInventoryOutDetail> rep)
    {
        _rep = rep;

    }

    /// <summary>
    /// 更新出库明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(List<FlcInventoryOutDetailOutput> inputList)
    {
        var oringList = _rep.AsQueryable().Where(x => x.IsDelete == false && x.OutId == inputList[0].OutId).ToList();
        foreach (var oring in oringList)
        {
            bool del_ok = true;
            foreach (var item in inputList)
            {
                if (item.SkuId == oring.SkuId)
                {
                    del_ok = false;
                }
            }
            if (del_ok)
            {
                var del_entity = oring.Adapt<FlcInventoryOutDetail>();
                await _rep.FakeDeleteAsync(del_entity);  //假删除
            }
        }
        foreach (var input in inputList)
        {
            var row = _rep.AsQueryable()
                .Where(x => x.IsDelete == false && x.OutId == input.OutId && x.SkuId == input.SkuId).First();
            if (row != null)
            {
                var entity = input.Adapt<FlcInventoryOutDetail>();
                await _rep.AsUpdateable(entity).ExecuteCommandAsync();
            }
            else
            {
               
                var entity = input.Adapt<FlcInventoryOutDetail>();
                await _rep.InsertAsync(entity);
            }
        }
    }
    /// <summary>
    /// 更新出库明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update_mini")]
    public async Task Update_mini(List<FlcInventoryOutDetailOutput> inputList)
    {
        var oringList = _rep.AsQueryable().Where(x => x.IsDelete == false && x.OutId == inputList[0].OutId).ToList();
        foreach (var oring in oringList)
        {
            bool del_ok = true;
            foreach (var item in inputList)
            {
                if (item.SkuId == oring.SkuId)
                {
                    del_ok = false;
                }
            }
            if (del_ok)
            {
                var del_entity = oring.Adapt<FlcInventoryOutDetail>();
                await _rep.FakeDeleteAsync(del_entity);  //假删除
            }
        }
        foreach (var input in inputList)
        {
            var row = _rep.AsQueryable()
                .Where(x => x.IsDelete == false && x.OutId == input.OutId && x.SkuId == input.SkuId).First();
            if (row != null)
            {
                var entity = input.Adapt<FlcInventoryOutDetail>();
                entity.OutNum+=row.OutNum;
                entity.TotalAmount += row.TotalAmount;
                await _rep.AsUpdateable(entity).ExecuteCommandAsync();
            }
            else
            {

                var entity = input.Adapt<FlcInventoryOutDetail>();
                await _rep.InsertAsync(entity);
            }
        }
    }
    /// <summary>
    /// 对应出库单明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcInventoryOutDetailOutput>> List([FromQuery] FlcInventoryOutDetailInputList input)
    {
        var list = _rep.AsQueryable()
            .LeftJoin<FlcGoodsSku>((x, sku)=>x.SkuId==sku.Id)
            .LeftJoin<FlcGoods>((x, sku, good) => sku.GoodsId==good.Id)
            .LeftJoin<FlcGoodsUnit>((x, sku, good,unit) => sku.UnitId == unit.Id)
             .LeftJoin<FlcInventory>((x, sku, good, unit, inv) => x.SkuId == inv.SkuId)
            .Where(x=>x.OutId==input.OutId&&x.IsDelete==false)
            .Select((x, sku, good, unit, inv) =>new FlcInventoryOutDetailOutput()
            {
                Id=x.Id,
                OutId=x.OutId,
                SkuId=x.SkuId,
                GoodsName=good.GoodsName,
                UnitName=unit.UnitName,
                InventoryNum= inv.Number,
                SkuImage=sku.CoverImage,
                Price=x.Price,
                OutNum=x.OutNum,
                TotalAmount=x.TotalAmount,
                Remark=x.Remark,
            }).ToList();
        _rep.Context.ThenMapper(list, pdetail =>
        {
            pdetail.speValueList = _rep.Context.Queryable<FlcSkuSpeValue>().Includes(x => x.FlcSpecificationValue).Where(x=>x.IsDelete == false)
            .SetContext(x => x.SkuId, () => pdetail.SkuId, pdetail)
            .Select(x => new labval
            {
                Id = x.SpeValueId,
                SpecificationId = x.FlcSpecificationValue.SpecificationId,
                SpeValue = x.FlcSpecificationValue.SpeValue
            }).ToList();
        });
        return list;
    }

    


}
