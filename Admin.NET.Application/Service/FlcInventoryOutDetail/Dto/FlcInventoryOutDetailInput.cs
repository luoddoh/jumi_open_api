
namespace Admin.NET.Application;
public class FlcInventoryOutDetailInput
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// 出库单Id
    /// </summary>
    public long? OutId { get; set; }

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
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

public class FlcInventoryOutDetailInputList
{
    /// <summary>
    /// 出库单Id
    /// </summary>
    public long? OutId { get; set; }

}
