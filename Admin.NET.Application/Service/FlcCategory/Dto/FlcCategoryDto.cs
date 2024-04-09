namespace Admin.NET.Application;

    /// <summary>
    /// 商品分类输出参数
    /// </summary>
    public class FlcCategoryDto
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
        
    }
