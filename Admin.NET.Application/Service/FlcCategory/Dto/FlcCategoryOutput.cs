namespace Admin.NET.Application;

/// <summary>
/// 商品分类输出参数
/// </summary>
public class FlcCategoryOutput
{
    /// <summary>
    /// 分类名
    /// </summary>
    public string? CategoryName { get; set; }
    
    /// <summary>
    /// 创建者部门Id
    /// </summary>
    public long? CreateOrgId { get; set; }
    
    /// <summary>
    /// 创建者部门名称
    /// </summary>
    public string? CreateOrgName { get; set; }
    
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 上级id
    /// </summary>
    public long? SuperiorId { get; set; } 
    
    /// <summary>
    /// 上级id 描述 
    /// </summary>
    public string? SuperiorIdCategoryName { get; set; } 
    
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
