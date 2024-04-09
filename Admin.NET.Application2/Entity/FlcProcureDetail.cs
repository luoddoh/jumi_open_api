using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 订货明细
/// </summary>
[SugarTable("flc_procure_detail","订货明细")]
public class FlcProcureDetail  : EntityBaseData
{
    /// <summary>
    /// 商品
    /// </summary>
    [SugarColumn(ColumnName = "GoodsId", ColumnDescription = "商品")]
    public int? GoodsId { get; set; }
    
    /// <summary>
    /// 规格
    /// </summary>
    [SugarColumn(ColumnName = "SpeValueId", ColumnDescription = "规格")]
    public int? SpeValueId { get; set; }
    
    /// <summary>
    /// 单位
    /// </summary>
    [SugarColumn(ColumnName = "UnitId", ColumnDescription = "单位")]
    public int? UnitId { get; set; }
    
    /// <summary>
    /// 采购价
    /// </summary>
    [SugarColumn(ColumnName = "PurchasePrice", ColumnDescription = "采购价", Length = 18, DecimalDigits=2 )]
    public decimal? PurchasePrice { get; set; }
    
    /// <summary>
    /// 采购数量
    /// </summary>
    [SugarColumn(ColumnName = "PurchaseNum", ColumnDescription = "采购数量")]
    public int? PurchaseNum { get; set; }
    
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
    /// 软删除
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "软删除")]
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// 库存数量
    /// </summary>
    [SugarColumn(ColumnName = "InventoryNum", ColumnDescription = "库存数量")]
    public int? InventoryNum { get; set; }
    
}
