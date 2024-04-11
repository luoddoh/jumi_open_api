using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 库存查询基础输入参数
    /// </summary>
    public class FlcInventoryBaseInput
    {
        /// <summary>
        /// sku表id
        /// </summary>
        public virtual long SkuId { get; set; }
        
        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Number { get; set; }
        
        /// <summary>
        /// 总价格
        /// </summary>
        public virtual decimal TotalAmount { get; set; }
        
    }

    /// <summary>
    /// 库存查询分页查询输入参数
    /// </summary>
    public class FlcInventoryInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// 总价格小于
        /// </summary>
        public decimal? MinTotalAmount { get; set; }
        /// <summary>
        /// 总价格大于
        /// </summary>
        public decimal? MaxTotalAmount { get; set; }
}

    /// <summary>
    /// 库存查询增加输入参数
    /// </summary>
    public class AddFlcInventoryInput : FlcInventoryBaseInput
    {
        /// <summary>
        /// sku表id
        /// </summary>
        [Required(ErrorMessage = "sku表id不能为空")]
        public override long SkuId { get; set; }
        
        /// <summary>
        /// 数量
        /// </summary>
        [Required(ErrorMessage = "数量不能为空")]
        public override int Number { get; set; }
        
        /// <summary>
        /// 总价格
        /// </summary>
        [Required(ErrorMessage = "总价格不能为空")]
        public override decimal TotalAmount { get; set; }
        
    }

    /// <summary>
    /// 库存查询删除输入参数
    /// </summary>
    public class DeleteFlcInventoryInput : BaseIdInput
    {
    }

    /// <summary>
    /// 库存查询更新输入参数
    /// </summary>
    public class UpdateFlcInventoryInput : FlcInventoryBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 库存查询主键查询输入参数
    /// </summary>
    public class QueryByIdFlcInventoryInput : DeleteFlcInventoryInput
    {

    }
