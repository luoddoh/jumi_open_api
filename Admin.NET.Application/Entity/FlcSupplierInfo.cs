using Admin.NET.Core;
namespace Admin.NET.Application.Entity;

/// <summary>
/// 供应商信息
/// </summary>
[SugarTable("flc_supplier_info","供应商信息")]
public class FlcSupplierInfo  : EntityBaseData
{
    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SupName", ColumnDescription = "供应商名称", Length = 32)]
    public string SupName { get; set; }
    
    /// <summary>
    /// 分类Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CategoryId", ColumnDescription = "分类Id")]
    public long CategoryId { get; set; }

    /// <summary>
    /// 供应商编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "code", ColumnDescription = "供应商编码", Length = 100)]
    public string? code { get; set; }
}
