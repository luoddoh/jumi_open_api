﻿namespace Admin.NET.Application;

/// <summary>
/// 出库单输出参数
/// </summary>
public class FlcInventoryOutOutput
{
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
    public string OutType { get; set; }
    /// <summary>
    /// 单据状态
    /// </summary>
    public string State { get; set; }
    /// <summary>
    /// 出库时间
    /// </summary>
    public DateTime OutTime { get; set; }
    /// <summary>
    /// 出库数量
    /// </summary>
    public int? OutNum { get; set; }
    /// <summary>
    /// 总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }
    /// <summary>
    /// 订单备注
    /// </summary>
    public string? Remark { get; set; }
    
    /// <summary>
    /// 审核人
    /// </summary>
    public long? Reviewer { get; set; } 
    
    /// <summary>
    /// 审核人 描述
    /// </summary>
    public string ReviewerRealName { get; set; } 
    
    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? ReviewerTime { get; set; }

    /// <summary>
    /// 操作员
    /// </summary>
    public string? OperatorRealName { get; set; }


    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreateUserName { get; set; }
}
 
