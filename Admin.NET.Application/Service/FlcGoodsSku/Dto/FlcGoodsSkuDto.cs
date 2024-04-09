namespace Admin.NET.Application;

    /// <summary>
    /// 商品sku表输出参数
    /// </summary>
    public class FlcGoodsSkuDto
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
        
    }
