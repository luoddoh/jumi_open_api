using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 采购退货列表
/// </summary>
[SugarTable("flc_procure_return","采购退货列表")]
public class FlcProcureReturn  : EntityBaseData
{
    /// <summary>
    /// 单据号
    /// </summary>
    [SugarColumn(ColumnName = "DocNumber", ColumnDescription = "单据号", Length = 32)]
    public string? DocNumber { get; set; }
    
    /// <summary>
    /// 供应商
    /// </summary>
    [SugarColumn(ColumnName = "SupplierId", ColumnDescription = "供应商")]
    public long? SupplierId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnName = "State", ColumnDescription = "状态")]
    public int? State { get; set; }

    /// <summary>
    /// 退货员
    /// </summary>
    [SugarColumn(ColumnName = "Returner", ColumnDescription = "退货员")]
    public long? Returner { get; set; }
    
    /// <summary>
    /// 退货时间
    /// </summary>
    [SugarColumn(ColumnName = "ReturnTime", ColumnDescription = "退货时间")]
    public DateTime? ReturnTime { get; set; }
    
    /// <summary>
    /// 审核人
    /// </summary>
    [SugarColumn(ColumnName = "Reviewer", ColumnDescription = "审核人")]
    public long? Reviewer { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>
    [SugarColumn(ColumnName = "AuditTime", ColumnDescription = "审核时间")]
    public DateTime? AuditTime { get; set; }
    
    /// <summary>
    /// 总价格
    /// </summary>
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总价格", Length = 18, DecimalDigits=2 )]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }


  
}
