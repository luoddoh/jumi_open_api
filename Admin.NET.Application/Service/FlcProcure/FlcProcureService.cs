﻿using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using AngleSharp.Dom;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Collections.Generic;
using NetTaste;
using Nest;
namespace Admin.NET.Application;
/// <summary>
/// 采购订货列表服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FlcProcureService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FlcProcure> _rep;
    public FlcProcureService(SqlSugarRepository<FlcProcure> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询采购订货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FlcProcureOutput>> Page(FlcProcureInput input)
    {
       
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
            )
                .Where(u => u.IsDelete == false)
                .WhereIF(input.sku_id!=null,u=>u.ProcureDetail.Any(z=>z.SkuId==input.sku_id))
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(input.SupplierId>0, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.State>0, u => u.State == input.State)
            .WhereIF(input.Purchaser>0, u => u.Purchaser == input.Purchaser)
            .WhereIF(!string.IsNullOrEmpty(input.Qtype), u => u.State != 100)
            .WhereIF((input.uid > 0&& input.uid!= 1300000000101&&input.uid!= 1300000000111&& (input.Isinventory==null|| input.Isinventory==false)), u => (u.Purchaser == input.uid||u.CreateUserId== input.uid))
            .WhereIF((input.Isinventory==true),u=>u.SupplierId==input.userSupplierId && u.State!=100)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<FlcSupplierInfo>((u, supplierid) => u.SupplierId == supplierid.Id )
            .LeftJoin<SysUser>((u, supplierid, purchaser) => u.Purchaser == purchaser.Id )
            .LeftJoin<SysUser>((u, supplierid, purchaser, reviewer) => u.Reviewer == reviewer.Id )
            .LeftJoin<SysOrg>((u, supplierid, purchaser, reviewer,org) => purchaser.OrgId == org.Id)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select((u, supplierid, purchaser, reviewer,org) => new FlcProcureOutput
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                SupplierId = u.SupplierId, 
                SupplierIdSupName = supplierid.SupName,
                State = u.State,
                AuditTime = u.AuditTime,
                ProcurementTime = u.ProcurementTime,
                Remark = u.Remark,
                TotalAmount = u.TotalAmount,
                Purchaser = u.Purchaser, 
                department= org.Name,
                CreateUserName = u.CreateUserName,
                PurchaserRealName = purchaser.RealName,
                Reviewer = u.Reviewer, 
                ReviewerRealName = reviewer.RealName,
                SupConfirm=u.SupConfirm
            });
        if(input.AuditTimeRange != null && input.AuditTimeRange.Count >0)
        {
            DateTime? start= input.AuditTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.AuditTime > start);
            if (input.AuditTimeRange.Count >1 && input.AuditTimeRange[1].HasValue)
            {
                var end = input.AuditTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.AuditTime < end);
            }
        } 
        if(input.ProcurementTimeRange != null && input.ProcurementTimeRange.Count >0)
        {
            DateTime? start= input.ProcurementTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.ProcurementTime > start);
            if (input.ProcurementTimeRange.Count >1 && input.ProcurementTimeRange[1].HasValue)
            {
                var end = input.ProcurementTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ProcurementTime < end);
            }
        }
        var list = query.ToList();
        int totlenum = 0;
        decimal totleamont = 0;
        foreach (var item in list)
        {

            totleamont += item.TotalAmount==null?0:(decimal)item.TotalAmount;
        }
        var a = query.ToPagedList(input.Page, input.PageSize);
        FlcOutputpage<FlcProcureOutput> result = new FlcOutputpage<FlcProcureOutput>();
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
    public async Task<SqlSugarPagedList<ProcureDetail>> DetailList(FlcProcureInput input)
    {
        var query = _rep.AsQueryable()
            .Where(u => u.IsDelete == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.DocNumber), u => u.DocNumber.Contains(input.DocNumber.Trim()))
            .WhereIF(input.SupplierId > 0, u => u.SupplierId == input.SupplierId)
            .WhereIF(input.State > 0, u => u.State == input.State)
            .WhereIF(input.Purchaser > 0, u => u.Purchaser == input.Purchaser)
            .WhereIF(!string.IsNullOrEmpty(input.Qtype), u => u.State != 100)
            .WhereIF((input.uid > 0 && input.uid != 1300000000101 && input.uid != 1300000000111 && (input.Isinventory == null || input.Isinventory == false)), u => (u.Purchaser == input.uid || u.CreateUserId == input.uid))
            .WhereIF((input.Isinventory == true), u => u.SupplierId == input.userSupplierId && u.State != 100)
            .LeftJoin<FlcProcureDetail>((u, d) => u.Id == d.ProcureId && d.IsDelete == false)
            .LeftJoin<FlcGoodsSku>((u, d, s) => d.SkuId == s.Id && s.IsDelete == false)
            .LeftJoin<FlcGoodsUnit>((u, d, s, t) => s.UnitId == t.Id && t.IsDelete == false)
            .LeftJoin<FlcGoods>((u, d, s, t, g) => s.GoodsId == g.Id && g.IsDelete == false)
            .LeftJoin<FlcSupplierInfo>((u, d, s, t, g, i) => u.SupplierId == i.Id && i.IsDelete == false)
            .LeftJoin<SysUser>((u, d, s, t, g, i, purchaser) => u.Purchaser == purchaser.Id)
            .LeftJoin<SysUser>((u, d, s, t, g, i, purchaser, reviewer) => u.Reviewer == reviewer.Id)
            .WhereIF(!string.IsNullOrWhiteSpace(input.goodsName), (u, d, s, t, g, purchaser, reviewer) => g.GoodsName.Contains(input.goodsName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.goodsCode), (u, d, s, t, g, purchaser, reviewer) => g.ProductCode.Contains(input.goodsCode))
            .WhereIF(!string.IsNullOrWhiteSpace(input.barCode), (u, d, s, t, g, purchaser, reviewer) => s.BarCode == input.barCode)
            .WhereIF(input.OperatorId > 0, (u, d, s, t, g, purchaser, reviewer) => purchaser.Id == input.OperatorId)
            .WhereIF(input.categoryId != null, (u, d, s, t, g, purchaser, reviewer) => input.categoryId.Contains(g.CategoryId))
            .Select((u, d, s, t, g, i, purchaser, reviewer) => new ProcureDetail
            {
                DocNumber = u.DocNumber,
                SupplierIdSupName = i.SupName,
                State = u.State,
                AuditTime = u.AuditTime,
                ProcurementTime = u.ProcurementTime,
                Remark = u.Remark,
                PurchaserRealName = purchaser.RealName,
                ReviewerRealName = reviewer.RealName,
                SkuId = s.Id,
                GoodsName = g.GoodsName,
                UnitName = t.UnitName,
                purchasePrice = d.purchasePrice,
                okNum = d.okNum,
                totalAmount = d.purchasePrice*d.okNum,
                DetailRemark = d.remark
            });
        if (input.AuditTimeRange != null && input.AuditTimeRange.Count > 0)
        {
            DateTime? start = input.AuditTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.AuditTime > start);
            if (input.AuditTimeRange.Count > 1 && input.AuditTimeRange[1].HasValue)
            {
                var end = input.AuditTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.AuditTime < end);
            }
        }
        if (input.ProcurementTimeRange != null && input.ProcurementTimeRange.Count > 0)
        {
            DateTime? start = input.ProcurementTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ProcurementTime > start);
            if (input.ProcurementTimeRange.Count > 1 && input.ProcurementTimeRange[1].HasValue)
            {
                var end = input.ProcurementTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ProcurementTime < end);
            }
        }
        var list=query.ToList();
        _rep.Context.ThenMapper(list, item =>
        {
            var info = _rep.Context.Queryable<FlcSkuSpeValue>().Includes(x => x.FlcSpecificationValue).Where(x => x.IsDelete == false)
            .SetContext(x => x.SkuId, () => item.SkuId, item)
            .Select(x => x.FlcSpecificationValue.SpeValue).ToList();
            item.speValueList = string.Join("/", info);
        });
        list = list.WhereIF(!string.IsNullOrWhiteSpace(input.skuName), u => u.speValueList.Contains(input.skuName)).ToList();
        return list.ToPagedList(input.Page, input.PageSize);
    }

    /// <summary>
    /// 小程序分页查询采购订货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "MiniPage")]
    public async Task<SqlSugarPagedList<FlcProcureOutputMini>> MiniPage(FlcProcureInput input)
    {

        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.DocNumber.Contains(input.SearchKey.Trim())
                || u.Remark.Contains(input.SearchKey.Trim())
            )
                .Where(u => u.IsDelete == false)
            .Where(u => u.State >100)
            .WhereIF(input.State > 0, u => u.State == input.State)
             .WhereIF(input.uid > 0, u => u.Purchaser == input.uid)
            //处理外键和TreeSelector相关字段的连接
            .LeftJoin<FlcSupplierInfo>((u, supplierid) => u.SupplierId == supplierid.Id)
            .LeftJoin<SysUser>((u, supplierid, purchaser) => u.Purchaser == purchaser.Id)
            .LeftJoin<SysUser>((u, supplierid, purchaser, reviewer) => u.Reviewer == reviewer.Id)
            .LeftJoin<SysOrg>((u, supplierid, purchaser, reviewer, org) => purchaser.OrgId == org.Id)
            .OrderBy(u => u.CreateTime,OrderByType.Desc)
            .Select((u, supplierid, purchaser, reviewer, org) => new FlcProcureOutputMini
            {
                Id = u.Id,
                DocNumber = u.DocNumber,
                State = u.State,
                ProcurementTime = u.ProcurementTime,
                TotalAmount = u.TotalAmount,
                Purchaser = u.Purchaser,
                PurchaserRealName = purchaser.RealName,
                CreateTime = u.CreateTime,
                CreateUserName = u.CreateUserName,
                supplier= supplierid.SupName,
                Remark=u.Remark,
            });
        if (input.ProcurementTimeRange != null && input.ProcurementTimeRange.Count > 0)
        {
            DateTime? start = input.ProcurementTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ProcurementTime > start);
            if (input.ProcurementTimeRange.Count > 1 && input.ProcurementTimeRange[1].HasValue)
            {
                var end = input.ProcurementTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ProcurementTime < end);
            }
        }
        var list=query.ToList();
        _rep.Context.ThenMapper(list, obj =>
        {
             var deta = _rep.Context.Queryable<FlcProcureDetail>().Where(x => x.IsDelete == false)
            .SetContext(x => x.ProcureId, () => obj.Id, obj)
            .Select(x => new
            {
                number = x.purchaseNum
            }).ToList();
            int num = 0;
            foreach ( var x in deta )
            {
                num += x.number;
            }
            obj.TotalNumber = num;
        });
        return list.ToPagedList(input.Page, input.PageSize);
    }
    
    /// <summary>
    /// 增加采购订货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<long> Add(AddFlcProcureInput input)
    {
        var entity = input.Adapt<FlcProcure>();
        entity.DocNumber="CG"+DateTime.Now.Year+ DateTime.Now.Month+ DateTime.Now.Day+ new Random().Next(1000,9999).ToString();
        entity.State = 100;
        await _rep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 删除采购订货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFlcProcureInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新采购订货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFlcProcureInput input)
    {
        var entity = input.Adapt<FlcProcure>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }
    /// <summary>
    /// 更新采购订货列表(供应商确认)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "UpdateSup")]
    public async Task UpdateSup(UpdateFlcProcureInput input)
    {
        var entity = input.Adapt<FlcProcure>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }
    /// <summary>
    /// 获取采购订货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<FlcProcure> Detail([FromQuery] QueryByIdFlcProcureInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }
    /// <summary>
    /// 获取采购订货列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "MiniDetail")]
    public async Task<dynamic> MiniDetail([FromQuery] QueryByIdFlcProcureInput input)
    {
        var quer= _rep.AsQueryable()
            .LeftJoin<SysUser>((x,p)=>x.Purchaser==p.Id)
            .LeftJoin<SysUser>((x,p,r) => x.Reviewer == r.Id)
            .LeftJoin<FlcSupplierInfo>((x, p, r, s) => x.SupplierId == s.Id)
            .Where(x=>x.Id==input.Id)
            .Select((x,p,r,s)=>new
            {
                docNumber=x.DocNumber,
                state=x.State,
                createUserName=x.CreateUserName,
                createTime=x.CreateTime,
                purchaserUserName=p.RealName,
                procurementTime=x.ProcurementTime,
                reviewer=r.RealName,
                auditTime=x.AuditTime,
                remark=x.Remark,
                supplier= s.SupName,
            })
            .FirstAsync();
        return await quer;
    }
    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Examine")]
    public async Task Examine( ExamineInput input)
    {
        var row= _rep.AsQueryable().Where(x=>x.Id == input.Id).First();
        row.State=input.state;
        if (input.Reviewer != null)
        {
            row.Reviewer=input.Reviewer;
            row.AuditTime=DateTime.Now;
        }
        if (input.state == 100)
        {
            row.Reviewer = null;
            row.AuditTime=null;
        }
        await _rep.AsUpdateable(row).ExecuteCommandAsync(); ;
    }

    /// <summary>
    /// 获取采购订货列表列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FlcProcureOutput>> List([FromQuery] FlcProcureInput input)
    {
        return await _rep.AsQueryable().Select<FlcProcureOutput>().ToListAsync();
    }
    
    /// <summary>
    /// 获取供应商列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "FlcSupplierInfoSupplierIdDropdown"), HttpGet]
    public async Task<dynamic> FlcSupplierInfoSupplierIdDropdown()
    {
        return await _rep.Context.Queryable<FlcSupplierInfo>()
            .Where(u=>u.IsDelete==false)
                .Select(u => new
                {
                    Label = u.SupName,
                    Value = u.Id,
                    code=u.code,
                }
                ).ToListAsync();
    }
    /// <summary>
    /// 获取采购员列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SysUserPurchaserDropdown"), HttpGet]
    public async Task<dynamic> SysUserPurchaserDropdown()
    {
        return await _rep.Context.Queryable<SysUser>()
            .Where (u=>u.IsDelete==false)
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
        return await _rep.Context.Queryable<SysUser>().Where(u => u.IsDelete == false)
                .Select(u => new
                {
                    Label = u.RealName,
                    Value = u.Id
                }
                ).ToListAsync();
    }




}

