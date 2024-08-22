using Admin.NET.Core;

namespace Admin.NET.Application;

/// <summary>
/// 库存查询输出参数
/// </summary>
public class FlcProcureStatisticsOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    public long supplierId { get; set; }
    public string supplierName { get; set; }

    /// <summary>
    /// 商品名
    /// </summary>
    public string GoodsName { get; set; }
    /// <summary>
    /// 商品图片
    /// </summary>
    public string GoodsImg { get; set; }
    /// <summary>
    /// 单位名
    /// </summary>
    public string GoodsCode { get; set; }
    /// <summary>
    /// 采购数量
    /// </summary>
    public int ProcureNumber { get; set; }
    /// <summary>
    /// 采购价格
    /// </summary>
    public decimal ProcurePrice { get; set; }
    /// <summary>
    /// 采购金额
    /// </summary>
    public decimal ProcureTotalAmount { get; set; }

    /// <summary>
    /// sku名称
    /// </summary>
    public string SkuName { get; set; }

    public long categoryId { get; set; }

    public string CategoryName { get; set; }
    /// <summary>
    /// sku条码
    /// </summary>
    public string SkuBarCode { get; set; }
    public string Remark { get; set; }
}




