using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 采购订货列表基础输入参数
    /// </summary>
    public class FlcProcureBaseInput
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public virtual string DocNumber { get; set; }
        
        /// <summary>
        /// 供应商
        /// </summary>
        public virtual long SupplierId { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int State { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 采购时间
        /// </summary>
        public virtual DateTime? ProcurementTime { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        
        /// <summary>
        /// 总价格
        /// </summary>
        public virtual decimal? TotalAmount { get; set; }
        
        /// <summary>
        /// 采购员
        /// </summary>
        public virtual long Purchaser { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual long? Reviewer { get; set; }
        
    }

    /// <summary>
    /// 采购订货列表分页查询输入参数
    /// </summary>
    public class FlcProcureInput : BasePageInput
    {
    /// <summary>
    /// 小程序登录用户id
    /// </summary>
    public long? uid { get; set; }

    /// <summary>
    /// 关键字查询
    /// </summary>
    public string? SearchKey { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string? DocNumber { get; set; }
        
        /// <summary>
        /// 供应商
        /// </summary>
        public long? SupplierId { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public int? State { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
         /// 审核时间范围
         /// </summary>
         public List<DateTime?> AuditTimeRange { get; set; } 
        /// <summary>
        /// 采购时间
        /// </summary>
        public DateTime? ProcurementTime { get; set; }
        
        /// <summary>
         /// 采购时间范围
         /// </summary>
         public List<DateTime?> ProcurementTimeRange { get; set; } 
        /// <summary>
        /// 总价格
        /// </summary>
        public decimal? TotalAmount { get; set; }
        
        /// <summary>
        /// 采购员
        /// </summary>
        public long? Purchaser { get; set; }
        
    }

    /// <summary>
    /// 采购订货列表增加输入参数
    /// </summary>
    public class AddFlcProcureInput : FlcProcureBaseInput
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public override string? DocNumber { get; set; }
        
        /// <summary>
        /// 供应商
        /// </summary>
        [Required(ErrorMessage = "供应商不能为空")]
        public override long SupplierId { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        public override int State { get; set; }
        
        /// <summary>
        /// 采购员
        /// </summary>
        [Required(ErrorMessage = "采购员不能为空")]
        public override long Purchaser { get; set; }
        
    }

    /// <summary>
    /// 采购订货列表删除输入参数
    /// </summary>
    public class DeleteFlcProcureInput : BaseIdInput
    {
    }

    /// <summary>
    /// 采购订货列表更新输入参数
    /// </summary>
    public class UpdateFlcProcureInput : FlcProcureBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 采购订货列表主键查询输入参数
    /// </summary>
    public class QueryByIdFlcProcureInput : DeleteFlcProcureInput
    {

    }

/// <summary>
/// 采购订订单审核参数
/// </summary>
public class ExamineInput
{
    /// <summary>
    /// 订单Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 状态值
    /// </summary>
    public int state { get; set; }

    public long? Reviewer { get; set; }
}
