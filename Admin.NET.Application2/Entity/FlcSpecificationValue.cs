using Admin.NET.Core;
namespace Admin.NET.Application2.Entity;

/// <summary>
/// 商品规格值
/// </summary>
[SugarTable("flc_specification_value","商品规格值")]
public class FlcSpecificationValue  : EntityBaseData
{
    /// <summary>
    /// 规格Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SpecificationId", ColumnDescription = "规格Id")]
    public int SpecificationId { get; set; }
    
    /// <summary>
    /// 规格值
    /// </summary>
    [SugarColumn(ColumnName = "SpeValue", ColumnDescription = "规格值", Length = 100)]
    public string? SpeValue { get; set; }
    
    /// <summary>
    /// 软删除
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "IsDeleted", ColumnDescription = "软删除")]
    public bool IsDeleted { get; set; }
    
}
