using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 入库单基础输入参数
    /// </summary>
    public class FlcInventoryInputListBaseInput
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public virtual string? DocNumber { get; set; }
        
        /// <summary>
        /// 入库时间
        /// </summary>
        public virtual DateTime InputTime { get; set; }
        
        /// <summary>
        /// 单据状态
        /// </summary>
        public virtual string? State { get; set; }
        
        /// <summary>
        /// 操作员
        /// </summary>
        public virtual long Operator { get; set; }
        
        /// <summary>
        /// 入库类型
        /// </summary>
        public virtual string InputType { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual long? Reviewer { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime? ReviewerTime { get; set; }
        
        /// <summary>
        /// 订单备注
        /// </summary>
        public virtual string? Remark { get; set; }
        
    }

    /// <summary>
    /// 入库单分页查询输入参数
    /// </summary>
    public class FlcInventoryInputListInput : BasePageInput
    {
        public long? Uid { get; set; }
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string? DocNumber { get; set; }
        
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? InputTime { get; set; }
        
        /// <summary>
         /// 入库时间范围
         /// </summary>
         public List<DateTime?> InputTimeRange { get; set; } 
        /// <summary>
        /// 入库类型
        /// </summary>
        public string? InputType { get; set; }
        
        /// <summary>
        /// 订单备注
        /// </summary>
        public string? Remark { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
        public string? State { get; set; }
        
    }

    /// <summary>
    /// 入库单增加输入参数
    /// </summary>
    public class AddFlcInventoryInputListInput : FlcInventoryInputListBaseInput
    {
        /// <summary>
        /// 入库时间
        /// </summary>
        [Required(ErrorMessage = "入库时间不能为空")]
        public override DateTime InputTime { get; set; }
        
        /// <summary>
        /// 单据状态
        /// </summary>
        public override string? State { get; set; }
        
        /// <summary>
        /// 操作员
        /// </summary>
        [Required(ErrorMessage = "操作员不能为空")]
        public override long Operator { get; set; }
        
        /// <summary>
        /// 入库类型
        /// </summary>
        [Required(ErrorMessage = "入库类型不能为空")]
        public override string InputType { get; set; }
        
    }

    /// <summary>
    /// 入库单删除输入参数
    /// </summary>
    public class DeleteFlcInventoryInputListInput : BaseIdInput
    {
    }

    /// <summary>
    /// 入库单更新输入参数
    /// </summary>
    public class UpdateFlcInventoryInputListInput : FlcInventoryInputListBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 入库单主键查询输入参数
    /// </summary>
    public class QueryByIdFlcInventoryInputListInput : DeleteFlcInventoryInputListInput
    {

    }

public class ExamineInvInput
{
    /// <summary>
    /// 订单Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 状态值
    /// </summary>
    public string state { get; set; }

    public long? Reviewer { get; set; }
}

