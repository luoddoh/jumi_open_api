namespace Admin.NET.Application;

    /// <summary>
    /// 供应商分类输出参数
    /// </summary>
    public class FlcCategorySupplierDto
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
        
    }
