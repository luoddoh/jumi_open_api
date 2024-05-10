using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 入库单
/// </summary>
[SugarTable("flc_inventory_input_list","入库单")]
public class FlcInventoryInputList  : EntityBaseData
{
    /// <summary>
    /// 单据号
    /// </summary>
    [SugarColumn(ColumnName = "DocNumber", ColumnDescription = "单据号", Length = 45)]
    public string? DocNumber { get; set; }
    
    /// <summary>
    /// 入库时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "InputTime", ColumnDescription = "入库时间")]
    public DateTime InputTime { get; set; }
    
    /// <summary>
    /// 单据状态
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "State", ColumnDescription = "单据状态", Length = 10)]
    public string State { get; set; }
    
    /// <summary>
    /// 操作员
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Operator", ColumnDescription = "操作员")]
    public long Operator { get; set; }
    
    /// <summary>
    /// 入库类型
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "InputType", ColumnDescription = "入库类型", Length = 10)]
    public string InputType { get; set; }
    
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
    /// 订单备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "订单备注", Length = 200)]
    public string? Remark { get; set; }
    
}
