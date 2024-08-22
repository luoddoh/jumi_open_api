using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Admin.NET.Core;
using Nest;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
namespace Admin.NET.Application;
/// <summary>
/// 库存查询服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcInventoryOutStatisticsService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcInventoryOut> _rep;
    private readonly SqlSugarRepository<FlcGoodsSku> _sku;
    private readonly SqlSugarRepository<FlcSkuSpeValue> _rep_v;
    private readonly SqlSugarRepository<FlcCategory> _Category;
    public FlcInventoryOutStatisticsService(SqlSugarRepository<FlcInventoryOut> rep, SqlSugarRepository<FlcSkuSpeValue> rep_v, SqlSugarRepository<FlcGoodsSku> sku, SqlSugarRepository<FlcCategory> category)
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
    public async Task<SqlSugarPagedList<FlcInventoryOutStatisticsOutput>> Page(FlcInventoryOutStatisticsInput input)
    {
        var categoryData=_Category.AsQueryable().Where(u=>u.IsDelete==false).ToList();
        var query = _rep.AsQueryable()
            .WhereIF(input.OperatorId!=null,u=>u.Operator==input.OperatorId)
            .LeftJoin<FlcInventoryOutDetail>((u,d)=>u.Id==d.OutId)
            .Where((u,d)=>u.IsDelete==false&&d.IsDelete==false)
            .LeftJoin<SysUser>((u, d, user) => u.Operator== user.Id&& user.IsDelete==false)
            .GroupBy((u,d, user) =>d.SkuId)
            .Select((u, d, user) => new
            {
                d.SkuId,
                OutTotalAmount = SqlFunc.AggregateSumNoNull(d.TotalAmount),
                OperatorId = SqlFunc.AggregateMax(user.Id),
                OperatorName = SqlFunc.AggregateMax(user.RealName),
                OutNumber = SqlFunc.AggregateSumNoNull(d.OutNum),
                PlanOutNumber= SqlFunc.AggregateSumNoNull(d.PlanOutNum),
            })
            .ToList();
        var query_sku_id= query.Select(u=>u.SkuId).ToList();
        var sku_query= _sku.AsQueryable()
            .Where(u=> query_sku_id.Contains(u.Id))
            .LeftJoin<FlcGoods>((sku,goods)=>sku.GoodsId==goods.Id)
            .Where((sku,goods)=>sku.IsDelete==false&&goods.IsDelete==false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.goodsName),(sku,goods)=>goods.GoodsName.Contains(input.goodsName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.goodsCode), (sku, goods) => goods.ProductCode== input.goodsCode)
            .WhereIF(input.categoryId!=null, (sku, goods) => input.categoryId.Contains(goods.CategoryId))
             .WhereIF(!string.IsNullOrWhiteSpace(input.barCode), (sku, goods) => sku.BarCode==input.barCode)
            .Select((sku,goods)=>new FlcInventoryOutStatisticsOutput
            {
                Id=sku.Id,
                GoodsName=goods.GoodsName,
                GoodsCode=goods.ProductCode,
                GoodsImg=goods.CoverImage,
                SkuBarCode=sku.BarCode,
                Remark=sku.PrintCustom,
                categoryId=goods.CategoryId,
            })
            .ToList();
        int totalNumber = 0;
        int planTotalNumber = 0;
        decimal totalAmount = 0;
        _sku.Context.ThenMapper(sku_query, sku =>
        {
            sku.SkuName = string.Join("/", _rep_v.AsQueryable().Includes(x => x.FlcSpecificationValue)
            .Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => sku.Id, sku)
            .Select(x => x.FlcSpecificationValue.SpeValue).ToList());
            var info= query.Where(u=>u.SkuId==sku.Id).FirstOrDefault();
            if (info != null)
            {
                sku.OutTotalAmount = (decimal)info.OutTotalAmount;
                sku.OperatorId = info.OperatorId;
                sku.OperatorName = info.OperatorName;
                sku.OutNumber = (int)info.OutNumber;
                sku.PlanOutNumber= (int)info.PlanOutNumber;
            }
            totalNumber += sku.OutNumber;
            totalAmount += sku.OutTotalAmount;
            planTotalNumber += sku.PlanOutNumber;
        });
        sku_query= sku_query
            .WhereIF(!string.IsNullOrWhiteSpace(input.skuName),u=>u.SkuName.Contains(input.skuName))
            .ToList();
        int count=sku_query.Count();
        sku_query= sku_query.Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToList();
        foreach (var sku in sku_query)
        {
            sku.CategoryName = getTreeCategoryName(categoryData, sku.categoryId);
        }
        var reuslt= new FlcOutputpage<FlcInventoryOutStatisticsOutput>
        {
            Page = input.Page,
            PageSize = input.PageSize,
            Items = sku_query,
            Total = count,
            TotalAmount = totalAmount,
            TotalNumber = totalNumber,
            planTotalNumber = planTotalNumber,
        };
        return reuslt;
    }
    [HttpPost]
    [ApiDescriptionSettings(Name = "Excel")]
    public async Task<dynamic> Excel([Required] IFormFile file)
    {
        Dictionary<string, string> headList = new Dictionary<string, string>()
            {
                  {"单据号","DocNumber" },
                  {"SKU条码","BarCode" },
                  {"计划出库数量","PlanNnum" },
            };
        var data=getExcel(file, headList);
        var query = _rep.Context.Queryable<FlcInventoryOutDetail>()
            .LeftJoin<FlcInventoryOut>(( d,u) => u.Id == d.OutId)
            .LeftJoin<FlcGoodsSku>(( d, u, sku) => d.SkuId == sku.Id)
            .Select((d, u, sku) => new 
            {
                DocNumber=u.DocNumber,
                BarCode=sku.BarCode,
                detail_id=d.Id,
            }).ToList();
        var detail_list_id= query.Select(u=>u.detail_id).ToList();
        var detail_data= _rep.Context.Queryable<FlcInventoryOutDetail>().Where(u => detail_list_id.Contains(u.Id)).ToList();
        List<FlcInventoryOutDetail> update_list = new List<FlcInventoryOutDetail>();
        foreach (var item in data)
        {
            var detail_id = query.Where(u => u.DocNumber == item["DocNumber"])
                .Where(u => u.BarCode == item["BarCode"])
                .Select(u => u.detail_id)
                .FirstOrDefault();
            if (detail_id != null)
            {
                var entity = detail_data.Where(u => u.Id == detail_id).FirstOrDefault();
                if(entity != null)
                {
                    entity.PlanOutNum = Convert.ToInt32(item["PlanNnum"]);
                    update_list.Add(entity);
                }
            }
        }
        if (update_list.Count > 0)
        {
            _rep.Context.Updateable(update_list).ExecuteCommand();
        }
        return new
        {
            code = 200,
            msg = "success"
        };
    }

    public static List<Dictionary<string, string>> getExcel(IFormFile file, Dictionary<string, string> headList)
    {
        var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀
        var filePath = Path.GetTempFileName();//获取一个临时文件路径
        using (var stream = new FileStream(filePath, FileMode.Create))//创建一个局部FileStream变量
        {
            file.CopyTo(stream);//异步将IFormFile文件流放到刚刚创建的临时文件里面
        }
        FileStream fs = new FileStream(filePath, FileMode.Open);//将转换好的文件打开并返回
        IWorkbook work = null;
        if (suffix == ".xls")
        {
            work = new HSSFWorkbook(fs);
        }
        else if (suffix == ".xlsx")
        {
            work = new XSSFWorkbook(fs);
        }
        else
        {
            return null;
        }
        ISheet sheet = work.GetSheetAt(0);
        IRow TopRow = sheet.GetRow(0);
        Dictionary<string, int> keyIndex = new Dictionary<string, int>();
        foreach (var key in headList.Keys)
        {
            int index = -1;
            for (int i = 0; i < TopRow.Cells.Count; i++)
            {
                if (key == TopRow.Cells[i].StringCellValue)
                {
                    index = i; break;
                }
            }
            keyIndex.Add(headList[key], index);
        }
        List<Dictionary<string, string>> listAll = new List<Dictionary<string, string>>();
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {

            try
            {
                Dictionary<string, string> ONEROW = new Dictionary<string, string>();
                IRow row = sheet.GetRow(i);
                if (row == null || row.Cells.All(cell => cell.CellType == CellType.Blank))
                {
                    // 空行
                    continue;
                }
                if (row.Cells.Count > 0)
                {
                    ONEROW.Add("index", i.ToString());
                    foreach (string cellName in keyIndex.Keys)
                    {
                        ICell cell = row.Cells.FirstOrDefault(x => x.ColumnIndex == keyIndex[cellName]);
                        string value = "";
                        if (cell != null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.String:
                                    value = cell.StringCellValue;
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {
                                        value = cell.DateCellValue.Value.ToString("g");
                                    }
                                    else
                                    {
                                        value = cell.NumericCellValue.ToString();
                                    }
                                    break;
                                case CellType.Boolean:
                                    value = cell.BooleanCellValue.ToString();
                                    break;
                                case CellType.Formula:
                                    try
                                    {
                                        // 如果是公式，尝试计算得到值
                                        value = cell.NumericCellValue.ToString();
                                    }
                                    catch
                                    {
                                        // 如果公式计算错误，返回公式本身
                                        value = cell.CellFormula;
                                    }
                                    break;
                                default:
                                    value = cell.ToString();
                                    break;
                            }
                        }

                        ONEROW.Add(cellName, value);
                    }
                    listAll.Add(ONEROW);
                }
            }
            catch (Exception)
            {
                int index = i;
                System.IO.File.Delete(filePath);
            }

        }
        System.IO.File.Delete(filePath);
        return listAll;
    }

    public static string getTreeCategoryName(List<FlcCategory> data,long categoryId)
    {
        var query= data.Where(u=>u.Id==categoryId).FirstOrDefault();
        if (query != null)
        {
            if (query.SuperiorId > 0)
            {
                return query.CategoryName +"/"+ getTreeCategoryName(data, (long)query.SuperiorId);
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

