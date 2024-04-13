namespace Admin.NET.Application;

/// <summary>
/// 供应商信息输出参数
/// </summary>
public class FlcSupplierInfoOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupName { get; set; }
    
    /// <summary>
    /// 供应商分类
    /// </summary>
    public long CategoryId { get; set; } 
    
    /// <summary>
    /// 供应商分类 描述 
    /// </summary>
    public string? CategoryIdCategoryName { get; set; }

    public string? code { get; set; }

}
 

    // 使用实际实体flc_category_supplier,所以这里就删了
    /*
    [SugarTable("flc_category_supplier")]
    public class FlcCategorySupplierTreeOutput: EntityBaseId
    {
        [SugarColumn(ColumnName = "CategoryName")]
        public string? Label { get; set; }
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = false)]
        public long  Value { get; set; }
        public long? SuperiorId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<FlcCategorySupplierTreeOutput> Children { get; set; }
    }
    */
