using Admin.NET.Application.Entity;

namespace Admin.NET.Application;

/// <summary>
/// 商品sku表输出参数
/// </summary>
public class FlcGoodsSkuOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 商品Id
    /// </summary>
    public long GoodsId { get; set; }
    
    /// <summary>
    /// 条码
    /// </summary>
    public string BarCode { get; set; }
    
    /// <summary>
    /// sku封面
    /// </summary>
    public string? CoverImage { get; set; }
    
    /// <summary>
    /// 单位Id
    /// </summary>
    public long UnitId { get; set; }
    
    /// <summary>
    /// 数量
    /// </summary>
    public int? Number { get; set; }

    public List<FlcSkuSpeValue> FlcSkuSpeValues { get; set; }
}

public class FlcGoodsSkuOutputs
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 商品Id
    /// </summary>
    public long GoodsId { get; set; }

    public string GoodsName { get; set; }
    public string UnitName { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string BarCode { get; set; }

    /// <summary>
    /// sku封面
    /// </summary>
    public string? CoverImage { get; set; }

    /// <summary>
    /// 库存数量
    /// </summary>
    public int InventoryNum { get; set; }
    /// <summary>
    /// 单位Id
    /// </summary>
    public long UnitId { get; set; }

    public decimal? CostPrice { get; set; }

    public decimal? SalesPrice { get; set; }

    public List<labval> speValueList { get; set; }
}

public class labval
{
    public long? Id { get; set; }
    public long? SpecificationId { get; set; }
    public string? SpeValue { get; set; }
}



