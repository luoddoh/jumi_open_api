using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 采购明细
/// </summary>
[SugarTable("flc_procure_detail","采购明细")]
public class FlcProcureDetail  : EntityBaseData
{
    /// <summary>
    /// 采购订单Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "ProcureId", ColumnDescription = "采购订单Id")]
    public long ProcureId { get; set; }
    
    /// <summary>
    /// 商品Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "GoodsId", ColumnDescription = "商品Id")]
    public long GoodsId { get; set; }
    
    /// <summary>
    /// 商品sku
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SkuId", ColumnDescription = "商品sku")]
    public long SkuId { get; set; }
    
    /// <summary>
    /// 库存量
    /// </summary>
    [SugarColumn(ColumnName = "InventoryNum", ColumnDescription = "库存量")]
    public int? InventoryNum { get; set; }
    
    /// <summary>
    /// 采购价
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "purchasePrice", ColumnDescription = "采购价", Length = 18, DecimalDigits=2 )]
    public decimal purchasePrice { get; set; }
    
    /// <summary>
    /// 采购数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "purchaseNum", ColumnDescription = "采购数量")]
    public int purchaseNum { get; set; }
    
    /// <summary>
    /// 未到数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "okNum", ColumnDescription = "已到数量")]
    public int okNum { get; set; }
    
    /// <summary>
    /// 已到数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "noNum", ColumnDescription = "未到数量")]
    public int noNum { get; set; }
    
    /// <summary>
    /// 总金额
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "totalAmount", ColumnDescription = "总金额", Length = 18, DecimalDigits=2 )]
    public decimal totalAmount { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", Length = 200)]
    public string? remark { get; set; }

    /// <summary>
    /// 打印次数
    /// </summary>
    [SugarColumn(ColumnName = "PrintNum", ColumnDescription = "打印次数", Length = 200)]
    public int? PrintNum { get; set; }

    /// <summary>
    /// 条码列表
    /// </summary>
    [SugarColumn(ColumnName = "BarCodeList", ColumnDescription = "条码列表",  Length = 2147483647)]
    public string? BarCodeList { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SkuId))]//一对一 Sku导航
    public FlcGoodsSku flcGoodsSku { get; set; } //不能赋值只能是null

    [Navigate(NavigateType.OneToOne, nameof(GoodsId))]//一对一 商品导航
    public FlcGoods flcGoods { get; set; } //不能赋值只能是null
}
