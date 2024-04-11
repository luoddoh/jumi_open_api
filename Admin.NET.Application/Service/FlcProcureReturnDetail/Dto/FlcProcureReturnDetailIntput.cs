using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;
public class FlcProcureReturnDetailIntput
{
    public long ReturnId {  get; set; }
}

public class FlcProcureReturnDetailUpdate
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// 退货单Id
    /// </summary>
    public long ReturnId { get; set; }

    /// <summary>
    /// sku表id
    /// </summary>
    public long SkuId { get; set; }

    /// <summary>
    /// 商品Id
    /// </summary>
    public long GoodsId { get; set; }

    /// <summary>
    /// 单位Id
    /// </summary>
    public long UnitId { get; set; }


    /// <summary>
    /// 退货价
    /// </summary>
    public decimal ReturnPrice { get; set; }

    /// <summary>
    /// 退货数量
    /// </summary>
    public int ReturnNum { get; set; }

    /// <summary>
    /// 总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}