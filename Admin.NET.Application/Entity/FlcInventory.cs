using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 库存
/// </summary>
[SugarTable("flc_inventory","库存")]
public class FlcInventory  : EntityBaseData
{
    /// <summary>
    /// sku表id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SkuId", ColumnDescription = "sku表id")]
    public long SkuId { get; set; }
    
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
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总价格", Length = 32, DecimalDigits=0 )]
    public decimal TotalAmount { get; set; }
    
}
