
namespace Admin.NET.Application;
public class FlcInventoryCheckDetailInput
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// 盘点单Id
    /// </summary>
    public long? CheckId { get; set; }

    /// <summary>
    /// 库存
    /// </summary>
    public int? InventoryNum { get; set; }

    /// <summary>
    /// 成本价
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 盘点数量
    /// </summary>
    public int? CheckNum { get; set; }

    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 差异数量
    /// </summary>
    public int? DifferenceNum { get; set; }

    /// <summary>
    /// 差异金额
    /// </summary>
    public decimal? DifferencePrice { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

public class FlcInventoryCheckDetailInputList
{
    /// <summary>
    /// 盘点单Id
    /// </summary>
    public long? CheckId { get; set; }

}
