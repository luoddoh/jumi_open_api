using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Admin.NET.Core;
using NewLife.Http;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaServiceMarketServiceGetServiceBuyerListResponse.Types.Buyer.Types;
using Nest;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaBusinessPerformanceBootResponse.Types.Data.Types.Body.Types.Table.Types;
using NetTaste;
namespace Admin.NET.Application;
/// <summary>
/// 商品信息服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcGoodsService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcGoods> _rep;
    public FlcGoodsService(SqlSugarRepository<FlcGoods> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询商品信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcGoodsOutput>> Page(FlcGoodsInput input)
    {
        var query = _rep.AsQueryable()
            .Where(u=>u.IsDelete==false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.GoodsName.Contains(input.SearchKey.Trim())
                || u.ProductCode.Contains(input.SearchKey.Trim())
                || u.Description.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsName), u => u.GoodsName.Contains(input.GoodsName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ProductCode), u => u.ProductCode.Contains(input.ProductCode.Trim()))
            .WhereIF(input.CategoryId>0, u => u.CategoryId == input.CategoryId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description.Contains(input.Description.Trim()))
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<FlcCategory>((u, categoryid) => u.CategoryId == categoryid.Id )
            .OrderBy(u => u.CreateTime)
            .Select((u, categoryid) => new FlcGoodsOutput
            {
                Id = u.Id,
                GoodsName = u.GoodsName,
                ProductCode = u.ProductCode,
                CategoryId = u.CategoryId,  
                CategoryIdCategoryName = categoryid.CategoryName,
                CoverImage = u.CoverImage,
                Description = u.Description,
            });
        List<FlcGoodsOutput> list = query.ToList();
        _rep.Context.ThenMapper(list,item =>
        {
            item.SkuList= _rep.Context.Queryable<FlcGoodsSku>()
            .LeftJoin<FlcSkuSpeValue>((u, line) => u.Id == line.SkuId && line.IsDelete == false)
            .LeftJoin<FlcSpecificationValue>((u, line, value) => line.SpeValueId == value.Id && value.IsDelete == false)
            .Where(u => u.IsDelete == false)
            .OrderBy(u => u.CreateTime)
            .Select((u, line, value) =>new skulabel
            {
                GoodsId=u.GoodsId,
                label = value.SpeValue
            })
            .SetContext(u => u.GoodsId, () => item.Id, item)
            .ToList();
        });
        if (!string.IsNullOrWhiteSpace(input.Sku))
        {
            list=list.Where(u=>u.SkuList.FindIndex(s =>s.label.Contains(input.Sku))!=-1).ToList();
        }
        return list.ToPagedList(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加商品信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcGoodsInput input)
    {
        var entity = input.Adapt<FlcGoods>();
        var list = _rep.AsQueryable().Where(u => u.IsDelete == false && u.GoodsName == entity.GoodsName).First();
        if(list == null)
        {
            await _rep.InsertAsync(entity);
            return entity.Id;
        }
        else
        {
            throw Oops.Oh(ErrorCodeEnum.D1006);
        }
    }

    /// <summary>
    /// 删除商品信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcGoodsInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新商品信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcGoodsInput input)
    {
        var entity = input.Adapt<FlcGoods>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取商品信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcGoods> Detail([FromQuery] QueryByIdFlcGoodsInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取商品信息列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcGoodsOutput>> List([FromQuery] FlcGoodsInput input)
    {
        return await _rep.AsQueryable().Select<FlcGoodsOutput>().ToListAsync();
    }




    [HttpGet]
    [ApiDescriptionSettings(Name = "FlcCategoryTree")]
    public async Task<dynamic> FlcCategoryTree()
    {
        return await _rep.Context.Queryable<FlcCategory>().ToTreeAsync(u => u.Children, u => u.SuperiorId, 0);
    }


    [HttpPost]
    [ApiDescriptionSettings(Name = "Excel")]
    public async Task<dynamic> Excel([Required] IFormFile file )
    {
        var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀
        var filePath = Path.GetTempFileName();//获取一个临时文件路径
        using (var stream = new FileStream(filePath, FileMode.Create))//创建一个局部FileStream变量
        {
            await file.CopyToAsync(stream);//异步将IFormFile文件流放到刚刚创建的临时文件里面
        }
        FileStream fs= new FileStream(filePath, FileMode.Open);//将转换好的文件打开并返回
        IWorkbook work = null;
        if (suffix == ".xls")
        {
            work=new HSSFWorkbook(fs);
        }
        else if(suffix ==".xlsx")
        {
            work = new XSSFWorkbook(fs);
        }
        else
        {
            return new { message="仅支持.xls/.xlsx文件格式"};
        }
        ISheet sheet = work.GetSheetAt(0);
        IRow TopRow = sheet.GetRow(0);

        Dictionary<string, string> headList = new Dictionary<string, string>()
        {
              {"一级分类","oneClass" },
              {"二级分类","TwoClass" },
              {"三级分类","ThreeClass" },
              {"条码","BarCard" },
              {"商品全名","GoodsName" },
              {"SKU编号","SkuCode" },
              {"重量(kg)","weight" },
              {"产地","Producer" },
              {"零售价","RetailPrice" },
              {"参考成本价","costPrice" },
              {"基本单位","Unit" },
              {"说明","PrintCustom" },
        };
        Dictionary<string, int> keyIndex = new Dictionary<string, int>();
        foreach (var key in headList.Keys)
        {
            int index = -1;
            for (int i = 0; i < TopRow.Cells.Count; i++)
            {
                if(key== TopRow.Cells[i].StringCellValue)
                {
                    index=i; break;
                }
            }
            keyIndex.Add(headList[key],index);
        } 
        List<string> barcodelist = new List<string>();
        List<SpeListValue> spe = new List<SpeListValue>();
        List<Dictionary<string, string>> listAll = new List<Dictionary<string, string>>();
        for (int i = 1;i <= sheet.LastRowNum; i++)
        {

            try
            {
                Dictionary<string, string> ONEROW= new Dictionary<string, string>();
                IRow row = sheet.GetRow(i);
                if(row.Cells.Count > 5)
                {
                    ONEROW.Add("index", (i+1).ToString());
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
                        if (cellName == "BarCard")
                        {
                            if (barcodelist.Count == 0)
                            {
                                barcodelist.Add(value);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    int index = barcodelist.IndexOf(value);
                                    if (index == -1)
                                    {
                                        barcodelist.Add(value);
                                    }
                                    else
                                    {
                                        throw Oops.Oh(ErrorCodeEnum.D1009);
                                    }
                                }
                            }
                        }

                        ONEROW.Add(cellName, value);
                    }
                    listAll.Add(ONEROW);
                    string name = row.Cells.First(x => x.ColumnIndex == keyIndex["GoodsName"]).StringCellValue;
                    string sku = row.Cells.First(x => x.ColumnIndex == keyIndex["SkuCode"]).StringCellValue;
                    if (!string.IsNullOrEmpty(name))
                    {
                        bool add_ok = true;
                        int index = 0;
                        for (int j = 0; j < spe.Count; j++)
                        {
                            if (spe[j].SpeName == name)
                            {
                                add_ok = false;
                                index = j;
                                break;
                            }
                        }
                        if (add_ok)
                        {
                            spe.Add(new SpeListValue()
                            {
                                SpeName = name,
                                Values = new List<string>() { sku }
                            });
                        }
                        else
                        {
                            spe[index].Values.Add(sku);
                        }

                    }
                }
            }
            catch (Exception )
            {
                int index = i;
                File.Delete(filePath);
                throw Oops.Oh(ErrorCodeEnum.D1009);
            }
            
        }

        File.Delete(filePath);
        return new
        {
            tabelDate= listAll,
            speVal= spe
        };
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "DateSave")]
    public async Task DateSave(SaveInput inputs)
    {
        if (inputs.speVals != null)
        {
            foreach (var spe in inputs.speVals)
            {
                initspeVal(spe);
            }
        }
        foreach (var input in inputs.table)
        {
            long CategoryId = initCategory(input);
            long GoodsId = initGoods(input, CategoryId);
            long UnitId = initUnit(input);
            long skuid= initSku(input, GoodsId, UnitId);
            long inventoryId = initInventory(skuid);
        }
    }
    public long initCategory(UploadFileGoodsInput input)
    {
        var frist_class = _rep.Context.Queryable<FlcCategory>().Where(x => x.CategoryName == input.oneClass && x.IsDelete == false).First();
        if (frist_class != null)
        {
            var two_class = _rep.Context.Queryable<FlcCategory>().Where(x => x.CategoryName == input.TwoClass && x.IsDelete == false).First();
            if (two_class != null)
            {
                var three_class = _rep.Context.Queryable<FlcCategory>().Where(x => x.CategoryName == input.ThreeClass && x.IsDelete == false).First();
                if (three_class == null)
                {
                    FlcCategory obj= new FlcCategory()
                    {
                        CategoryName= input.ThreeClass,
                        SuperiorId= two_class.Id,
                    };
                    long id_three= _rep.Context.Insertable<FlcCategory>(obj).ExecuteReturnSnowflakeId();
                    return id_three;
                }
                else
                {
                    return three_class.Id;
                }
            }
            else
            {
                FlcCategory obj_two = new FlcCategory()
                {
                    CategoryName = input.TwoClass,
                    SuperiorId = frist_class.Id,
                };
                long id_two = _rep.Context.Insertable(obj_two).ExecuteReturnSnowflakeId();
                FlcCategory obj_three = new FlcCategory()
                {
                    CategoryName = input.ThreeClass,
                    SuperiorId = id_two,
                };
                long id_three = _rep.Context.Insertable<FlcCategory>(obj_three).ExecuteReturnSnowflakeId();
                return id_three;
            }
        }
        else
        {
            FlcCategory obj_one = new FlcCategory()
            {
                CategoryName = input.oneClass,
                SuperiorId = 0,
            };
            long id_one = _rep.Context.Insertable(obj_one).ExecuteReturnSnowflakeId();
            FlcCategory obj_two = new FlcCategory()
            {
                CategoryName = input.TwoClass,
                SuperiorId = id_one,
            };
            long id_two = _rep.Context.Insertable(obj_two).ExecuteReturnSnowflakeId();
            FlcCategory obj_three = new FlcCategory()
            {
                CategoryName = input.ThreeClass,
                SuperiorId = id_two,
            };
            long id_three = _rep.Context.Insertable<FlcCategory>(obj_three).ExecuteReturnSnowflakeId();
            return id_three;
        }
    }
    public long initGoods(UploadFileGoodsInput input,long CategoryId)
    {
        var row= _rep.AsQueryable().Where(x=>x.GoodsName==input.GoodsName && x.IsDelete == false).First();
        if (row == null)
        {
            FlcGoods flcGoods = new FlcGoods()
            {
                GoodsName = input.GoodsName,
                CategoryId= CategoryId,
                Weight=input.Weight,
                Producer=input.Producer,
            };
            return _rep.Context.Insertable(flcGoods).ExecuteReturnSnowflakeId();
        }
        else
        {
            return row.Id;
        }
    }

    public long initUnit(UploadFileGoodsInput input)
    {
        var row = _rep.Context.Queryable<FlcGoodsUnit>().Where(x=>x.UnitName==input.Unit && x.IsDelete == false).First();
        if(row == null)
        {
            FlcGoodsUnit flcGoodsUnit = new FlcGoodsUnit()
            {
                UnitName = input.Unit,
            };
            return _rep.Context.Insertable(flcGoodsUnit).ExecuteReturnSnowflakeId();
        }
        else
        {
            return row.Id;
        }
    }
    public void initspeVal(speVal spe)
    {
        var row= _rep.Context.Queryable<FlcProductSpecifications>().Where(x=>x.SpeName==spe.speName&&x.IsDelete==false).First();
        if(row == null) {
            FlcProductSpecifications flcProductSpecifications = new FlcProductSpecifications()
            {
                SpeName = spe.speName,
                Enable = true,
            };
            long id=_rep.Context.Insertable(flcProductSpecifications).ExecuteReturnSnowflakeId();
            foreach (string value in spe.values)
            {
                FlcSpecificationValue flcSpecificationValue = new FlcSpecificationValue()
                {
                    SpecificationId=id,
                    SpeValue=value
                };
                _rep.Context.Insertable(flcSpecificationValue).ExecuteCommand();
            }
        }
        else
        {
            foreach (string value in spe.values)
            {
                var sv = _rep.Context.Queryable<FlcSpecificationValue>().Where(x => x.SpeValue == value && x.IsDelete == false&&x.SpecificationId== row.Id).First();
                if(sv == null)
                {
                    FlcSpecificationValue flcSpecificationValue = new FlcSpecificationValue()
                    {
                        SpecificationId = row.Id,
                        SpeValue = value
                    };
                    _rep.Context.Insertable(flcSpecificationValue).ExecuteCommand();
                }
            }
                
        }
    }

    public long initSku(UploadFileGoodsInput input, long GoodsId, long UnitId)
    {
        var spev=_rep.Context.Queryable<FlcSpecificationValue>()
            .LeftJoin<FlcProductSpecifications>((v,s)=>v.SpecificationId==s.Id&&s.IsDelete==false)
            .Where((v,s)=> s.SpeName == input.GoodsName&& input.SkuCode.Contains(v.SpeValue)&&v.SpeValue==input.SkuCode).First();
        var skuspe= _rep.Context.Queryable<FlcSkuSpeValue>()
            .LeftJoin<FlcGoodsSku>((x,k)=>x.SkuId==k.Id)
            .LeftJoin<FlcGoods>((x,k,g)=>k.GoodsId==g.Id)
            .Where((x, k, g) => x.IsDelete==false&&x.SpeValueId == spev.Id&&g.IsDelete==false ).First();
        if(skuspe == null)
        {
            FlcGoodsSku sku = new FlcGoodsSku()
            {
                GoodsId = GoodsId,
                UnitId = UnitId,
                CostPrice = string.IsNullOrEmpty(input.costPrice) ? null : Convert.ToDecimal(input.costPrice),
                SalesPrice = string.IsNullOrEmpty(input.RetailPrice) ? null : Convert.ToDecimal(input.RetailPrice),
                PrintCustom= input.PrintCustom,
                BarCode = input.BarCard
            };
            long id = _rep.Context.Insertable(sku).ExecuteReturnSnowflakeId();
            FlcSkuSpeValue flcSkuSpeValue = new FlcSkuSpeValue()
            {
                SkuId = id,
                SpeValueId = spev.Id,
            };
            _rep.Context.Insertable(flcSkuSpeValue).ExecuteCommand();
            return id;
        }
        else
        {
            return (long)skuspe.SkuId;
        }
       
    }

    public long initInventory(long skuId)
    {
        var row=_rep.Context.Queryable<FlcInventory>()
            .Where(x=>x.IsDelete==false&&x.SkuId==skuId).First();
        if (row == null)
        {
            FlcInventory flcInventory = new FlcInventory()
            {
                SkuId=skuId,
                Number=0,
                TotalAmount=0,
            };
            return _rep.Context.Insertable(flcInventory).ExecuteReturnSnowflakeId();
        }
        else
        {
            return row.Id;
        }
    }
}


