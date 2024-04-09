using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 采购退货列表基础输入参数
    /// </summary>
    public class FlcProcureReturnBaseInput
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public virtual string? DocNumber { get; set; }
        
        /// <summary>
        /// 供应商
        /// </summary>
        public virtual long? SupplierId { get; set; }
        
        /// <summary>
        /// 退货员
        /// </summary>
        public virtual long? Returner { get; set; }
        
        /// <summary>
        /// 退货时间
        /// </summary>
        public virtual DateTime? ReturnTime { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual long? Reviewer { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 总价格
        /// </summary>
        public virtual decimal? TotalAmount { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        
    }

    /// <summary>
    /// 采购退货列表分页查询输入参数
    /// </summary>
    public class FlcProcureReturnInput : BasePageInput
    {
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
        /// 退货员
        /// </summary>
        public long? Returner { get; set; }
        
        /// <summary>
        /// 退货时间
        /// </summary>
        public DateTime? ReturnTime { get; set; }
        
        /// <summary>
         /// 退货时间范围
         /// </summary>
         public List<DateTime?> ReturnTimeRange { get; set; } 
    }

    /// <summary>
    /// 采购退货列表增加输入参数
    /// </summary>
    public class AddFlcProcureReturnInput : FlcProcureReturnBaseInput
    {
    }

    /// <summary>
    /// 采购退货列表删除输入参数
    /// </summary>
    public class DeleteFlcProcureReturnInput : BaseIdInput
    {
    }

    /// <summary>
    /// 采购退货列表更新输入参数
    /// </summary>
    public class UpdateFlcProcureReturnInput : FlcProcureReturnBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 采购退货列表主键查询输入参数
    /// </summary>
    public class QueryByIdFlcProcureReturnInput : DeleteFlcProcureReturnInput
    {

    }


public class ConfirmInput
{
    public long Id { get; set; }
}
