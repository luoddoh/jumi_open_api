using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 验货明细
/// </summary>
[SugarTable("flc_inspection_detail","验货明细")]
public class FlcInspectionDetail  : EntityBaseData
{
    /// <summary>
    /// 采购单Id
    /// </summary>
    [SugarColumn(ColumnName = "ProcureId", ColumnDescription = "采购单Id")]
    public int? ProcureId { get; set; }
    
    /// <summary>
    /// 采购明细Id
    /// </summary>
    [SugarColumn(ColumnName = "ProDetailId", ColumnDescription = "采购明细Id")]
    public int? ProDetailId { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnName = "State", ColumnDescription = "状态")]
    public int? State { get; set; }
    
    /// <summary>
    /// 条码
    /// </summary>
    [SugarColumn(ColumnName = "BarCode", ColumnDescription = "条码", Length = 200)]
    public string? BarCode { get; set; }
    
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
    
}
