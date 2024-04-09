using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

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
    [SugarColumn(ColumnName = "GoodsName", ColumnDescription = "商品名", Length = 45)]
    public string GoodsName { get; set; }
    
    /// <summary>
    /// 商品简介
    /// </summary>
    [SugarColumn(ColumnName = "Description", ColumnDescription = "商品简介", Length = 200)]
    public string? Description { get; set; }
    
    /// <summary>
    /// 商品编码
    /// </summary>
    [SugarColumn(ColumnName = "ProductCode", ColumnDescription = "商品编码", Length = 100)]
    public string? ProductCode { get; set; }
    
    /// <summary>
    /// 所属分类
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CategoryId", ColumnDescription = "所属分类")]
    public long CategoryId { get; set; }
    /// <summary>
    /// 重量(kg)
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Weight", ColumnDescription = "重量(kg)")]
    public int? Weight { get; set; }


    /// <summary>
    /// 产地
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Producer", ColumnDescription = "产地")]
    public string? Producer { get; set; }    /// <summary>
                                            /// 商品封面图
                                            /// </summary>
    [SugarColumn(ColumnName = "CoverImage", ColumnDescription = "商品封面图", Length = 2147483647)]
    public string? CoverImage { get; set; }
    
}
