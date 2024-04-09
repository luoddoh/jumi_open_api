using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 盘点单列表
/// </summary>
[SugarTable("flc_check","盘点单列表")]
public class FlcCheck  : EntityBaseData
{
    /// <summary>
    /// 单据号
    /// </summary>
    [SugarColumn(ColumnName = "DocNumber", ColumnDescription = "单据号")]
    public int? DocNumber { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnName = "State", ColumnDescription = "状态")]
    public int? State { get; set; }
    
    /// <summary>
    /// 盘点人
    /// </summary>
    [SugarColumn(ColumnName = "CheckPerson", ColumnDescription = "盘点人", Length = 45)]
    public string? CheckPerson { get; set; }
    
    /// <summary>
    /// 盘点时间
    /// </summary>
    [SugarColumn(ColumnName = "CheckTime", ColumnDescription = "盘点时间")]
    public DateTime? CheckTime { get; set; }
    
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
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnName = "CreatedTime", ColumnDescription = "创建时间")]
    public DateTime? CreatedTime { get; set; }
    
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
    /// 软删除
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "软删除")]
    public bool IsDeleted { get; set; }
    
}
