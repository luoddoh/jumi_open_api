using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 退货明细
/// </summary>
[SugarTable("flc_procure_return_detail","退货明细")]
public class FlcProcureReturnDetail  : EntityBaseData
{
    /// <summary>
    /// 退货单Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "ReturnId", ColumnDescription = "退货单Id")]
    public long ReturnId { get; set; }
    
    /// <summary>
    /// sku表id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SkuId", ColumnDescription = "sku表id")]
    public long SkuId { get; set; }
    
    /// <summary>
    /// 商品Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "GoodsId", ColumnDescription = "商品Id")]
    public long GoodsId { get; set; }
    
    /// <summary>
    /// 退货价
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "ReturnPrice", ColumnDescription = "退货价", Length = 18, DecimalDigits=2 )]
    public decimal ReturnPrice { get; set; }
    
    /// <summary>
    /// 退货数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "ReturnNum", ColumnDescription = "退货数量")]
    public int ReturnNum { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "总金额", Length = 18, DecimalDigits=2 )]
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 200)]
    public string? Remark { get; set; }

    /// <summary>
    /// 单位id
    /// </summary>
    [SugarColumn(ColumnName = "UnitId", ColumnDescription = "单位id")]
    public long UnitId { get; set; }


    [Navigate(NavigateType.OneToOne, nameof(GoodsId))]//一对一导航
    public FlcGoods FlcGoods { get; set; } //不能赋值只能是null

    [Navigate(NavigateType.OneToOne, nameof(SkuId))]//一对一导航
    public FlcGoodsSku FlcGoodsSku { get; set; } //不能赋值只能是null
}
