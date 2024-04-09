using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 采购明细基础输入参数
    /// </summary>
    public class FlcProcureDetailBaseInput
    {
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public virtual long ProcureId { get; set; }
        
        /// <summary>
        /// 商品Id
        /// </summary>
        public virtual long GoodsId { get; set; }
        
        /// <summary>
        /// 商品sku
        /// </summary>
        public virtual long SkuId { get; set; }
        
        /// <summary>
        /// 库存量
        /// </summary>
        public virtual int? InventoryNum { get; set; }
        
        /// <summary>
        /// 采购价
        /// </summary>
        public virtual decimal purchasePrice { get; set; }
        
        /// <summary>
        /// 采购数量
        /// </summary>
        public virtual int purchaseNum { get; set; }
        
        /// <summary>
        /// 已到数量
        /// </summary>
        public virtual int okNum { get; set; }

        /// <summary>
        ///未到数量 
        /// </summary>
    public virtual int noNum { get; set; }
        
        /// <summary>
        /// 总金额
        /// </summary>
        public virtual decimal totalAmount { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? remark { get; set; }
        
    }

    /// <summary>
    /// 采购明细查询参数
    /// </summary>
    public class FlcProcureDetailInput
    {

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public long? ProcureId { get; set; }
        
    }

    /// <summary>
    /// 采购明细增加输入参数
    /// </summary>
    public class AddFlcProcureDetailInput : FlcProcureDetailBaseInput
    {
        /// <summary>
        /// 采购订单Id
        /// </summary>
        [Required(ErrorMessage = "采购订单Id不能为空")]
        public override long ProcureId { get; set; }
        
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required(ErrorMessage = "商品Id不能为空")]
        public override long GoodsId { get; set; }
        
        /// <summary>
        /// 商品sku
        /// </summary>
        [Required(ErrorMessage = "商品sku不能为空")]
        public override long SkuId { get; set; }
        
        /// <summary>
        /// 采购价
        /// </summary>
        [Required(ErrorMessage = "采购价不能为空")]
        public override decimal purchasePrice { get; set; }
        
        /// <summary>
        /// 采购数量
        /// </summary>
        [Required(ErrorMessage = "采购数量不能为空")]
        public override int purchaseNum { get; set; }
        
        /// <summary>
        /// 未到数量
        /// </summary>
        [Required(ErrorMessage = "已到数量不能为空")]
        public override int okNum { get; set; }
        
        /// <summary>
        /// 已到数量
        /// </summary>
        [Required(ErrorMessage = "未到数量不能为空")]
        public override int noNum { get; set; }
        
        /// <summary>
        /// 总金额
        /// </summary>
        [Required(ErrorMessage = "总金额不能为空")]
        public override decimal totalAmount { get; set; }
        
    }

    /// <summary>
    /// 采购明细删除输入参数
    /// </summary>
    public class DeleteFlcProcureDetailInput : BaseIdInput
    {
    }

    /// <summary>
    /// 采购明细更新输入参数
    /// </summary>
    public class UpdateFlcProcureDetailInput : FlcProcureDetailBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public virtual long? Id { get; set; }
        
    }

/// <summary>
/// 采购验货输入参数
/// </summary>
public class InspectionFlcProcureDetailInput : FlcProcureDetailBaseInput
{

    /// <summary>
    /// 主键Id
    /// </summary>
    public virtual long Id { get; set; }
    /// <summary>
    /// 到货数量
    /// </summary>
    public int OneOkNumber { get; set; }

    public decimal ok_totalAmount { get; set; }

}
/// <summary>
/// 采购明细主键查询输入参数
/// </summary>
public class QueryByIdFlcProcureDetailInput : DeleteFlcProcureDetailInput
    {

    }

public class PrintInput
{
    public long Id { get; set; }
    public int Num {  get; set; }
}

public class CodeInput
{
    public string Code { get; set; }
}
