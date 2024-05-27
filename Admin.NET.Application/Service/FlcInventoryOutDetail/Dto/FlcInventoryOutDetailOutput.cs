
namespace Admin.NET.Application;
public class FlcInventoryOutDetailOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 出库单Id
    /// </summary>
    public long? OutId { get; set; }

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
    public int? OutNum { get; set; }

    /// <summary>
    /// 本次扫码条码
    /// </summary>
    public string? oneCodeList { get; set; }
    /// <summary>
    /// 已扫条码
    /// </summary>
    public string? OkCodeList { get; set; }
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    public int OneOutNum { get; set; }
    /// <summary>
    /// sku图片
    /// </summary>
    public string SkuImage { get; set; }
    public List<labval> speValueList { get; set;}
}
