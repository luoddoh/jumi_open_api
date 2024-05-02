using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 商品信息基础输入参数
    /// </summary>
    public class FlcGoodsBaseInput
    {
        /// <summary>
        /// 商品名
        /// </summary>
        public virtual string GoodsName { get; set; }
        
        /// <summary>
        /// 商品编码
        /// </summary>
        public virtual string? ProductCode { get; set; }
        
        /// <summary>
        /// 所属分类
        /// </summary>
        public virtual long CategoryId { get; set; }
        
        /// <summary>
        /// 商品封面图
        /// </summary>
        public virtual string? CoverImage { get; set; }
        
        /// <summary>
        /// 商品简介
        /// </summary>
        public virtual string? Description { get; set; }
        
    }

    /// <summary>
    /// 商品信息分页查询输入参数
    /// </summary>
    public class FlcGoodsInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        public string? GoodsName { get; set; }
        
        /// <summary>
        /// 商品编码
        /// </summary>
        public string? ProductCode { get; set; }
        
        /// <summary>
        /// 所属分类
        /// </summary>
        public long? CategoryId { get; set; }
        
        /// <summary>
        /// 商品简介
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        public string? Sku { get; set; }
}

    /// <summary>
    /// 商品信息增加输入参数
    /// </summary>
    public class AddFlcGoodsInput : FlcGoodsBaseInput
    {
        /// <summary>
        /// 商品名
        /// </summary>
        [Required(ErrorMessage = "商品名不能为空")]
        public override string GoodsName { get; set; }
        
        /// <summary>
        /// 所属分类
        /// </summary>
        [Required(ErrorMessage = "所属分类不能为空")]
        public override long CategoryId { get; set; }
        
    }

    /// <summary>
    /// 商品信息删除输入参数
    /// </summary>
    public class DeleteFlcGoodsInput : BaseIdInput
    {
    }

    /// <summary>
    /// 商品信息更新输入参数
    /// </summary>
    public class UpdateFlcGoodsInput : FlcGoodsBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 商品信息主键查询输入参数
    /// </summary>
    public class QueryByIdFlcGoodsInput : DeleteFlcGoodsInput
    {

    }
public class SaveInput
{
   public List<UploadFileGoodsInput> table {  get; set; }
    public List<speVal>? speVals { get; set; }
}
public class speVal
{
    public string speName {  get; set; }
    public List<string> values { get; set; }
}
public class UploadFileGoodsInput
{
    /// <summary>
    /// 一级分类
    /// </summary>
    public string? oneClass { set; get; }
    /// <summary>
    /// 二级分类
    /// </summary>
    public string? TwoClass { set; get; }
    /// <summary>
    /// 三级分类
    /// </summary>
    public string? ThreeClass { set; get; }
    /// <summary>
    /// 商品全名
    /// </summary>
    public string? GoodsName { set; get; }
    /// <summary>
    /// 条码
    /// </summary>
    public string? BarCard { set; get; }
    /// <summary>
    /// SKU编号
    /// </summary>
    public string? SkuCode { set; get; }
    /// <summary>
    /// 重量(kg)
    /// </summary>
    public int? Weight { set; get; }
    /// <summary>
    /// 产地
    /// </summary>
    public string? Producer { set; get; }
    /// <summary>
    /// 零售价
    /// </summary>
    public string? RetailPrice { set; get; }
    /// <summary>
    /// 参考成本价
    /// </summary>
    public string? costPrice { set; get; }
    /// <summary>
    /// 基本单位
    /// </summary>
    public string? Unit { set; get; }
    public string? PrintCustom { set; get; }
}
