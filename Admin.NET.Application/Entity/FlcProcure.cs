using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 采购订货列表
/// </summary>
[SugarTable("flc_procure","采购订货列表")]
public class FlcProcure  : EntityBaseData
{
    /// <summary>
    /// 单据号
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "DocNumber", ColumnDescription = "单据号", Length = 32)]
    public string DocNumber { get; set; }
    
    /// <summary>
    /// 供应商Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SupplierId", ColumnDescription = "供应商Id")]
    public long SupplierId { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "State", ColumnDescription = "状态")]
    public int State { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>
    [SugarColumn(ColumnName = "AuditTime", ColumnDescription = "审核时间")]
    public DateTime? AuditTime { get; set; }
    
    /// <summary>
    /// 采购时间
    /// </summary>
    [SugarColumn(ColumnName = "ProcurementTime", ColumnDescription = "采购时间")]
    public DateTime? ProcurementTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 32)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 总价格
    /// </summary>
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总价格", Length = 18, DecimalDigits=2 )]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 采购员
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Purchaser", ColumnDescription = "采购员")]
    public long Purchaser { get; set; }
    
    /// <summary>
    /// 审核人
    /// </summary>
    [SugarColumn(ColumnName = "Reviewer", ColumnDescription = "审核人")]
    public long? Reviewer { get; set; }

    /// <summary>
    /// 供应商确认状态
    /// </summary>
    [SugarColumn(ColumnName = "SupConfirm", ColumnDescription = "供应商确认状态")]
    public bool? SupConfirm { get; set; }

}
