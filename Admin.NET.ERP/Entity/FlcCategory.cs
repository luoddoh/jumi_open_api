using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 商品分类
/// </summary>
[SugarTable("flc_category","商品分类")]
public class FlcCategory  : EntityBaseData
{
    /// <summary>
    /// 分类名
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CategoryName", ColumnDescription = "分类名", Length = 80)]
    public string CategoryName { get; set; }
    
    /// <summary>
    /// 上级分类Id
    /// </summary>
    [SugarColumn(ColumnName = "SuperiorId", ColumnDescription = "上级分类Id")]
    public int? SuperiorId { get; set; }
    
    /// <summary>
    /// 墓碑字段
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "墓碑字段")]
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// 所属类别等级
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Level", ColumnDescription = "所属类别等级")]
    public int Level { get; set; }
    
}
