using Admin.NET.Application.Entity;
using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 商品sku表基础输入参数
    /// </summary>
    public class FlcGoodsSkuBaseInput
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public virtual long GoodsId { get; set; }
        
        /// <summary>
        /// 条码
        /// </summary>
        public virtual string BarCode { get; set; }
        
        /// <summary>
        /// sku封面
        /// </summary>
        public virtual string? CoverImage { get; set; }
        
        /// <summary>
        /// 单位Id
        /// </summary>
        public virtual long UnitId { get; set; }
        
        /// <summary>
        /// 数量
        /// </summary>
        public virtual int? Number { get; set; }

    public decimal CostPrice { get; set; }

    public decimal SalesPrice { get; set; }

}

    /// <summary>
    /// 商品sku表分页查询输入参数
    /// </summary>
    public class FlcGoodsSkuInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public long? GoodsId { get; set; }
        
        /// <summary>
        /// 条码
        /// </summary>
        public string? BarCode { get; set; }

       
}

public class FlcGoodsSkuInputById
{
 

    /// <summary>
    /// 商品Id
    /// </summary>
    public long GoodsId { get; set; }


}

/// <summary>
/// 商品sku表增加输入参数
/// </summary>
public class AddFlcGoodsSkuInput : FlcGoodsSkuBaseInput
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required(ErrorMessage = "商品Id不能为空")]
        public override long GoodsId { get; set; }
        
        /// <summary>
        /// 条码
        /// </summary>
        [Required(ErrorMessage = "条码不能为空")]
        public override string BarCode { get; set; }
        
        /// <summary>
        /// 单位Id
        /// </summary>
        [Required(ErrorMessage = "单位Id不能为空")]
        public override long UnitId { get; set; }
        
    }

    /// <summary>
    /// 商品sku表删除输入参数
    /// </summary>
    public class DeleteFlcGoodsSkuInput : BaseIdInput
    {
    }

    /// <summary>
    /// 商品sku表更新输入参数
    /// </summary>
    public class UpdateFlcGoodsSkuInput : FlcGoodsSkuBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

        public List<FlcSpecificationValue>? SpeValueList { get; set; }
}

    /// <summary>
    /// 商品sku表主键查询输入参数
    /// </summary>
    public class QueryByIdFlcGoodsSkuInput : DeleteFlcGoodsSkuInput
    {

    }
