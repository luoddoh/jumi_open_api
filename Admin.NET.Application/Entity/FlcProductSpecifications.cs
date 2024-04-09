using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 商品规格
/// </summary>
[SugarTable("flc_product_specifications","商品规格")]
public class FlcProductSpecifications  : EntityBaseData
{
    /// <summary>
    /// 规格名
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SpeName", ColumnDescription = "规格名", Length = 45)]
    public string SpeName { get; set; }
    
    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(ColumnName = "Enable", ColumnDescription = "启用")]
    public bool? Enable { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(FlcSpecificationValue.SpecificationId))]
    public List<FlcSpecificationValue> SpeValues { get; set; }
}
