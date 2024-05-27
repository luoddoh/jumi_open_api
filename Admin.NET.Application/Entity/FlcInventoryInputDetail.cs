using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 入库单明细
/// </summary>
[SugarTable("flc_inventory_input_detail","入库单明细")]
public class FlcInventoryInputDetail  : EntityBaseData
{
    /// <summary>
    /// 入库单Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "InputId", ColumnDescription = "入库单Id")]
    public long InputId { get; set; }
    
    /// <summary>
    /// SKU表Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SkuId", ColumnDescription = "SKU表Id")]
    public long SkuId { get; set; }
    
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
    /// 入库数量
    /// </summary>
    [SugarColumn(ColumnName = "InputNum", ColumnDescription = "入库数量")]
    public int? InputNum { get; set; }
    
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

    /// <summary>
    /// 已扫条码
    /// </summary>
    [SugarColumn(ColumnName = "OkCodeList", ColumnDescription = "已扫条码", Length = 2147483647)]
    public string? OkCodeList { get; set; }
}
