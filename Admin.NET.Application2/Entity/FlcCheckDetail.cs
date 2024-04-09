using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 盘点明细
/// </summary>
[SugarTable("flc_check_detail","盘点明细")]
public class FlcCheckDetail  : EntityBaseData
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
    /// 库存数量
    /// </summary>
    [SugarColumn(ColumnName = "InventoryNum", ColumnDescription = "库存数量")]
    public int? InventoryNum { get; set; }
    
    /// <summary>
    /// 盘点数量
    /// </summary>
    [SugarColumn(ColumnName = "CheckNum", ColumnDescription = "盘点数量")]
    public int? CheckNum { get; set; }
    
    /// <summary>
    /// 成本价
    /// </summary>
    [SugarColumn(ColumnName = "CostPrice", ColumnDescription = "成本价", Length = 18, DecimalDigits=4 )]
    public decimal? CostPrice { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总金额", Length = 18, DecimalDigits=4 )]
    public decimal? TotalAmount { get; set; }
    
    /// <summary>
    /// 差异数量
    /// </summary>
    [SugarColumn(ColumnName = "DifferenceNum", ColumnDescription = "差异数量")]
    public int? DifferenceNum { get; set; }
    
    /// <summary>
    /// 差异金额
    /// </summary>
    [SugarColumn(ColumnName = "DifferencePrice", ColumnDescription = "差异金额", Length = 18, DecimalDigits=4 )]
    public decimal? DifferencePrice { get; set; }
    
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
    
}
