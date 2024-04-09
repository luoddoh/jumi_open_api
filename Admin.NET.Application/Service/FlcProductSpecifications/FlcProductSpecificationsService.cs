using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
namespace Admin.NET.Application;
/// <summary>
/// 商品规格服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcProductSpecificationsService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcProductSpecifications> _rep;
    public FlcProductSpecificationsService(SqlSugarRepository<FlcProductSpecifications> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询商品规格
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcProductSpecificationsOutput>> Page(FlcProductSpecificationsInput input)
    {
        var query = _rep.AsQueryable()
            .Where(u=>u.IsDelete==false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.SpeName.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.SpeName), u => u.SpeName.Contains(input.SpeName.Trim()))
            .Select<FlcProductSpecificationsOutput>();
        return await query.OrderBuilder(input).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加商品规格
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcProductSpecificationsInput input)
    {
        var entity = input.Adapt<FlcProductSpecifications>();
        var db = _rep.Context;

        await db.InsertNav(entity)
            .Include(z1 => z1.SpeValues).ExecuteCommandAsync();
        return entity.Id;
    }

    /// <summary>
    /// 删除商品规格
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcProductSpecificationsInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        var vlist = _rep.Context.Queryable<FlcSpecificationValue>()
            .Where(u => u.IsDelete == false && u.SpecificationId == entity.Id)
            .LeftJoin<FlcSkuSpeValue>((u,x)=>u.Id==x.SpeValueId&&x.IsDelete==false)
            .Select((u,x)=>new
            {
                Id=u.Id,
                LineId=x.SkuId,
            })
            .ToList();
        bool ok=true;
        foreach (var v in vlist)
        {
            if (v.LineId >= 0)
            {
                ok = false;
                break;
            }
        }
        if (ok)
        {
            await _rep.FakeDeleteAsync(entity);   //假删除
            //await _rep.DeleteAsync(entity);   //真删除
        }
        else
        {
            throw Oops.Oh(ErrorCodeEnum.D1007);
        }
    }

    /// <summary>
    /// 更新商品规格
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcProductSpecificationsInput input)
    {
        var entity = input.Adapt<FlcProductSpecifications>();
        var db = _rep.Context;

        await db.UpdateNav(entity)
            .Include(z1 => z1.SpeValues,new SqlSugar.UpdateNavOptions()
        {
            OneToManyInsertOrUpdate = true, 
        }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取商品规格
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcProductSpecifications> Detail([FromQuery] QueryByIdFlcProductSpecificationsInput input)
    {
        return await _rep.AsQueryable()
            .Includes(x => x.SpeValues)
            .Where(u => u.Id == input.Id).FirstAsync();
    }

    /// <summary>
    /// 获取商品规格列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcProductSpecificationsOutput>> List([FromQuery] FlcProductSpecificationsInput input)
    {
        return await _rep.AsQueryable()
            .Includes(x => x.SpeValues)
            .Where(x=>x.Enable==true&&x.IsDelete==false)
            .Select(x=>new FlcProductSpecificationsOutput
            {
                Id = x.Id,
                Enable = x.Enable,
                SpeName = x.SpeName,
                SpeValues = x.SpeValues,
            }).ToListAsync();
    }





}

