using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 出库单明细
/// </summary>
[SugarTable("flc_inventory_out_detail","出库单明细")]
public class FlcInventoryOutDetail  : EntityBaseData
{
    /// <summary>
    /// 出库单Id
    /// </summary>
    [SugarColumn(ColumnName = "OutId", ColumnDescription = "出库单Id")]
    public long? OutId { get; set; }
    
    /// <summary>
    /// SKU表Id
    /// </summary>
    [SugarColumn(ColumnName = "SkuId", ColumnDescription = "SKU表Id")]
    public long? SkuId { get; set; }
    
    /// <summary>
    /// 库存
    /// </summary>
    [SugarColumn(ColumnName = "InventoryNum", ColumnDescription = "库存")]
    public int? InventoryNum { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    [SugarColumn(ColumnName = "Price", ColumnDescription = "成本价", Length = 18, DecimalDigits=2 )]
    public decimal? Price { get; set; }
    
    /// <summary>
    /// 出库数量
    /// </summary>
    [SugarColumn(ColumnName = "OutNum", ColumnDescription = "出库数量")]
    public int? OutNum { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总金额", Length = 18, DecimalDigits=2 )]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }
    
}
