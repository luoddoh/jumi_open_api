using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 盘点单
/// </summary>
[SugarTable("flc_inventory_check","盘点单")]
public class FlcInventoryCheck  : EntityBaseData
{
    /// <summary>
    /// 单据号
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "DocNumber", ColumnDescription = "单据号", Length = 45)]
    public string DocNumber { get; set; }
    
    /// <summary>
    /// 盘点人
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CheckPeople", ColumnDescription = "盘点人")]
    public long CheckPeople { get; set; }
    
    /// <summary>
    /// 盘点时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CheckTime", ColumnDescription = "盘点时间")]
    public DateTime CheckTime { get; set; }
    
    /// <summary>
    /// 审核人
    /// </summary>
    [SugarColumn(ColumnName = "Reviewer", ColumnDescription = "审核人")]
    public long? Reviewer { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>
    [SugarColumn(ColumnName = "ReviewerTime", ColumnDescription = "审核时间")]
    public DateTime? ReviewerTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "State", ColumnDescription = "状态", Length = 10)]
    public string State { get; set; }
    
}
