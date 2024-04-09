using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 商品sku表
/// </summary>
[SugarTable("flc_goods_sku","商品sku表")]
public class FlcGoodsSku  : EntityBaseData
{
    /// <summary>
    /// 商品Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "GoodsId", ColumnDescription = "商品Id")]
    public int GoodsId { get; set; }
    
    /// <summary>
    /// 规格值Id
    /// </summary>
    [SugarColumn(ColumnName = "SpeValueId", ColumnDescription = "规格值Id")]
    public int? SpeValueId { get; set; }
    
    /// <summary>
    /// 条码
    /// </summary>
    [SugarColumn(ColumnName = "BarCode", ColumnDescription = "条码", Length = 45)]
    public string? BarCode { get; set; }
    
    /// <summary>
    /// sku封面
    /// </summary>
    [SugarColumn(ColumnName = "CoverImage", ColumnDescription = "sku封面", Length = 2147483647)]
    public string? CoverImage { get; set; }
    
    /// <summary>
    /// 单位Id
    /// </summary>
    [SugarColumn(ColumnName = "UnitId", ColumnDescription = "单位Id")]
    public int? UnitId { get; set; }
    
    /// <summary>
    /// 数量
    /// </summary>
    [SugarColumn(ColumnName = "Number", ColumnDescription = "数量")]
    public int? Number { get; set; }
    
    /// <summary>
    /// 软删除
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "软删除")]
    public bool IsDeleted { get; set; }
    
}
