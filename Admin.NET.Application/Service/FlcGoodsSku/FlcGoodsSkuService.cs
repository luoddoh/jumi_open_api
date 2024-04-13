using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using Admin.NET.Core;
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
            try
            {
                barcode = input.BarCode.Substring(0, 7);
            }
            catch (Exception )
            {
                throw Oops.Oh(ErrorCodeEnum.D1002);
            }
            
        }
        var db = _rep.Context;
        var db_v = _rep_v.Context;
        var list = db.Queryable<FlcGoodsSku>()
           .Includes(x => x.flcGoods)
           .Includes(x => x.flcGoodsUnit)
           .Where(x=>x.IsDelete==false&&x.flcGoods.IsDelete==false)
           .LeftJoin<FlcInventory>((x,p)=>x.Id==p.SkuId)
           .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey),x=>x.flcGoods.GoodsName.Contains(input.SearchKey))
           .WhereIF(!string.IsNullOrWhiteSpace(barcode), x => x.BarCode== barcode)
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
            sku.speValueList = _rep_v.AsQueryable().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
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

        var db_all = db.Queryable<FlcGoodsSku>().Where(x => x.IsDelete == false && x.GoodsId == inputs[0].GoodsId).ToList();
        foreach (var FlcGoodsSku in db_all)
        {
            bool isdel = true;
            foreach (var input in inputs)
            {
                var barcode_row= db.Queryable<FlcGoodsSku>().Where(x=>x.IsDelete==false&&x.BarCode == input.BarCode&&x.GoodsId!=input.GoodsId).First();
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

            var row = db.Queryable<FlcGoodsSku>().Where(r => r.GoodsId == input.GoodsId && r.BarCode == input.BarCode).First();
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


}

