using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NewLife.Reflection;
namespace Admin.NET.Application;
/// <summary>
/// 采购退货列表服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcProcureReturnService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcProcureReturn> _rep;
    public FlcProcureReturnService(SqlSugarRepository<FlcProcureReturn> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcProcureReturnOutput>> Page(FlcProcureReturnInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
            )
                .Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(input.SupplierId>0, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.Returner>0, u => u.Returner == input.Returner)
            .WhereIF((input.uid > 0 && input.uid != 1300000000101 && input.uid != 1300000000111 && (input.Isinventory == null || input.Isinventory == false)), u => (u.Returner == input.uid||u.CreateUserId== input.uid))
            .WhereIF((input.Isinventory == true), u => u.SupplierId == input.userSupplierId)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<SysUser>((u, returner) => u.Returner == returner.Id )
            .LeftJoin<SysUser>((u, returner, reviewer) => u.Reviewer == reviewer.Id )
            .LeftJoin<FlcSupplierInfo>((u, returner, reviewer, suppl) => u.SupplierId == suppl.Id)
             .LeftJoin<SysOrg>((u, returner, reviewer, suppl,org) => returner.OrgId == org.Id)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select((u, returner, reviewer, suppl,org) => new FlcProcureReturnOutput()
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                SupplierId = u.SupplierId,
                SupplierIdSuplName=suppl.SupName,
                Returner = u.Returner, 
                ReturnerRealName = returner.RealName,
                department=org.Name,
                ReturnTime = u.ReturnTime,
                Reviewer = u.Reviewer, 
                ReviewerRealName = reviewer.RealName,
                AuditTime = u.AuditTime,
                TotalAmount = u.TotalAmount,
                Remark = u.Remark,
                State=u.State
            });
        if(input.ReturnTimeRange != null && input.ReturnTimeRange.Count >0)
        {
            DateTime? start= input.ReturnTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.ReturnTime > start);
            if (input.ReturnTimeRange.Count >1 && input.ReturnTimeRange[1].HasValue)
            {
                var end = input.ReturnTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReturnTime < end);
            }
        }
        var list = query.ToList();
        int totlenum = 0;
        decimal totleamont = 0;
        foreach (var item in list)
        {

            totleamont += (decimal)item.TotalAmount;
        }
        var a = query.ToPagedList(input.Page, input.PageSize);
        FlcOutputpage<FlcProcureReturnOutput> result = new FlcOutputpage<FlcProcureReturnOutput>();
        result.Page = a.Page;
        result.PageSize = a.PageSize;
        result.Items = a.Items;
        result.Total = a.Total;
        result.TotalPages = a.TotalPages;
        result.HasPrevPage = a.HasPrevPage;
        result.HasNextPage = a.HasNextPage;
        result.TotalNumber = totlenum;
        result.TotalAmount = totleamont;
        return result;
    }
    /// <summary>
    /// 导出详情查询数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "DetailList")]
    public async Task<SqlSugarPagedList<ReturnDetail>> DetailList(FlcProcureReturnInput input)
    {
        var query = _rep.AsQueryable()
            .Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(input.SupplierId > 0, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.Returner > 0, u => u.Returner == input.Returner)
            .WhereIF((input.uid > 0 && input.uid != 1300000000101 && input.uid != 1300000000111 && (input.Isinventory == null || input.Isinventory == false)), u => (u.Returner == input.uid || u.CreateUserId == input.uid))
            .WhereIF((input.Isinventory == true), u => u.SupplierId == input.userSupplierId)
            .LeftJoin<FlcProcureReturnDetail>((u, d) => u.Id == d.ReturnId && d.IsDelete == false)
            .LeftJoin<FlcGoodsSku>((u, d, s) => d.SkuId == s.Id && s.IsDelete == false)
            .LeftJoin<FlcGoodsUnit>((u, d, s, t) => s.UnitId == t.Id && t.IsDelete == false)
            .LeftJoin<FlcGoods>((u, d, s, t, g) => s.GoodsId == g.Id && g.IsDelete == false)
            .LeftJoin<FlcSupplierInfo>((u, d, s, t, g, i) => u.SupplierId == i.Id && i.IsDelete == false)
            .LeftJoin<SysUser>((u, d, s, t, g, i, returner) => u.Returner == returner.Id)
            .LeftJoin<SysUser>((u, d, s, t, g, i, returner, reviewer) => u.Reviewer == reviewer.Id)
            .Select((u, d, s, t, g, i, returner, reviewer) => new ReturnDetail
            {
                DocNumber = u.DocNumber,
                SupplierIdSuplName = i.SupName,
                State = u.State,
                AuditTime = u.AuditTime,
                ReturnTime = u.ReturnTime,
                Remark = u.Remark,
                ReturnerRealName = returner.RealName,
                ReviewerRealName = reviewer.RealName,
                SkuId = s.Id,
                GoodsName = g.GoodsName,
                UnitName = t.UnitName,
                ReturnPrice = d.ReturnPrice,
                ReturnNum = d.ReturnNum,
                totalAmount = d.TotalAmount,
                DetailRemark = d.Remark
            });
        if (input.ReturnTimeRange != null && input.ReturnTimeRange.Count > 0)
        {
            DateTime? start = input.ReturnTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReturnTime > start);
            if (input.ReturnTimeRange.Count > 1 && input.ReturnTimeRange[1].HasValue)
            {
                var end = input.ReturnTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReturnTime < end);
            }
        }
        var list = query.ToList();
        _rep.Context.ThenMapper(list, item =>
        {
            var info = _rep.Context.Queryable<FlcSkuSpeValue>().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => item.SkuId, item)
            .Select(x => new labval
            {
                Id = x.SpeValueId,
                SpecificationId = x.FlcSpecificationValue.SpecificationId,
                SpeValue = x.FlcSpecificationValue.SpeValue
            }).ToList();
            string value = "";
            foreach (var ele in info)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = ele.SpeValue;
                }
                else
                {
                    value += "/" + ele.SpeValue;
                }
            }
            item.speValueList = value;
        });
        return list.ToPagedList(input.Page, input.PageSize);
    }
    /// <summary>
    /// 增加采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcProcureReturnInput input)
    {
        var entity = input.Adapt<FlcProcureReturn>();
        entity.DocNumber = "TD" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + new Random().Next(1000, 9999).ToString();
        entity.State = 100;
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 删除采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcProcureReturnInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcProcureReturnInput input)
    {
        var entity = input.Adapt<FlcProcureReturn>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }
    [HttpPost]
    [ApiDescriptionSettings(Name = "Excel")]
    public async Task<dynamic> Excel([Required] IFormFile file)
    {
        var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀
        var filePath = Path.GetTempFileName();//获取一个临时文件路径
        using (var stream = new FileStream(filePath, FileMode.Create))//创建一个局部FileStream变量
        {
            await file.CopyToAsync(stream);//异步将IFormFile文件流放到刚刚创建的临时文件里面
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
            return new { message = "仅支持.xls/.xlsx文件格式" };
        }
        ISheet sheet = work.GetSheetAt(0);
        IRow TopRow = sheet.GetRow(0);

        Dictionary<string, string> headList = new Dictionary<string, string>()
        {
              {"SKU编码","SkuCode" },
              {"数量","Number" },
              {"备注","Remake" },
        };
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
        List<string> barcodelist = new List<string>();
        List<Dictionary<string, string>> listAll = new List<Dictionary<string, string>>();
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {

            try
            {
                Dictionary<string, string> ONEROW = new Dictionary<string, string>();
                IRow row = sheet.GetRow(i);
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
            catch (Exception)
            {
                int index = i;
                File.Delete(filePath);
                throw Oops.Oh(ErrorCodeEnum.D1009);
            }

        }

        File.Delete(filePath);
        return Datainit(listAll);
    }
    public List<excelOut> Datainit(List<Dictionary<string, string>> table)
    {
        var list = _rep.Context.Queryable<FlcGoodsSku>()
           .Includes(x => x.flcGoods)
           .Includes(x => x.flcGoodsUnit)
           .Where(x => x.IsDelete == false && x.flcGoods.IsDelete == false)
           .LeftJoin<FlcInventory>((x, p) => x.Id == p.SkuId)
            .Where(x => x.IsDelete == false)
            .Select((x, p) => new excelOut
            {
                Id = x.Id,
                BarCode = x.BarCode,
                returnPrice = x.CostPrice,
                CoverImage = x.CoverImage,
                GoodsId = x.GoodsId,
                UnitId = x.UnitId,
                GoodsName = x.flcGoods.GoodsName,
                UnitName = x.flcGoodsUnit.UnitName,
                InventoryNum = p.Number,
            })
           .ToList();
        _rep.Context.ThenMapper(list, sku =>
        {
            sku.speValueList = _rep.Context.Queryable<FlcSkuSpeValue>().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => sku.Id, sku)
            .Select(x => new labval
            {
                Id = x.SpeValueId,
                SpecificationId = x.FlcSpecificationValue.SpecificationId,
                SpeValue = x.FlcSpecificationValue.SpeValue
            }).ToList();
        });
        List<excelOut> result = new List<excelOut>();
        foreach (var item in table)
        {
            var sku = item["SkuCode"];
            if (result.Count == 0)
            {
                var obj=list.Where(u => u.speValueList.FindIndex(x => x.SpeValue == sku) != -1).First();
                obj.returnNum =Convert.ToInt32(item["Number"]);
                obj.remark = item["Remake"];
                result.Add(obj);
            }
            else
            {
                bool add_ok = false;
                int index = 0;
                for (int i = 0; i < result.Count; i++)
                {
                    var ele = result[i];
                    if (ele.speValueList.FindIndex(x => x.SpeValue == sku) != -1)
                    {
                        add_ok = true;
                        index = i;
                        break;
                    }
                }
                if(add_ok)
                {
                    result[index].returnNum += Convert.ToInt32(item["Number"]);
                    if (!string.IsNullOrWhiteSpace(item["Remake"]))
                    {
                        result[index].remark += (","+ item["Remake"]);
                    }
                }
                else
                {
                    var obj = list.Where(u => u.speValueList.FindIndex(x => x.SpeValue == sku) != -1).First();
                    obj.returnNum = Convert.ToInt32(item["Number"]);
                    obj.remark = item["Remake"];
                    result.Add(obj);
                }
            }
           
        }
        return result;
    }
    [HttpPost]
    [ApiDescriptionSettings(Name = "Examine")]
    public async Task Examine(ExamineInput input)
    {
        var row = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        row.State = input.state;
        if (input.Reviewer != null)
        {
            row.Reviewer = input.Reviewer;
            row.AuditTime = DateTime.Now;
        }
        if (input.state == 100)
        {
            row.Reviewer = null;
            row.AuditTime = null;
        }
        await _rep.AsUpdateable(row).ExecuteCommandAsync(); ;
    }
    [HttpGet]
    [ApiDescriptionSettings(Name = "Confirm")]
    public async Task Confirm([FromQuery] ConfirmInput input)
    {
        var list = _rep.Context.Queryable<FlcProcureReturnDetail>()
            .Where(x=>x.ReturnId==input.Id&&x.IsDelete==false)
            .ToList();
        foreach (var item in list)
        {
            var row = _rep.Context.Queryable<FlcInventory>()
                .Where(x => x.SkuId == item.SkuId).First();
            row.Number=row.Number-item.ReturnNum;
            await _rep.Context.Updateable(row).ExecuteCommandAsync();
        }
        var detail = _rep.AsQueryable().Where(x => x.Id == input.Id).First();
        detail.State = 300;
        await _rep.AsUpdateable(detail).ExecuteCommandAsync(); ;
    }

    /// <summary>
    /// 获取采购退货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcProcureReturn> Detail([FromQuery] QueryByIdFlcProcureReturnInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取采购退货列表列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcProcureReturnOutput>> List([FromQuery] FlcProcureReturnInput input)
    {
        return await _rep.AsQueryable().Select<FlcProcureReturnOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取退货员列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SysUserReturnerDropdown"), HttpGet]
    public async Task<dynamic> SysUserReturnerDropdown()
    {
        return await _rep.Context.Queryable<SysUser>()
                .Select(u => new
                {
                    Label = u.RealName,
                    Value = u.Id
                }
                ).ToListAsync();
    }
    /// <summary>
    /// 获取审核人列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SysUserReviewerDropdown"), HttpGet]
    public async Task<dynamic> SysUserReviewerDropdown()
    {
        return await _rep.Context.Queryable<SysUser>()
                .Select(u => new
                {
                    Label = u.RealName,
                    Value = u.Id
                }
                ).ToListAsync();
    }




}

