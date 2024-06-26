﻿namespace Admin.NET.Application;

    /// <summary>
    /// 采购订货列表输出参数
    /// </summary>
    public class FlcProcureDto
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierIdSupName { get; set; }
        
        /// <summary>
        /// 采购员
        /// </summary>
        public string PurchaserRealName { get; set; }
        
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
        /// 供应商
        /// </summary>
        public long SupplierId { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 采购时间
        /// </summary>
        public DateTime? ProcurementTime { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        
        /// <summary>
        /// 总价格
        /// </summary>
        public decimal? TotalAmount { get; set; }
        
        /// <summary>
        /// 采购员
        /// </summary>
        public long Purchaser { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public long? Reviewer { get; set; }
        
    }
