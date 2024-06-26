﻿using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NewLife.Data;
namespace Admin.NET.Application;
/// <summary>
/// 采购明细服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcProcureDetailService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcProcureDetail> _rep;
    private readonly SqlSugarRepository<FlcSkuSpeValue> _rep_v;
    private readonly SqlSugarRepository<FlcGoodsSku> _rep_sku;
    private readonly SqlSugarRepository<FlcInventory> _rep_inventory;
    public FlcProcureDetailService(SqlSugarRepository<FlcProcureDetail> rep, 
        SqlSugarRepository<FlcSkuSpeValue> rep_v,
        SqlSugarRepository<FlcGoodsSku> rep_sku, SqlSugarRepository<FlcInventory> rep_inventory)
    {
        _rep = rep;
        _rep_v = rep_v;
        _rep_sku = rep_sku;
        _rep_inventory = rep_inventory;

    }

    
    /// <summary>
    /// 更新采购明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(List<UpdateFlcProcureDetailInput> inputList)
    {
        var oringList= _rep.AsQueryable().Where(x => x.IsDelete == false && x.ProcureId == inputList[0].ProcureId).ToList();
        foreach (var oring in oringList)
        {
            bool del_ok=true;
            foreach (var item in inputList)
            {
                if (item.SkuId == oring.SkuId)
                {
                    del_ok = false;
                }
            }
            if (del_ok)
            {
                var del_entity = oring.Adapt<FlcProcureDetail>();
                await _rep.DeleteAsync(del_entity);  //假删除
            }
        }
        foreach (var input in inputList)
        {
            var row= _rep.AsQueryable()
                .Includes(x=>x.flcGoodsSku)
                .Where(x => x.IsDelete == false && x.ProcureId == input.ProcureId&&x.SkuId==input.SkuId).First();
            if(row != null)
            {
                //添加唯一条码
                List<string> barCodeList = new List<string>();
                if (!string.IsNullOrEmpty(row.BarCodeList))
                {
                   barCodeList = JsonConvert.DeserializeObject<List<string>>(row.BarCodeList);
                }
                if (barCodeList.Count > input.purchaseNum)
                {
                    barCodeList.RemoveRange(input.purchaseNum - 1, barCodeList.Count - input.purchaseNum);
                }
                else if (barCodeList.Count < input.purchaseNum)
                {
                    int for_index = input.purchaseNum - barCodeList.Count;
                    for (int i = 0; i < for_index; i++)
                    {
                        int number = 1000 + i;
                        var now = DateTime.Now;
                        //sku固定条码（7位）+分（2位）+秒（2位）+毫秒（1~3位）+随机数（3位）= 15-17位
                        barCodeList.Add(row.flcGoodsSku.BarCode + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString() + number.ToString());
                        //barCodeList.Add(row.flcGoodsSku.BarCode + DateTimeOffset.Now.ToUnixTimeSeconds()+new Random().Next(100,999));
                    }
                }
                
                var entity = input.Adapt<FlcProcureDetail>();
                entity.BarCodeList = JsonConvert.SerializeObject(barCodeList);
                await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            }
            else
            {
                //添加唯一条码
                List<string> barCodeList = new List<string>();
                string BarCode= _rep_sku.AsQueryable().Where(x=>x.Id==input.SkuId).First().BarCode;
                for(int i = 0; i < input.purchaseNum; i++)
                {
                    int number= 1000+i;
                    var now = DateTime.Now;
                    barCodeList.Add(BarCode + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString() + number.ToString());
                    //barCodeList.Add(BarCode + DateTimeOffset.Now.ToUnixTimeSeconds() + new Random().Next(100, 999));
                }
                var entity = input.Adapt<FlcProcureDetail>();
                entity.BarCodeList = JsonConvert.SerializeObject(barCodeList);
                await _rep.InsertAsync(entity);
            }
        }
    }

    /// <summary>
    /// 打印
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Print")]
    public async Task Print([FromQuery] PrintInput input)
    {
        var row = _rep.AsQueryable().Where(x=>x.Id==input.Id).First();
        if (row.PrintNum == null)
        {
            row.PrintNum = input.Num;
        }
        else
        {
            row.PrintNum += input.Num;
        }
        await _rep.AsUpdateable(row).ExecuteCommandAsync();
    }
    /// <summary>
    /// 采购验货
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Inspection")]
    public async Task Inspection(List<InspectionFlcProcureDetailInput> inputList)
    {
       
        
        foreach (var input in inputList)
        {
            //更新明细
            var row = _rep.AsQueryable()
                .Where(x=>x.Id==input.Id).First();
            if (row != null)
            {
                input.okNum = input.OneOkNumber + input.okNum;
                var entity = input.Adapt<FlcProcureDetail>();
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
                await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            }
            else
            {
                var entity = input.Adapt<FlcProcureDetail>();
                await _rep.InsertAsync(entity);
            }
            //更新库存
            var row_inventory = _rep_inventory.AsQueryable().Where(x => x.SkuId == input.SkuId&&x.IsDelete==false).First();
            if (row_inventory != null)
            {
                row_inventory.Number += input.OneOkNumber;
                row_inventory.TotalAmount += input.ok_totalAmount;
                await _rep_inventory.AsUpdateable(row_inventory).ExecuteCommandAsync();
            }
            else
            {
                row_inventory=new FlcInventory()
                {
                    SkuId = input.SkuId,
                    Number= input.OneOkNumber,
                    TotalAmount= input.totalAmount,
                    IsDelete =false,
                };
                await _rep_inventory.AsInsertable(row_inventory).ExecuteCommandAsync();
            }
            
        }
        var procure = _rep.Context.Queryable<FlcProcure>().Where(x => x.Id == inputList[0].ProcureId).First();
        var on_detail = _rep.Context.Queryable<FlcProcureDetail>()
            .Where(x=>x.IsDelete == false)
            .Where(x => x.ProcureId == inputList[0].ProcureId).ToList();
        procure.State = 400;
        foreach(var item in on_detail)
        {
            if (item.okNum < item.purchaseNum)
            {
                procure.State = 300;
                break;
            }
        }
        await _rep.Context.Updateable(procure).ExecuteCommandAsync();
    }

    [HttpGet]
    [ApiDescriptionSettings(Name = "BarCodeInfoDetail")]
    public async Task<FlcskuDetailOutput> BarCodeInfoDetail([FromQuery] CodeInput BarCode)
    {
        string code = BarCode.Code.Substring(0,7);
        var quer = _rep.Context.Queryable<FlcGoodsSku>()
            .LeftJoin<FlcGoods>((k, g) => k.GoodsId == g.Id)
            .LeftJoin<FlcGoodsUnit>((k, g, t) => k.UnitId == t.Id)
            .LeftJoin<FlcInventory>((k, g, t, i) => k.Id == i.SkuId)
            .Where((k, g, t, i) => k.BarCode == code && k.IsDelete == false&&g.IsDelete==false)
            .Select((k, g, t, i) => new FlcskuDetailOutput
            {
                Id = k.Id,
                goodsName = g.GoodsName,
                SkuImage = k.CoverImage,
                UnitName = t.UnitName,
                InventoryNum=i.Number,
                Price=k.CostPrice,
            }).First();
        if (quer != null)
        {
            quer.speValueList = _rep_v.AsQueryable().Includes(x => x.FlcSpecificationValue)
            .Where(x => x.IsDelete == false && x.SkuId == quer.Id)
            .Select(x => new labval
            {
                Id = x.SpeValueId,
                SpecificationId = x.FlcSpecificationValue.SpecificationId,
                SpeValue = x.FlcSpecificationValue.SpeValue
            }).ToList();
        }
        
        return quer;
    }
    /// <summary>
    /// 获取采购明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcProcureDetail> Detail([FromQuery] QueryByIdFlcProcureDetailInput input)
    {

        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取采购明细列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcProcureDetailOutput>> List([FromQuery] FlcProcureDetailInput input)
    {
        var db = _rep.Context;
        var db_v = _rep_v.Context;
        var list= _rep.AsQueryable()
            .Includes(x=>x.flcGoods)
            .Includes(x => x.flcGoodsSku.Where(u=>u.IsDelete==false).ToList(),u=>u.flcGoodsUnit)
            .LeftJoin<FlcInventory>((x,p)=>x.SkuId==p.SkuId)
            .Where(x=>x.ProcureId==input.ProcureId&&x.IsDelete==false)
            .Select((x,p)=>new FlcProcureDetailOutput
            {
                Id = x.Id,
                GoodsId=x.GoodsId,
                SkuId=x.SkuId,
                InventoryNum=p.Number,
                noNum=x.noNum,
                okNum=x.okNum,
                ProcureId=x.ProcureId,
                purchaseNum=x.purchaseNum,  
                purchasePrice=x.purchasePrice,
                remark=x.remark,
                SkuImage=x.flcGoodsSku.CoverImage,
                totalAmount=x.totalAmount,
                GoodsName=x.flcGoods.GoodsName,
                UnitName=x.flcGoodsSku.flcGoodsUnit.UnitName,
                PrintNum=x.PrintNum == null ? 0 : x.PrintNum,
                BarCodeList=x.BarCodeList,
                PrintCustom=x.flcGoodsSku.PrintCustom,
                OkCodeList=x.OkCodeList,
            }).ToList();
        db.ThenMapper(list, pdetail =>
        {
            pdetail.speValueList = _rep_v.AsQueryable().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
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
    /// 获取采购订单Id列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "FlcProcureProcureIdDropdown"), HttpGet]
    public async Task<dynamic> FlcProcureProcureIdDropdown()
    {
        return await _rep.Context.Queryable<FlcProcure>()
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

