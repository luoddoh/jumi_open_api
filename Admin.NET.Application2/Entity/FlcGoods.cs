using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 商品表
/// </summary>
[SugarTable("flc_goods","商品表")]
public class FlcGoods  : EntityBaseData
{
    /// <summary>
    /// 商品名
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "GoodsName", ColumnDescription = "商品名", Length = 80)]
    public string GoodsName { get; set; }
    
    /// <summary>
    /// 商品简介
    /// </summary>
    [SugarColumn(ColumnName = "Description", ColumnDescription = "商品简介", Length = 100)]
    public string? Description { get; set; }
    
    /// <summary>
    /// 商品编码
    /// </summary>
    [SugarColumn(ColumnName = "ProductCode", ColumnDescription = "商品编码", Length = 100)]
    public string? ProductCode { get; set; }
    
    /// <summary>
    /// 所属分类ID
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CategoryId", ColumnDescription = "所属分类ID")]
    public int CategoryId { get; set; }
    
    /// <summary>
    /// 商品封面图
    /// </summary>
    [SugarColumn(ColumnName = "CoverImage", ColumnDescription = "商品封面图", Length = 2147483647)]
    public string? CoverImage { get; set; }
    
    /// <summary>
    /// 墓碑字段
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "墓碑字段")]
    public int IsDeleted { get; set; }
    
}
