using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using Admin.NET.Application.Service.FlcProcureReturnDetail.Dto;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ScanProductAddV2Request.Types.Product.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using Newtonsoft.Json;
namespace Admin.NET.Application;
/// <summary>
/// 盘点明细服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryCheckDetailService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventoryCheckDetail> _rep;
    public FlcInventoryCheckDetailService(SqlSugarRepository<FlcInventoryCheckDetail> rep)
    {
        _rep = rep;

    }

    /// <summary>
    /// 更新盘点明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(List<FlcInventoryCheckDetailOutput> inputList)
    {
        var oringList = _rep.AsQueryable().Where(x => x.IsDelete == false && x.CheckId == inputList[0].CheckId).ToList();
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
                .Where(x => x.IsDelete == false && x.CheckId == input.CheckId && x.SkuId == input.SkuId).First();
            if (row != null)
            {
                var entity = input.Adapt<FlcInventoryCheckDetail>();
                await _rep.AsUpdateable(entity).ExecuteCommandAsync();
            }
            else
            {
               
                var entity = input.Adapt<FlcInventoryCheckDetail>();
                await _rep.InsertAsync(entity);
            }
        }
    }
    /// <summary>
    /// 更新盘点明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update_mini")]
    public async Task Update_mini(List<FlcInventoryCheckDetailOutput> inputList)
    {
        var oringList = _rep.AsQueryable().Where(x => x.IsDelete == false && x.CheckId == inputList[0].CheckId).ToList();
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
                .Where(x => x.IsDelete == false && x.CheckId == input.CheckId && x.SkuId == input.SkuId).First();
            if (row != null)
            {
                var entity = input.Adapt<FlcInventoryCheckDetail>();
                entity.CheckNum += input.OneCheckNum;
                entity.TotalAmount += (input.OneCheckNum*input.Price);
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

                var entity = input.Adapt<FlcInventoryCheckDetail>();
                await _rep.InsertAsync(entity);
            }
        }
    }
    /// <summary>
    /// 对应盘点单明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcInventoryCheckDetailOutput>> List([FromQuery] FlcInventoryCheckDetailInputList input)
    {
        var list = _rep.AsQueryable()
            .LeftJoin<FlcGoodsSku>((x, sku)=>x.SkuId==sku.Id)
            .LeftJoin<FlcGoods>((x, sku, good) => sku.GoodsId==good.Id)
            .LeftJoin<FlcGoodsUnit>((x, sku, good,unit) => sku.UnitId == unit.Id)
            .LeftJoin<FlcInventory>((x, sku, good, unit,inv)=>x.SkuId==inv.SkuId)
            .Where(x=>x.CheckId == input.CheckId && x.IsDelete==false)
            .Select((x, sku, good, unit, inv) =>new FlcInventoryCheckDetailOutput()
            {
                Id=x.Id,
                CheckId = x.CheckId,
                SkuId=x.SkuId,
                GoodsName=good.GoodsName,
                UnitName=unit.UnitName,
                InventoryNum= inv.Number,
                SkuImage=sku.CoverImage,
                Price=x.Price,
                CheckNum = x.CheckNum,
                OkCodeList=x.OkCodeList,
                TotalAmount=x.TotalAmount,
                DifferenceNum=x.DifferenceNum,
                DifferencePrice=x.DifferencePrice,
                Remark =x.Remark,
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
