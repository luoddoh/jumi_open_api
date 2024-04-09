namespace Admin.NET.Application;

    /// <summary>
    /// 出库单输出参数
    /// </summary>
    public class FlcInventoryOutDto
    {
        /// <summary>
        /// 审核人
        /// </summary>
        public string ReviewerRealName { get; set; }
        
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 单据号
        /// </summary>
        public string DocNumber { get; set; }
        
        /// <summary>
        /// 出库类型
        /// </summary>
        public int OutType { get; set; }
        
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime OutTime { get; set; }
        
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
        
    }

public class ExamineInvOutInput
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
