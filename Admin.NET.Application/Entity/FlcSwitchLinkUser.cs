using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 开关用户链接表
/// </summary>
[SugarTable("flcSwitchLinkUser","开关用户链接表")]
public class FlcSwitchLinkUser  : EntityBaseData
{
    /// <summary>
    /// 开关Id
    /// </summary>
    [SugarColumn(ColumnName = "SwitchId", ColumnDescription = "开关Id")]
    public long? SwitchId { get; set; }
    
    /// <summary>
    /// 用户Id
    /// </summary>
    [SugarColumn(ColumnName = "UserId", ColumnDescription = "用户Id")]
    public long? UserId { get; set; }
    
}
