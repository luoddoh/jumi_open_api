using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 商品单位
/// </summary>
[SugarTable("flc_goods_unit","商品单位")]
public class FlcGoodsUnit  : EntityBaseData
{
    /// <summary>
    /// 单位名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "UnitName", ColumnDescription = "单位名称", Length = 20)]
    public string UnitName { get; set; }
    
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
