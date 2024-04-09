using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 退货明细
/// </summary>
[SugarTable("flc_return_detail","退货明细")]
public class FlcReturnDetail  : EntityBaseData
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
    /// 退货数量
    /// </summary>
    [SugarColumn(ColumnName = "ReturnNum", ColumnDescription = "退货数量")]
    public int? ReturnNum { get; set; }
    
    /// <summary>
    /// 退货价
    /// </summary>
    [SugarColumn(ColumnName = "ReturnPrice", ColumnDescription = "退货价", Length = 18, DecimalDigits=2 )]
    public decimal? ReturnPrice { get; set; }
    
    /// <summary>
    /// 总价格
    /// </summary>
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总价格", Length = 18, DecimalDigits=2 )]
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
    
}
