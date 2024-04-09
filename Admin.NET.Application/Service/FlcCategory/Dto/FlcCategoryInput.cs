using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 商品分类基础输入参数
    /// </summary>
    public class FlcCategoryBaseInput
    {
        /// <summary>
        /// 分类名
        /// </summary>
        public virtual string? CategoryName { get; set; }
        
        /// <summary>
        /// 创建者部门Id
        /// </summary>
        public virtual long? CreateOrgId { get; set; }
        
        /// <summary>
        /// 创建者部门名称
        /// </summary>
        public virtual string? CreateOrgName { get; set; }
        
        /// <summary>
        /// 上级id
        /// </summary>
        public virtual long? SuperiorId { get; set; }
        
    }

    /// <summary>
    /// 商品分类分页查询输入参数
    /// </summary>
    public class FlcCategoryInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 分类名
        /// </summary>
        public string? CategoryName { get; set; }
        
        /// <summary>
        /// 上级id
        /// </summary>
        public long? SuperiorId { get; set; }
        
    }

    /// <summary>
    /// 商品分类增加输入参数
    /// </summary>
    public class AddFlcCategoryInput : FlcCategoryBaseInput
    {
    }

    /// <summary>
    /// 商品分类删除输入参数
    /// </summary>
    public class DeleteFlcCategoryInput : BaseIdInput
    {
    }

    /// <summary>
    /// 商品分类更新输入参数
    /// </summary>
    public class UpdateFlcCategoryInput : FlcCategoryBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 商品分类主键查询输入参数
    /// </summary>
    public class QueryByIdFlcCategoryInput : DeleteFlcCategoryInput
    {

    }
