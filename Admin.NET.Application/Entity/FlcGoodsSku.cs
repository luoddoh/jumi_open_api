using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

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
    public long GoodsId { get; set; }
    
    /// <summary>
    /// 条码
    /// </summary>
    [Required]
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
    [Required]
    [SugarColumn(ColumnName = "UnitId", ColumnDescription = "单位Id")]
    public long UnitId { get; set; }
    
    /// <summary>
    /// 数量
    /// </summary>
    [SugarColumn(ColumnName = "Number", ColumnDescription = "数量")]
    public int? Number { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CostPrice", ColumnDescription = "成本价", Length = 18, DecimalDigits=2 )]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 销售价
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SalesPrice", ColumnDescription = "销售价", Length = 18, DecimalDigits=2 )]
    public decimal? SalesPrice { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(FlcSkuSpeValue.SkuId))]
    public List<FlcSkuSpeValue> SpeValueList { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(GoodsId))]//一对一 商品导航
    public FlcGoods flcGoods { get; set; } //不能赋值只能是null

    [Navigate(NavigateType.OneToOne, nameof(UnitId))]//一对一 单位导航
    public FlcGoodsUnit flcGoodsUnit { get; set; } //不能赋值只能是null
}
