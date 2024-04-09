namespace Admin.NET.Application;

    /// <summary>
    /// 采购明细输出参数
    /// </summary>
    public class FlcProcureReturnDetailDto
{
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public string ProcureIdDocNumber { get; set; }
        
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsIdGoodsName { get; set; }
        
        /// <summary>
        /// 商品sku
        /// </summary>
        public long SkuIdId { get; set; }
        
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public long ProcureId { get; set; }
        
        /// <summary>
        /// 商品Id
        /// </summary>
        public long GoodsId { get; set; }
        
        /// <summary>
        /// 商品sku
        /// </summary>
        public long SkuId { get; set; }
        
        /// <summary>
        /// 库存量
        /// </summary>
        public int? InventoryNum { get; set; }
        
        /// <summary>
        /// 采购价
        /// </summary>
        public decimal purchasePrice { get; set; }
        
        /// <summary>
        /// 采购数量
        /// </summary>
        public int purchaseNum { get; set; }
        
        /// <summary>
        /// 未到数量
        /// </summary>
        public int okNum { get; set; }
        
        /// <summary>
        /// 已到数量
        /// </summary>
        public int noNum { get; set; }
        
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal totalAmount { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string? remark { get; set; }
        
    }