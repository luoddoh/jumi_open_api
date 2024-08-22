using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 出库单
/// </summary>
[SugarTable("flc_inventory_out","出库单")]
public class FlcInventoryOut  : EntityBaseData
{
    /// <summary>
    /// 单据号
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "DocNumber", ColumnDescription = "单据号", Length = 45)]
    public string DocNumber { get; set; }
    
    /// <summary>
    /// 出库类型
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "OutType", ColumnDescription = "出库类型", Length = 10)]
    public string? OutType { get; set; }

    /// <summary>
    /// 单据状态
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "State", ColumnDescription = "单据状态", Length = 10)]
    public string? State { get; set; }
    /// <summary>
    /// 出库时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "OutTime", ColumnDescription = "出库时间")]
    public DateTime OutTime { get; set; }
    
    /// <summary>
    /// 订单备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "订单备注", Length = 200)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 审核人Id
    /// </summary>
    [SugarColumn(ColumnName = "Reviewer", ColumnDescription = "审核人Id")]
    public long? Reviewer { get; set; }
    
    /// <summary>
    /// 审核时间
    /// </summary>
    [SugarColumn(ColumnName = "ReviewerTime", ColumnDescription = "审核时间")]
    public DateTime? ReviewerTime { get; set; }

    /// <summary>
    /// 操作员Id
    /// </summary>
    [SugarColumn(ColumnName = "Operator", ColumnDescription = "操作员")]
    public long? Operator { get; set; }

    //用例1：正常一对多
    [Navigate(NavigateType.OneToMany, nameof(FlcInventoryOutDetail.OutId))]
    public List<FlcInventoryOutDetail> InventoryOutDetail { get; set; }
}
