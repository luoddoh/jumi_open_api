using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 商品分类
/// </summary>
[SugarTable("flc_category","商品分类")]
public class FlcCategory  : EntityBase
{
    /// <summary>
    /// 分类名
    /// </summary>
    [SugarColumn(ColumnName = "CategoryName", ColumnDescription = "分类名", Length = 32)]
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 创建者部门Id
    /// </summary>
    [SugarColumn(ColumnName = "CreateOrgId", ColumnDescription = "创建者部门Id")]
    public long? CreateOrgId { get; set; }
    
    /// <summary>
    /// 创建者部门名称
    /// </summary>
    [SugarColumn(ColumnName = "CreateOrgName", ColumnDescription = "创建者部门名称", Length = 64)]
    public string? CreateOrgName { get; set; }
    
    /// <summary>
    /// 上级id
    /// </summary>
    [SugarColumn(ColumnName = "SuperiorId", ColumnDescription = "上级id")]
    public long? SuperiorId { get; set; }
    [SugarColumn(IsIgnore = true)]
    public List<FlcCategory> Children { get; set; }
}
