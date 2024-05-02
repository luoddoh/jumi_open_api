namespace Admin.NET.Application;

/// <summary>
/// 商品信息输出参数
/// </summary>
public class FlcGoodsOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 商品名
    /// </summary>
    public string GoodsName { get; set; }
    
    /// <summary>
    /// 商品编码
    /// </summary>
    public string? ProductCode { get; set; }
    
    /// <summary>
    /// 所属分类
    /// </summary>
    public long CategoryId { get; set; } 
    
    /// <summary>
    /// 所属分类 描述 
    /// </summary>
    public string? CategoryIdCategoryName { get; set; } 
    
    /// <summary>
    /// 商品封面图
    /// </summary>
    public string? CoverImage { get; set; }
    
    /// <summary>
    /// 商品简介
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// sku列表
    /// </summary>
    public List<skulabel>? SkuList { get; set; }
    }
 
    public class skulabel
    {
        public long GoodsId { get; set; }
        public string label { get; set;}
    }
    // 使用实际实体flc_category,所以这里就删了
    /*
    [SugarTable("flc_category")]
    public class FlcCategoryTreeOutput: EntityBaseId
    {
        [SugarColumn(ColumnName = "CategoryName")]
        public string? Label { get; set; }
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = false)]
        public long  Value { get; set; }
        public long? SuperiorId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<FlcCategoryTreeOutput> Children { get; set; }
    }
    */
