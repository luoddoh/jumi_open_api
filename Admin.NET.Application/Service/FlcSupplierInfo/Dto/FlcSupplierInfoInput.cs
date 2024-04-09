using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 供应商信息基础输入参数
    /// </summary>
    public class FlcSupplierInfoBaseInput
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string SupName { get; set; }
        
        /// <summary>
        /// 供应商分类
        /// </summary>
        public virtual long CategoryId { get; set; }
        
    }

    /// <summary>
    /// 供应商信息分页查询输入参数
    /// </summary>
    public class FlcSupplierInfoInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string? SupName { get; set; }
        
        /// <summary>
        /// 供应商分类
        /// </summary>
        public long? CategoryId { get; set; }
        
    }

    /// <summary>
    /// 供应商信息增加输入参数
    /// </summary>
    public class AddFlcSupplierInfoInput : FlcSupplierInfoBaseInput
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Required(ErrorMessage = "供应商名称不能为空")]
        public override string SupName { get; set; }
        
        /// <summary>
        /// 供应商分类
        /// </summary>
        [Required(ErrorMessage = "供应商分类不能为空")]
        public override long CategoryId { get; set; }
        
    }

    /// <summary>
    /// 供应商信息删除输入参数
    /// </summary>
    public class DeleteFlcSupplierInfoInput : BaseIdInput
    {
    }

    /// <summary>
    /// 供应商信息更新输入参数
    /// </summary>
    public class UpdateFlcSupplierInfoInput : FlcSupplierInfoBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 供应商信息主键查询输入参数
    /// </summary>
    public class QueryByIdFlcSupplierInfoInput : DeleteFlcSupplierInfoInput
    {

    }
