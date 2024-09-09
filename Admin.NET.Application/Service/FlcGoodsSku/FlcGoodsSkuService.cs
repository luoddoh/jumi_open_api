using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using Admin.NET.Core;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaBusinessPerformanceBootResponse.Types.Data.Types.Body.Types.Table.Types;
using System.Data;
namespace Admin.NET.Application;
/// <summary>
/// 商品sku表服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcGoodsSkuService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcGoodsSku> _rep;
    private readonly SqlSugarRepository<FlcSkuSpeValue> _rep_v;
    public FlcGoodsSkuService(SqlSugarRepository<FlcGoodsSku> rep, SqlSugarRepository<FlcSkuSpeValue> rep_v)
    {
        _rep = rep;
        _rep_v = rep_v;
    }

    /// <summary>
    /// 分页查询商品sku表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcGoodsSkuOutputs>> Page(FlcGoodsSkuInput input)
    {
        string barcode = "";
        if (!string.IsNullOrWhiteSpace(input.BarCode))
        {
            if (input.BarCode.Length > 7)
            {
                barcode = input.BarCode.Substring(0, 7);
            }
            else
            {
                barcode = input.BarCode;
            }

        }
        var no_query = _rep.Context.Queryable<FlcGoodsSku>()
            .LeftJoin<FlcSkuSpeValue>((sku, link) => sku.Id == link.SkuId)
            .LeftJoin<FlcSpecificationValue>((sku, link, val)=> link.SpeValueId==val.Id)
            .LeftJoin<FlcProductSpecifications>((sku, link, val,spe)=>val.SpecificationId==spe.Id)
            .Where((sku, link, val, spe)=>sku.IsDelete==false&&spe.Enable==false)
            .Select((sku, link, val, spe) => sku.Id)
            .ToList();
        var no_good = _rep.Context.Queryable<FlcGoods>()
             .LeftJoin<FlcProductSpecifications>((goods, spe) => goods.GoodsName == spe.SpeName)
             .Where((goods, spe) => goods.IsDelete == false && spe.Enable == false)
             .Select((goods, spe) => goods.Id)
             .ToList();
        var db = _rep.Context;
        var db_v = _rep_v.Context;
        var list = db.Queryable<FlcGoodsSku>()
           .Includes(x => x.flcGoods)
           .Includes(x => x.flcGoodsUnit)
           .Where(x=>x.IsDelete==false&&x.flcGoods.IsDelete==false)
           .LeftJoin<FlcInventory>((x,p)=>x.Id==p.SkuId)
           .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey),x=>x.flcGoods.GoodsName.Contains(input.SearchKey))
           .WhereIF(!string.IsNullOrWhiteSpace(barcode), x => x.BarCode== barcode)
           .WhereIF(no_good.Count > 0, x => !no_good.Contains(x.GoodsId))
           .WhereIF(no_query.Count>0, x => !no_query.Contains(x.Id))
            .Where(x => x.IsDelete == false)
            .Select((x,p)=>new FlcGoodsSkuOutputs
            {
                Id = x.Id,
                BarCode = x.BarCode,
                CostPrice = x.CostPrice,
                CoverImage = x.CoverImage,
                GoodsId = x.GoodsId,
                SalesPrice = x.SalesPrice,
                UnitId = x.UnitId,
                GoodsName=x.flcGoods.GoodsName,
                UnitName=x.flcGoodsUnit.UnitName,
                InventoryNum=p.Number,
            })
           .ToList();

        db.ThenMapper(list, sku =>
        {
            sku.speValueList = _rep_v.AsQueryable().Includes(x => x.FlcSpecificationValue)
            .Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => sku.Id, sku)
            .Select(x => new labval
            {
                Id = x.SpeValueId,
                SpecificationId = x.FlcSpecificationValue.SpecificationId,
                SpeValue = x.FlcSpecificationValue.SpeValue
            }).ToList();
        });
        if (!string.IsNullOrWhiteSpace(input.spevalue))
        {
            list = list.Where(u => u.speValueList.Find(x => x.SpeValue.Contains(input.spevalue)) != null).ToList();
        }
        return list.ToPagedList(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加商品sku表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcGoodsSkuInput input)
    {
        var entity = input.Adapt<FlcGoodsSku>();
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 删除商品sku表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcGoodsSkuInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新商品sku表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(List<UpdateFlcGoodsSkuInput> inputs)
    {
        var db = _rep.Context;
        var db_v = _rep_v.Context;
        var on_query = _rep.Context.Queryable<FlcGoods>()
           .LeftJoin<FlcProductSpecifications>((goods, spe) => goods.GoodsName == spe.SpeName)
           .Where((goods, spe) => goods.IsDelete == false && spe.Enable == true)
           .LeftJoin<FlcGoodsSku>((goods, spe,sku)=>goods.Id==sku.GoodsId)
           .Where((goods, spe, sku)=>sku.IsDelete==false)
           .Select((goods, spe, sku) => sku)
           .ToList();
        var db_all = db.Queryable<FlcGoodsSku>().Where(x => x.IsDelete == false && x.GoodsId == inputs[0].GoodsId).ToList();
        foreach (var FlcGoodsSku in db_all)
        {
            bool isdel = true;
            foreach (var input in inputs)
            {
                var barcode_row= on_query.Where(x=>x.BarCode == input.BarCode&&x.GoodsId!=input.GoodsId).FirstOrDefault();
                if (barcode_row != null)
                {
                    throw Oops.Oh(ErrorCodeEnum.D1006);
                }
                else
                {
                    if (FlcGoodsSku.GoodsId == input.GoodsId && FlcGoodsSku.BarCode == input.BarCode)
                    {
                        isdel = false;
                        break;
                    }
                }
            }
            if (isdel)
            {
                await db.FakeDeleteAsync(FlcGoodsSku);
            }
        }
        foreach (var input in inputs)
        {
            var entity = input.Adapt<FlcGoodsSku>();
            var row = on_query.Where(r => r.GoodsId == input.GoodsId && r.BarCode == input.BarCode).FirstOrDefault();
            if (row != null)
            {
                entity.Id = row.Id;
                db.Updateable(entity).ExecuteCommand();
            }
            else
            {
                entity.Id = db.Insertable(entity).ExecuteReturnSnowflakeId();
            }
            var id = entity.Id;
            foreach (var item in input.SpeValueList)
            {
                FlcSkuSpeValueInput speValueInput = new FlcSkuSpeValueInput()
                {
                    SkuId = id,
                    SpeValueId = item.Id
                };
                var entity_v = speValueInput.Adapt<FlcSkuSpeValue>();

                var row_v = db_v.Queryable<FlcSkuSpeValue>().Where(r => r.SkuId == speValueInput.SkuId && r.SpeValueId == speValueInput.SpeValueId).First();
                if (row_v == null)
                {
                    db_v.Insertable(entity_v).ExecuteCommand();
                }
            }
        }


        //await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取商品sku表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcGoodsSku> Detail([FromQuery] QueryByIdFlcGoodsSkuInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取商品sku表列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcGoodsSkuOutput>> List([FromQuery] FlcGoodsSkuInput input)
    {
        return await _rep.AsQueryable().Select<FlcGoodsSkuOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取商品sku列表(根据商品Id)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "ByIdList")]
    public async Task<List<FlcGoodsSkuOutputs>> ByIdList([FromQuery] FlcGoodsSkuInputById input)
    {
        var db = _rep.Context;
        var db_v = _rep_v.Context;
        var list = db.Queryable<FlcGoodsSku>().Where(x => x.GoodsId == input.GoodsId&&x.IsDelete==false).Select<FlcGoodsSkuOutputs>().ToList();
        db.ThenMapper(list, sku =>
        {
            sku.speValueList = _rep_v.AsQueryable().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => sku.Id, sku)
            .Select(x => new labval {
                Id=x.SpeValueId,
                SpecificationId=x.FlcSpecificationValue.SpecificationId,
                SpeValue=x.FlcSpecificationValue.SpeValue
            }).ToList();
        });
        return  list;
        //return await _rep.AsQueryable()
        //    .Includes(x => x.SpeValueList.Select(z=>new FlcSkuSpeValue { FlcSpecificationValue=z.FlcSpecificationValue}).ToList(), ss => ss.FlcSpecificationValue )
        //    .Where(x=>x.GoodsId==input.GoodsId)
        //    .Select<FlcGoodsSku>().ToListAsync();
    }

    /// <summary>
    /// 合并重复sku数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "help")]
    public async Task<dynamic> help()
    {
        var query = _rep.Context.Ado.GetDataTable(@"select GoodsName,sku.Id,sku.GoodsId,BarCode,iny.Number,sku.CreateTime,sku.UpdateTime,sku.CostPrice from flc_goods_sku sku 
join flc_goods goods on goods.Id=sku.GoodsId 
join flc_inventory iny on iny.SkuId=sku.Id
where sku.IsDelete=0 and goods.IsDelete=0 and BarCode!='' and BarCode in (select BarCode from flc_goods_sku sku join flc_sku_speValue link on link.SkuId=sku.Id 
join flc_specification_value val on val.Id=link.SpeValueId 
join flc_product_specifications spe on spe.Id=val.SpecificationId 
join flc_goods goods on goods.Id=sku.GoodsId
where spe.Enable=1 and sku.IsDelete=0 and goods.IsDelete=0 and BarCode!='' group by BarCode having count(sku.Id)>1) order by BarCode", new {});

        List<helpModel> info = new List<helpModel>();
        foreach(DataRow row in query.Rows)
        {
            if (info.Count == 0)
            {
                info.Add(new helpModel
                {
                    barCode = row["BarCode"].ToString(),
                    sku_list=new List<helpSkuModel>
                    {
                        new helpSkuModel
                        {
                            skuId=Convert.ToInt64( row["Id"]),
                            number=Convert.ToInt32( row["Number"]),
                            costPrice=Convert.ToDecimal( row["CostPrice"]),
                            update=row["UpdateTime"]==DBNull.Value?null:Convert.ToDateTime(row["UpdateTime"])
                        }
                    }
                });
            }
            else
            {
                int index = info.FindIndex(u => u.barCode == row["BarCode"].ToString());
                if(index == -1)
                {
                    info.Add(new helpModel
                    {
                        barCode = row["BarCode"].ToString(),
                        sku_list = new List<helpSkuModel>
                    {
                        new helpSkuModel
                        {
                            skuId=Convert.ToInt64( row["Id"]),
                            number=Convert.ToInt32( row["Number"]),
                             costPrice=Convert.ToDecimal( row["CostPrice"]),
                            update=row["UpdateTime"]==DBNull.Value?null:Convert.ToDateTime(row["UpdateTime"])
                        }
                    }
                    });
                }
                else
                {
                    info[index].sku_list.Add(new helpSkuModel
                    {
                        skuId = Convert.ToInt64(row["Id"]),
                        number = Convert.ToInt32(row["Number"]),
                        costPrice = Convert.ToDecimal(row["CostPrice"]),
                        update = row["UpdateTime"] == DBNull.Value ? null : Convert.ToDateTime(row["UpdateTime"])
                    });
                }
            }
        }
        _rep.Context.Ado.BeginTran();
        try
        {
            foreach (var item in info)
            {
                var sku_list_new = item.sku_list.OrderByDescending(u => u.update).ToList();
                var sku_new = sku_list_new[0];
                sku_new.number = sku_list_new.Sum(u => u.number);
                _rep.Context.Updateable<FlcInventory>().SetColumns(u =>new FlcInventory(){Number=sku_new.number,TotalAmount= sku_new.number * sku_new.costPrice }  ).Where(u => u.SkuId == sku_new.skuId).ExecuteCommand();
                for (int i = 1; i < sku_list_new.Count; i++)
                {
                    var sku_old = sku_list_new[i];
                    _rep.Context.Updateable<FlcProcureDetail>().SetColumns(u => u.SkuId == sku_new.skuId).Where(u => u.SkuId == sku_old.skuId).ExecuteCommand();
                    _rep.Context.Updateable<FlcInventoryCheckDetail>().SetColumns(u => u.SkuId == sku_new.skuId).Where(u => u.SkuId == sku_old.skuId).ExecuteCommand();
                    _rep.Context.Updateable<FlcInventoryInputDetail>().SetColumns(u => u.SkuId == sku_new.skuId).Where(u => u.SkuId == sku_old.skuId).ExecuteCommand();
                    _rep.Context.Updateable<FlcInventoryOutDetail>().SetColumns(u => u.SkuId == sku_new.skuId).Where(u => u.SkuId == sku_old.skuId).ExecuteCommand();
                    _rep.Context.Updateable<FlcGoodsSku>().SetColumns(u => u.IsDelete == true).Where(u => u.Id == sku_old.skuId).ExecuteCommand();
                }

            }
            _rep.Context.Ado.CommitTran();
            return new { msg = "success" };
        }
        catch (Exception e)
        {
            _rep.Context.Ado.RollbackTran();
            return new {msg=e.Message};
        }
    }
    public class helpModel
    {
        public string barCode;
        public List<helpSkuModel> sku_list;
    }
    public class helpSkuModel
    {
        public long skuId;
        public int number;
        public decimal costPrice;
        public DateTime? update;
    }
}

