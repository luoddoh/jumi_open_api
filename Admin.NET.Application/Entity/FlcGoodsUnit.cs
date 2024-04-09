using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 商品单位
/// </summary>
[SugarTable("flc_goods_unit","商品单位")]
public class FlcGoodsUnit  : EntityBaseData
{
    /// <summary>
    /// 单位名称
    /// </summary>
    [SugarColumn(ColumnName = "UnitName", ColumnDescription = "单位名称", Length = 32)]
    public string? UnitName { get; set; }
    
}
