using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;



    /// <summary>
    /// 库存查询分页查询输入参数
    /// </summary>
    public class FlcInventoryOutStatisticsInput : BasePageInput
    {
    /// <summary>
    /// 商品名称查询
    /// </summary>
    public string? goodsName { get; set; }

    /// <summary>
    /// 商品编码查询
    /// </summary>
    public string? goodsCode { get; set; }
    public string? skuName { get; set; }
    public string? barCode { get; set; }
    public long? OperatorId { get; set; }
    public List<long>? categoryId { get; set; }
    /// <summary>
    /// 单据时间范围
    /// </summary>
    public List<DateTime?> docTimeRange { get; set; }
}

