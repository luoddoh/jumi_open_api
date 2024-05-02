namespace Admin.NET.Application;

/// <summary>
/// 盘点单输出参数
/// </summary>
public class FlcInventoryCheckOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 单据号
    /// </summary>
    public string DocNumber { get; set; }
    
    /// <summary>
    /// 状态
    /// </summary>
    public string State { get; set; }
    
    /// <summary>
    /// 盘点人
    /// </summary>
    public long CheckPeople { get; set; } 
    
    /// <summary>
    /// 盘点人 描述
    /// </summary>
    public string CheckPeopleRealName { get; set; } 
    
    /// <summary>
    /// 盘点时间
    /// </summary>
    public DateTime CheckTime { get; set; }
    
    /// <summary>
    /// 审核人
    /// </summary>
    public long? Reviewer { get; set; } 
    
    /// <summary>
    /// 审核人 描述
    /// </summary>
    public string ReviewerRealName { get; set; } 
    
    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? ReviewerTime { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreateUserName { get; set; }
    /// <summary>
    /// 差异数量
    /// </summary>
    public int? DifferenceNum { get; set; }

    /// <summary>
    /// 差异金额
    /// </summary>
    public decimal? DifferencePrice { get; set; }
}
public class InventoryCheckDetail : FlcInventoryCheckOutput
{
    /// <summary>
    /// 商品sku
    /// </summary>
    public long SkuId { get; set; }
    /// <summary>
    /// 商品名称
    /// </summary>
    public string GoodsName { get; set; }
    /// <summary>
    /// 规格
    /// </summary>
    public string? speValueList { get; set; }
    /// <summary>
    /// 单位名称
    /// </summary>
    public string UnitName { get; set; }
    /// <summary>
    /// 成本价
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 差异数量
    /// </summary>
    public int? DifferenceNum { get; set; }
    /// <summary>
    /// 差异金额
    /// </summary>
    public decimal DifferencePrice { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? DetailRemark { get; set; }
}

