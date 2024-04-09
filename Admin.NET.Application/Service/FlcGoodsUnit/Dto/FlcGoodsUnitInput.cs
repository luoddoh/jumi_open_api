using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 商品单位基础输入参数
    /// </summary>
    public class FlcGoodsUnitBaseInput
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        public virtual string? UnitName { get; set; }
        
    }

    /// <summary>
    /// 商品单位分页查询输入参数
    /// </summary>
    public class FlcGoodsUnitInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string? UnitName { get; set; }
        
    }

    /// <summary>
    /// 商品单位增加输入参数
    /// </summary>
    public class AddFlcGoodsUnitInput : FlcGoodsUnitBaseInput
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        [Required(ErrorMessage = "单位名称不能为空")]
        public override string? UnitName { get; set; }
        
    }

    /// <summary>
    /// 商品单位删除输入参数
    /// </summary>
    public class DeleteFlcGoodsUnitInput : BaseIdInput
    {
    }

    /// <summary>
    /// 商品单位更新输入参数
    /// </summary>
    public class UpdateFlcGoodsUnitInput : FlcGoodsUnitBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 商品单位主键查询输入参数
    /// </summary>
    public class QueryByIdFlcGoodsUnitInput : DeleteFlcGoodsUnitInput
    {

    }
