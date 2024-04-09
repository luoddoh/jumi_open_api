using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 商品规格值
/// </summary>
[SugarTable("flc_specification_value","商品规格值")]
public class FlcSpecificationValue  : EntityBaseData
{
    /// <summary>
    /// 规格Id
    /// </summary>
    [SugarColumn(ColumnName = "SpecificationId", ColumnDescription = "规格Id")]
    public long? SpecificationId { get; set; }
    
    /// <summary>
    /// 规格值
    /// </summary>
    [SugarColumn(ColumnName = "SpeValue", ColumnDescription = "规格值", Length = 100)]
    public string? SpeValue { get; set; }
    
}
