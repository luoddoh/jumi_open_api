using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 供应商分类基础输入参数
    /// </summary>
    public class FlcCategorySupplierBaseInput
    {
        /// <summary>
        /// 分类名
        /// </summary>
        public virtual string? CategoryName { get; set; }
        
        /// <summary>
        /// 上级Id
        /// </summary>
        public virtual long? SuperiorId { get; set; }
        
    }

    /// <summary>
    /// 供应商分类分页查询输入参数
    /// </summary>
    public class FlcCategorySupplierInput : BasePageInput
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
        /// 上级Id
        /// </summary>
        public long? SuperiorId { get; set; }
        
    }

    /// <summary>
    /// 供应商分类增加输入参数
    /// </summary>
    public class AddFlcCategorySupplierInput : FlcCategorySupplierBaseInput
    {
    }

    /// <summary>
    /// 供应商分类删除输入参数
    /// </summary>
    public class DeleteFlcCategorySupplierInput : BaseIdInput
    {
    }

    /// <summary>
    /// 供应商分类更新输入参数
    /// </summary>
    public class UpdateFlcCategorySupplierInput : FlcCategorySupplierBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 供应商分类主键查询输入参数
    /// </summary>
    public class QueryByIdFlcCategorySupplierInput : DeleteFlcCategorySupplierInput
    {

    }
