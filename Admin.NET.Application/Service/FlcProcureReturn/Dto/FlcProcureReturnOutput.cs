namespace Admin.NET.Application;

/// <summary>
/// 采购退货列表输出参数
/// </summary>
public class FlcProcureReturnOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 单据号
    /// </summary>
    public string? DocNumber { get; set; }
    
    /// <summary>
    /// 供应商
    /// </summary>
    public long? SupplierId { get; set; }
    /// <summary>
    /// 供应商 描述
    /// </summary>
    public string SupplierIdSuplName { get; set; }

    /// <summary>
    ///状态
    /// </summary>
    public int? State {  get; set; }

    /// <summary>
    /// 退货员
    /// </summary>
    public long? Returner { get; set; }

    /// <summary>
    /// 退货员部门
    /// </summary>
    public string department { get; set; }

    /// <summary>
    /// 退货员 描述
    /// </summary>
    public string ReturnerRealName { get; set; } 
    
    /// <summary>
    /// 退货时间
    /// </summary>
    public DateTime? ReturnTime { get; set; }
    
    /// <summary>
    /// 审核人
    /// </summary>
    public long? Reviewer { get; set; } 
    
    /// <summary>
    /// 审核人 描述
    /// </summary>
    public string ReviewerRealName { get; set; } 
    
    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? AuditTime { get; set; }
    
    /// <summary>
    /// 总价格
    /// </summary>
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    
    }
public class ReturnDetail: FlcProcureReturnOutput
{
    /// <summary>
    /// 商品sku
    /// </summary>
    public long SkuId { get; set; }
    /// <summary>
    /// 商品名称
    /// </summary>
    public string GoodsName { get; set; }
    /// <summary>
    /// 规格
    /// </summary>
    public string? speValueList { get; set; }
    /// <summary>
    /// 单位名称
    /// </summary>
    public string UnitName { get; set; }
    /// <summary>
    /// 退货价
    /// </summary>
    public decimal ReturnPrice { get; set; }

    /// <summary>
    /// 退货数量
    /// </summary>
    public int ReturnNum { get; set; }
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal totalAmount { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? DetailRemark { get; set; }
}

public class excelOut: FlcGoodsSkuOutputs
{
    public int returnNum { get; set; }
    public int totalAmount { get; set; }
    public string remark { get; set; }
    public decimal? returnPrice { get; set; }
}



