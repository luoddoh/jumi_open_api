
namespace Admin.NET.Application;
public class FlcInventoryInputDetailOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 入库单Id
    /// </summary>
    public long? InputId { get; set; }

    /// <summary>
    /// SKU表Id
    /// </summary>
    public long? SkuId { get; set; }
    /// <summary>
    /// 商品名
    /// </summary>
    public string? GoodsName { get; set; }
    /// <summary>
    /// 单位名
    /// </summary>
    public string? UnitName { get; set; }
    /// <summary>
    /// 库存
    /// </summary>
    public int? InventoryNum { get; set; }

    /// <summary>
    /// 成本价
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 出库数量
    /// </summary>
    public int? InputNum { get; set; }

    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// sku图片
    /// </summary>
    public string SkuImage { get; set; }
    public List<labval> speValueList { get; set; }
}
