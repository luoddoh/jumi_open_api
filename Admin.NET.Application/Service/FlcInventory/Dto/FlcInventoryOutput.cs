using Admin.NET.Core;

namespace Admin.NET.Application;

/// <summary>
/// 库存查询输出参数
/// </summary>
public class FlcInventoryOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// sku表id
    /// </summary>
    public long SkuId { get; set; } 
    
    /// <summary>
    /// 条码
    /// </summary>
    public string SkuIdBarCode { get; set; }

    /// <summary>
    /// 商品名
    /// </summary>
    public string GoodesName { get; set; }

    /// <summary>
    /// 单位名
    /// </summary>
    public string UnitName { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    public int Number { get; set; }
    
    /// <summary>
    /// 总价格
    /// </summary>
    public decimal TotalAmount { get; set; }


    public string speValue { get; set; }
}

public class FlcOutputpage<TEntity> : SqlSugarPagedList<TEntity>
{
    public int? TotalNumber { get; set; }
    public decimal? TotalAmount { get; set; }
    public int? planTotalNumber { get; set; }
}



