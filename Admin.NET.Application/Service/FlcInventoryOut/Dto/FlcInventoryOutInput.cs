using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 出库单基础输入参数
    /// </summary>
    public class FlcInventoryOutBaseInput
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public virtual string DocNumber { get; set; }
        
        /// <summary>
        /// 出库类型
        /// </summary>
        public virtual string OutType { get; set; }

    /// <summary>
    /// 单据状态
    /// </summary>
    public virtual string State { get; set; }
    /// <summary>
    /// 出库时间
    /// </summary>
    public virtual DateTime OutTime { get; set; }
        
        /// <summary>
        /// 订单备注
        /// </summary>
        public virtual string? Remark { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual long? Reviewer { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime? ReviewerTime { get; set; }

    /// <summary>
    /// 操作员Id
    /// </summary>
    public long Operator { get; set; }

}

    /// <summary>
    /// 出库单分页查询输入参数
    /// </summary>
    public class FlcInventoryOutInput : BasePageInput
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
        /// 出库类型
        /// </summary>
        public string? OutType { get; set; }

    /// <summary>
    /// 单据状态
    /// </summary>
    public string? State { get; set; }
    /// <summary>
    /// 出库时间
    /// </summary>
    public DateTime? OutTime { get; set; }
        
        /// <summary>
         /// 出库时间范围
         /// </summary>
         public List<DateTime?> OutTimeRange { get; set; } 
        /// <summary>
        /// 订单备注
        /// </summary>
        public string? Remark { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public long? Reviewer { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ReviewerTime { get; set; }
        
        /// <summary>
         /// 审核时间范围
         /// </summary>
         public List<DateTime?> ReviewerTimeRange { get; set; }
    /// <summary>
    /// 操作员Id
    /// </summary>
    public long? Operator { get; set; }
}

    /// <summary>
    /// 出库单增加输入参数
    /// </summary>
    public class AddFlcInventoryOutInput : FlcInventoryOutBaseInput
    {
        /// <summary>
        /// 出库类型
        /// </summary>
        [Required(ErrorMessage = "出库类型不能为空")]
        public override string OutType { get; set; }
        
        /// <summary>
        /// 出库时间
        /// </summary>
        [Required(ErrorMessage = "出库时间不能为空")]
        public override DateTime OutTime { get; set; }
        
    }

    /// <summary>
    /// 出库单删除输入参数
    /// </summary>
    public class DeleteFlcInventoryOutInput : BaseIdInput
    {
    }

    /// <summary>
    /// 出库单更新输入参数
    /// </summary>
    public class UpdateFlcInventoryOutInput : FlcInventoryOutBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 出库单主键查询输入参数
    /// </summary>
    public class QueryByIdFlcInventoryOutInput : DeleteFlcInventoryOutInput
    {

    }
