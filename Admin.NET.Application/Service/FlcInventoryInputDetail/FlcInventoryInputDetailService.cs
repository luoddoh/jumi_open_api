

using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using Admin.NET.Application.Service.FlcProcureReturnDetail.Dto;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ScanProductAddV2Request.Types.Product.Types;
using NPOI.SS.Formula.Functions;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Admin.NET.Application;
/// <summary>
/// 入库明细服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryInputDetailService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventoryInputDetail> _rep;
    public FlcInventoryInputDetailService(SqlSugarRepository<FlcInventoryInputDetail> rep)
    {
        _rep = rep;

    }

    /// <summary>
    /// 更新入库明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(List<FlcInventoryInputDetailOutput> inputList)
    {
        var oringList = _rep.AsQueryable().Where(x => x.IsDelete == false && x.InputId == inputList[0].InputId).ToList();
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
                .Where(x => x.IsDelete == false && x.InputId == input.InputId && x.SkuId == input.SkuId).First();
            if (row != null)
            {
                var entity = input.Adapt<FlcInventoryInputDetail>();
                await _rep.AsUpdateable(entity).ExecuteCommandAsync();
            }
            else
            {

                var entity = input.Adapt<FlcInventoryInputDetail>();
                await _rep.InsertAsync(entity);
            }
        }
    }

    /// <summary>
    /// 更新入库明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update_mini")]
    public async Task Update_mini(List<FlcInventoryInputDetailOutput> inputList)
    {
        var oringList = _rep.AsQueryable().Where(x => x.IsDelete == false && x.InputId == inputList[0].InputId).ToList();
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
                .Where(x => x.IsDelete == false && x.InputId == input.InputId && x.SkuId == input.SkuId).First();
            if (row != null)
            {
                var entity = input.Adapt<FlcInventoryInputDetail>();
                entity.InputNum += input.OneInputNum;
                entity.TotalAmount += (input.OneInputNum* input.Price);
                if (!string.IsNullOrWhiteSpace(input.oneCodeList))
                {
                    if (string.IsNullOrWhiteSpace(entity.OkCodeList))
                    {
                        entity.OkCodeList = input.oneCodeList;
                    }
                    else
                    {
                        List<string> ok = JsonConvert.DeserializeObject<List<string>>(entity.OkCodeList);
                        List<string> one = JsonConvert.DeserializeObject<List<string>>(input.oneCodeList);
                        var list = ok.Concat(one);
                        entity.OkCodeList = JsonConvert.SerializeObject(list);
                    }
                }
                await _rep.AsUpdateable(entity).ExecuteCommandAsync();
            }
            else
            {
                var entity = input.Adapt<FlcInventoryInputDetail>();
                entity.InputNum = input.OneInputNum;
                entity.TotalAmount = (input.OneInputNum * input.Price);
                entity.OkCodeList = JsonConvert.SerializeObject(input.oneCodeList);
                await _rep.InsertAsync(entity);
            }
        }
    }
    /// <summary>
    /// 对应入库单明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcInventoryInputDetailOutput>> List([FromQuery] FlcInventoryInputDetailInputList input)
    {
        var list = _rep.AsQueryable()
            .LeftJoin<FlcGoodsSku>((x, sku) => x.SkuId == sku.Id)
            .LeftJoin<FlcGoods>((x, sku, good) => sku.GoodsId == good.Id)
            .LeftJoin<FlcGoodsUnit>((x, sku, good, unit) => sku.UnitId == unit.Id)
             .LeftJoin<FlcInventory>((x, sku, good, unit, inv) => x.SkuId == inv.SkuId)
            .Where(x => x.InputId == input.InputId && x.IsDelete == false)
            .Select((x, sku, good, unit, inv) => new FlcInventoryInputDetailOutput()
            {
                Id = x.Id,
                InputId = x.InputId,
                SkuId = x.SkuId,
                GoodsName = good.GoodsName,
                UnitName = unit.UnitName,
                InventoryNum = inv.Number,
                SkuImage = sku.CoverImage,
                Price = x.Price,
                InputNum = x.InputNum,
                OkCodeList= x.OkCodeList,
                TotalAmount = x.TotalAmount,
                Remark = x.Remark,
            }).ToList();
        _rep.Context.ThenMapper(list, pdetail =>
        {
            pdetail.speValueList = _rep.Context.Queryable<FlcSkuSpeValue>().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
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
