using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 采购订货列表
/// </summary>
[SugarTable("flc_goods_procure","采购订货列表")]
public class FlcGoodsProcure  : EntityBaseData
{
    /// <summary>
    /// 单据号
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "DocNumber", ColumnDescription = "单据号", Length = 45)]
    public string DocNumber { get; set; }
    
    /// <summary>
    /// 供应商
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SupplierId", ColumnDescription = "供应商")]
    public int SupplierId { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "State", ColumnDescription = "状态")]
    public int State { get; set; }
    
    /// <summary>
    /// 采购员
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Purchaser", ColumnDescription = "采购员", Length = 45)]
    public string Purchaser { get; set; }
    
    /// <summary>
    /// 审核人
    /// </summary>
    [SugarColumn(ColumnName = "Reviewer", ColumnDescription = "审核人", Length = 45)]
    public string? Reviewer { get; set; }
    
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
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnName = "CreatedTime", ColumnDescription = "创建时间")]
    public DateTime? CreatedTime { get; set; }
    
    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnName = "UpdatedTime", ColumnDescription = "更新时间")]
    public DateTime? UpdatedTime { get; set; }
    
    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnName = "CreatedUserId", ColumnDescription = "创建者Id")]
    public long? CreatedUserId { get; set; }
    
    /// <summary>
    /// 创建者名称
    /// </summary>
    [SugarColumn(ColumnName = "CreatedUserName", ColumnDescription = "创建者名称", Length = 20)]
    public string? CreatedUserName { get; set; }
    
    /// <summary>
    /// 修改者Id
    /// </summary>
    [SugarColumn(ColumnName = "UpdatedUserId", ColumnDescription = "修改者Id")]
    public long? UpdatedUserId { get; set; }
    
    /// <summary>
    /// 修改者名称
    /// </summary>
    [SugarColumn(ColumnName = "UpdatedUserName", ColumnDescription = "修改者名称", Length = 20)]
    public string? UpdatedUserName { get; set; }
    
    /// <summary>
    /// 软删除
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "软删除")]
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 总价格
    /// </summary>
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总价格", Length = 18, DecimalDigits=2 )]
    public decimal? TotalAmount { get; set; }
    
}
