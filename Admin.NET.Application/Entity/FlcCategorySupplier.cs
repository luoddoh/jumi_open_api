using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 供应商分类
/// </summary>
[SugarTable("flc_category_supplier","供应商分类")]
public class FlcCategorySupplier  : EntityBaseData
{
    /// <summary>
    /// 分类名
    /// </summary>
    [SugarColumn(ColumnName = "CategoryName", ColumnDescription = "分类名", Length = 32)]
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 上级Id
    /// </summary>
    [SugarColumn(ColumnName = "SuperiorId", ColumnDescription = "上级Id")]
    public long? SuperiorId { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<FlcCategorySupplier> Children { get; set; }
}
