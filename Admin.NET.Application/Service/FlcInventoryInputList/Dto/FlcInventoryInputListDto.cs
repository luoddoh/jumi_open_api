namespace Admin.NET.Application;

    /// <summary>
    /// 入库单输出参数
    /// </summary>
    public class FlcInventoryInputListDto
    {
        /// <summary>
        /// 操作员
        /// </summary>
        public string OperatorRealName { get; set; }
        
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
        public string? DocNumber { get; set; }
        
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime InputTime { get; set; }
        
        /// <summary>
        /// 单据状态
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// 操作员
        /// </summary>
        public long Operator { get; set; }
        
        /// <summary>
        /// 入库类型
        /// </summary>
        public string InputType { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public long? Reviewer { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ReviewerTime { get; set; }
        
        /// <summary>
        /// 订单备注
        /// </summary>
        public string? Remark { get; set; }
        
    }
