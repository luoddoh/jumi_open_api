namespace Admin.NET.Application;

    /// <summary>
    /// 供应商信息输出参数
    /// </summary>
    public class FlcSupplierInfoDto
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
        
    }
