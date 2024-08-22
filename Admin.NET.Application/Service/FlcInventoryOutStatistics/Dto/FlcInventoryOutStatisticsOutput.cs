using Admin.NET.Application.Entity;
using Admin.NET.Core;

namespace Admin.NET.Application;

/// <summary>
/// 库存查询输出参数
/// </summary>
public class FlcInventoryOutStatisticsOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    public long OperatorId { get; set; }
    public string OperatorName { get; set; }

    /// <summary>
    /// 商品名
    /// </summary>
    public string GoodsName { get; set; }
    /// <summary>
    /// 商品图片
    /// </summary>
    public string GoodsImg { get; set; }
    /// <summary>
    ///商品编码
    /// </summary>
    public string GoodsCode { get; set; }
    /// <summary>
    /// 出库数量
    /// </summary>
    public int OutNumber { get; set; }
    /// <summary>
    /// 计划出库数量
    /// </summary>
    public int PlanOutNumber { get; set; }
    /// <summary>
    /// 出库金额
    /// </summary>
    public decimal OutTotalAmount { get; set; }

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

public class outmodel
{
    public string BarCode { get; set; }
    public string DocNumber { get; set; }

    public FlcInventoryOutDetail Detail { get; set; }
}


