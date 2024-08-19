// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.FlcProcureReturnDetail.Dto;
public class FlcProcureReturnDetailOutput
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
    /// 库存量
    /// </summary>
    public int InventoryNum {  get; set; }

    /// <summary>
    /// 商品Id
    /// </summary>
    public long GoodsId { get; set; }

    /// <summary>
    /// 商品名
    /// </summary>
    public string GoodsName { get; set; }

    /// <summary>
    /// 单位Id
    /// </summary>
    public long? UnitId { get; set; }

    public string? SkuImage { get; set; }
    /// <summary>
    /// 单位名
    /// </summary>
    public string? UnitName { get; set; }
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

    public List<labval> speValueList { get; set;}
}
