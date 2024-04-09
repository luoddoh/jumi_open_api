namespace Admin.NET.Application;

    /// <summary>
    /// 盘点单输出参数
    /// </summary>
    public class FlcInventoryCheckDto
    {
        /// <summary>
        /// 盘点人
        /// </summary>
        public string CheckPeopleRealName { get; set; }
        
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
        /// 状态
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// 盘点人
        /// </summary>
        public long CheckPeople { get; set; }
        
        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime CheckTime { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public long? Reviewer { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ReviewerTime { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        
    }

public class ExamineInvChecInput
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
