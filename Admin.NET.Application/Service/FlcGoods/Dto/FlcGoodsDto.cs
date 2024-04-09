namespace Admin.NET.Application;

    /// <summary>
    /// 商品信息输出参数
    /// </summary>
    public class FlcGoodsDto
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
        /// 商品封面图
        /// </summary>
        public string? CoverImage { get; set; }
        
        /// <summary>
        /// 商品简介
        /// </summary>
        public string? Description { get; set; }
        
    }

public class SpeListValue
{
    public string SpeName { get; set; }
    public List<string> Values { get; set; }
}
