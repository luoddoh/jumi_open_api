using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using Admin.NET.Application.Service.FlcProcureReturnDetail.Dto;
namespace Admin.NET.Application;
/// <summary>
/// 退货明细服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcProcureReturnDetailService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcProcureReturnDetail> _rep;
    public FlcProcureReturnDetailService(SqlSugarRepository<FlcProcureReturnDetail> rep)
    {
        _rep = rep;

    }

    /// <summary>
    /// 更新退货明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(List<FlcProcureReturnDetailUpdate> inputList)
    {
        var oringList = _rep.AsQueryable().Where(x => x.IsDelete == false && x.ReturnId == inputList[0].ReturnId).ToList();
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
                var del_entity = oring.Adapt<FlcProcureReturnDetail>();
                await _rep.FakeDeleteAsync(del_entity);  //假删除
            }
        }
        foreach (var input in inputList)
        {
            var row = _rep.AsQueryable()
                .Where(x => x.IsDelete == false && x.ReturnId == input.ReturnId && x.SkuId == input.SkuId).First();
            if (row != null)
            {
                var entity = input.Adapt<FlcProcureReturnDetail>();
                await _rep.AsUpdateable(entity).ExecuteCommandAsync();
            }
            else
            {
               
                var entity = input.Adapt<FlcProcureReturnDetail>();
                await _rep.InsertAsync(entity);
            }
        }
    }

    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcProcureReturnDetailOutput>> List([FromQuery] FlcProcureReturnDetailIntput input)
    {
        var list = _rep.AsQueryable()
            .LeftJoin <FlcInventory>((x,inv)=>x.SkuId==inv.SkuId)
            .LeftJoin<FlcGoods>((x, inv,good) => x.GoodsId==good.Id)
            .LeftJoin<FlcGoodsUnit>((x, inv, good,unit) => x.UnitId == unit.Id)
            .Where(x=>x.ReturnId==input.ReturnId&&x.IsDelete==false)
            .Select((x,inv,good, unit) =>new FlcProcureReturnDetailOutput()
            {
                Id=x.Id,
                GoodsId = x.ReturnId,
                ReturnId = x.ReturnId,
                ReturnNum=x.ReturnNum,
                ReturnPrice=x.ReturnPrice,
                InventoryNum=inv.Number,
                SkuId=x.SkuId,
                TotalAmount=x.TotalAmount,
                Remark=x.Remark,
                GoodsName= good.GoodsName,
                UnitId=x.UnitId,
                UnitName= unit.UnitName,
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

    /// <summary>
    /// 获取退货订单Id列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "FlcProcureReturnIdDropdown"), HttpGet]
    public async Task<dynamic> FlcProcureReturnIdDropdown()
    {
        return await _rep.Context.Queryable<FlcProcureReturn>()
                .Select(u => new
                {
                    Label = u.DocNumber,
                    Value = u.Id
                }
                ).ToListAsync();
    }
    /// <summary>
    /// 获取商品Id列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "FlcGoodsGoodsIdDropdown"), HttpGet]
    public async Task<dynamic> FlcGoodsGoodsIdDropdown()
    {
        return await _rep.Context.Queryable<FlcGoods>()
                .Select(u => new
                {
                    Label = u.GoodsName,
                    Value = u.Id
                }
                ).ToListAsync();
    }
    /// <summary>
    /// 获取商品sku列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "FlcGoodsSkuSkuIdDropdown"), HttpGet]
    public async Task<dynamic> FlcGoodsSkuSkuIdDropdown()
    {
        return await _rep.Context.Queryable<FlcGoodsSku>()
                .Select(u => new
                {
                    Label = u.Id,
                    Value = u.Id
                }
                ).ToListAsync();
    }


}
