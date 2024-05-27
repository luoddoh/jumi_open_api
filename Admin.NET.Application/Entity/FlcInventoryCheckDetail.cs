using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 盘点明细
/// </summary>
[SugarTable("flc_inventory_check_detail","盘点明细")]
public class FlcInventoryCheckDetail  : EntityBaseData
{
    /// <summary>
    /// 盘点单Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CheckId", ColumnDescription = "盘点单Id")]
    public long CheckId { get; set; }
    
    /// <summary>
    /// Sku表Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SkuId", ColumnDescription = "Sku表Id")]
    public long SkuId { get; set; }
    
    /// <summary>
    /// 库存量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "InventoryNum", ColumnDescription = "库存量")]
    public int InventoryNum { get; set; }
    
    /// <summary>
    /// 盘点数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CheckNum", ColumnDescription = "盘点数量")]
    public int CheckNum { get; set; }

    /// <summary>
    /// 总金额
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总金额", Length = 18, DecimalDigits=2 )]
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// 差异数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "DifferenceNum", ColumnDescription = "差异数量")]
    public int DifferenceNum { get; set; }
    
    /// <summary>
    /// 差异金额
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "DifferencePrice", ColumnDescription = "差异金额", Length = 18, DecimalDigits=2 )]
    public decimal DifferencePrice { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "Price", ColumnDescription = "成本价", Length = 18, DecimalDigits=2 )]
    public decimal Price { get; set; }

    /// <summary>
    /// 已扫条码
    /// </summary>
    [SugarColumn(ColumnName = "OkCodeList", ColumnDescription = "已扫条码", Length = 2147483647)]
    public string? OkCodeList { get; set; }
}
