using Admin.NET.Application.Entity;

namespace Admin.NET.Application;

/// <summary>
/// 商品规格输出参数
/// </summary>
public class FlcProductSpecificationsOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 规格名
    /// </summary>
    public string SpeName { get; set; }
    
    /// <summary>
    /// 启用
    /// </summary>
    public bool? Enable { get; set; }


    public List<FlcSpecificationValue>? SpeValues { get; set; }
}
 

