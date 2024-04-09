using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// sku与规格值关联(一对多)
/// </summary>
[SugarTable("flc_sku_speValue","sku与规格值关联(一对多)")]
public class FlcSkuSpeValue  : EntityBaseData
{
    /// <summary>
    /// sku表Id
    /// </summary>
    [SugarColumn(ColumnName = "SkuId", ColumnDescription = "sku表Id")]
    public long? SkuId { get; set; }
    
    /// <summary>
    /// 规格值Id
    /// </summary>
    [SugarColumn(ColumnName = "SpeValueId", ColumnDescription = "规格值Id")]
    public long? SpeValueId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SpeValueId))]
    public FlcSpecificationValue FlcSpecificationValue { get; set; }

}
