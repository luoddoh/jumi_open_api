namespace Admin.NET.Application;

/// <summary>
/// 供应商分类输出参数
/// </summary>
public class FlcCategorySupplierOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 分类名
    /// </summary>
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 上级Id
    /// </summary>
    public long? SuperiorId { get; set; } 
    
    /// <summary>
    /// 上级Id 描述 
    /// </summary>
    public string? SuperiorIdCategoryName { get; set; } 
    
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
