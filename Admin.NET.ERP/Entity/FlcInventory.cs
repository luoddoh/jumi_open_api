using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 库存
/// </summary>
[SugarTable("flc_inventory","库存")]
public class FlcInventory  : EntityBaseData
{
    /// <summary>
    /// 分类
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CategoryId", ColumnDescription = "分类")]
    public int CategoryId { get; set; }
    
    /// <summary>
    /// 商品
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "GoodsId", ColumnDescription = "商品")]
    public int GoodsId { get; set; }
    
    /// <summary>
    /// 规格
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "ProSpeId", ColumnDescription = "规格")]
    public int ProSpeId { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "UnitId", ColumnDescription = "单位")]
    public int UnitId { get; set; }
    
    /// <summary>
    /// 数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Number", ColumnDescription = "数量")]
    public int Number { get; set; }
    
    /// <summary>
    /// 总价格
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总价格", Length = 18, DecimalDigits=2 )]
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// 墓碑字段
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "墓碑字段")]
    public bool IsDeleted { get; set; }
    
}
