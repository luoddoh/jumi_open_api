using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 盘点单基础输入参数
    /// </summary>
    public class FlcInventoryCheckBaseInput
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public virtual string DocNumber { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public virtual string State { get; set; }
        
        /// <summary>
        /// 盘点人
        /// </summary>
        public virtual long CheckPeople { get; set; }
        
        /// <summary>
        /// 盘点时间
        /// </summary>
        public virtual DateTime CheckTime { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual long? Reviewer { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime? ReviewerTime { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        
    }

    /// <summary>
    /// 盘点单分页查询输入参数
    /// </summary>
    public class FlcInventoryCheckInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string? DocNumber { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public string? State { get; set; }
        
        /// <summary>
        /// 盘点人
        /// </summary>
        public long? CheckPeople { get; set; }
        
        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime? CheckTime { get; set; }
        
        /// <summary>
         /// 盘点时间范围
         /// </summary>
         public List<DateTime?> CheckTimeRange { get; set; } 
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
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    public long? uid { get; set; }
}

    /// <summary>
    /// 盘点单增加输入参数
    /// </summary>
    public class AddFlcInventoryCheckInput : FlcInventoryCheckBaseInput
    {
        /// <summary>
        /// 盘点人
        /// </summary>
        [Required(ErrorMessage = "盘点人不能为空")]
        public override long CheckPeople { get; set; }
        
        /// <summary>
        /// 盘点时间
        /// </summary>
        [Required(ErrorMessage = "盘点时间不能为空")]
        public override DateTime CheckTime { get; set; }
        
    }

    /// <summary>
    /// 盘点单删除输入参数
    /// </summary>
    public class DeleteFlcInventoryCheckInput : BaseIdInput
    {
    }

    /// <summary>
    /// 盘点单更新输入参数
    /// </summary>
    public class UpdateFlcInventoryCheckInput : FlcInventoryCheckBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 盘点单主键查询输入参数
    /// </summary>
    public class QueryByIdFlcInventoryCheckInput : DeleteFlcInventoryCheckInput
    {

    }
