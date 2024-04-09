using Admin.NET.Application.Entity;
using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 商品规格基础输入参数
    /// </summary>
    public class FlcProductSpecificationsBaseInput
    {
        /// <summary>
        /// 规格名
        /// </summary>
        public virtual string SpeName { get; set; }
        
        /// <summary>
        /// 启用
        /// </summary>
        public virtual bool? Enable { get; set; }

        public List<FlcSpecificationValue>? SpeValues { get; set; }
}

    /// <summary>
    /// 商品规格分页查询输入参数
    /// </summary>
    public class FlcProductSpecificationsInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 规格名
        /// </summary>
        public string? SpeName { get; set; }
        
        /// <summary>
        /// 启用
        /// </summary>
        public bool? Enable { get; set; }
        
    }

    /// <summary>
    /// 商品规格增加输入参数
    /// </summary>
    public class AddFlcProductSpecificationsInput : FlcProductSpecificationsBaseInput
    {
        /// <summary>
        /// 规格名
        /// </summary>
        [Required(ErrorMessage = "规格名不能为空")]
        public override string SpeName { get; set; }
        
    }

    /// <summary>
    /// 商品规格删除输入参数
    /// </summary>
    public class DeleteFlcProductSpecificationsInput : BaseIdInput
    {
    }

    /// <summary>
    /// 商品规格更新输入参数
    /// </summary>
    public class UpdateFlcProductSpecificationsInput : FlcProductSpecificationsBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 商品规格主键查询输入参数
    /// </summary>
    public class QueryByIdFlcProductSpecificationsInput : DeleteFlcProductSpecificationsInput
    {

    }
