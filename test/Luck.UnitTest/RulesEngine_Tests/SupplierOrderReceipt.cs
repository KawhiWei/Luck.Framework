namespace Luck.UnitTest.RulesEngine_Tests;

public class SupplierOrderReceipt
{
    /// <summary>
    /// 单据号
    /// </summary>

    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 订单号
    /// </summary>

    public string OrderSerialId { get; set; } = string.Empty;

    /// <summary>
    /// 对客订单号
    /// </summary>

    public string CustomerOrderSerialId { get; set; } = string.Empty;

    /// <summary>
    /// 供应商订单号（白金会员任务添加）
    /// </summary>

    public string SupplierOrderSerialId { get; set; } = string.Empty;

    /// <summary>
    /// 创建日期
    /// </summary>

    public string CreateDate { get; set; } = string.Empty;

    /// <summary>
    /// 出票时间(回团日期)
    /// </summary>

    public string BackDate { get; set; } = string.Empty;

    /// <summary>
    /// 来源项目
    /// </summary>

    public string ProjectId { get; set; } = "10210101007";

    /// <summary>
    /// 明细来源项目
    /// </summary>

    public string SecondProjectId { get; set; } = "502501";

    /// <summary>
    /// 采购主体
    /// </summary>

    public string AccountingCompanyId { get; set; } = string.Empty;

    /// <summary>
    /// 供应商类型
    /// </summary>

    public string SupplierType { get; set; } = string.Empty;

    /// <summary>
    /// 内部往来订单号
    /// </summary>

    public string InnerPurchaseOrderSerialId { get; set; } = string.Empty;

    /// <summary>
    /// 内部往来主体
    /// </summary>

    public string InnerPurchaseCompanyId { get; set; } = string.Empty;

    /// <summary>
    /// 往来项目
    /// </summary>

    public string InnerPurchaseProjectId { get; set; } = string.Empty;

    /// <summary>
    /// 往来明细项目
    /// </summary>

    public string InnerPurchaseSecondProjectId { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>

    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>

    public string SupplierId { get; set; } = string.Empty;

    /// <summary>
    /// 品牌id
    /// </summary>

    public string SceneryId { get; set; } = string.Empty;

    /// <summary>
    /// 品牌名称
    /// </summary>

    public string SceneryName { get; set; } = string.Empty;

    /// <summary>
    /// 协议号
    /// </summary>

    public string ContractNo { get; set; } = string.Empty;

    /// <summary>
    /// 资源类型
    /// </summary>

    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// 产品类型
    /// </summary>

    public string ProductType { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>

    public string Currency { get; set; } = "CNY";

    /// <summary>
    /// 网上价
    /// </summary>

    public decimal SaleAmount { get; set; }

    /// <summary>
    /// 网上价-本币
    /// </summary>

    public decimal SaleCurrencyAmount { get; set; }

    /// <summary>
    /// 佣金
    /// </summary>

    public decimal CommissionAmount { get; set; }

    /// <summary>
    /// 佣金-本币
    /// </summary>

    public decimal CommissionCurrencyAmount { get; set; }

    /// <summary>
    /// 结算总价
    /// </summary>

    public decimal ContractAmount { get; set; }

    /// <summary>
    /// 结算总价-本币
    /// </summary>

    public decimal ContractCurrencyAmount { get; set; }

    /// <summary>
    /// 结算周期
    /// </summary>

    public string SettlePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 结算模式
    /// </summary>

    public string SettleMode { get; set; } = string.Empty;

    /// <summary>
    /// 计划结算日期
    /// </summary>

    public string SettleDate { get; set; } = string.Empty;

    /// <summary>
    /// 与供应商签订的合作模式，值集有：1-收入成本模式2-佣金模式3-收入费用模式
    /// </summary>

    public string AccountingMode { get; set; } = string.Empty;

    /// <summary>
    /// 是否已支付
    /// </summary>

    public int IsComplete { get; set; }

    /// <summary>
    /// 支付方式
    /// </summary>

    public string PayType { get; set; } = string.Empty;

    /// <summary>
    /// 支付账号
    /// </summary>

    public string AccountId { get; set; } = string.Empty;

    /// <summary>
    /// 商户流水号
    /// </summary>

    public string BusinessSerialId { get; set; } = string.Empty;

    /// <summary>
    /// 业务流水号
    /// </summary>

    public string PaymentSerialId { get; set; } = string.Empty;

    /// <summary>
    /// PNR
    /// </summary>

    public string PNR { get; set; } = string.Empty;

    /// <summary>
    /// 支付时间
    /// </summary>

    public string CompleteDate { get; set; } = string.Empty;

    /// <summary>
    /// 电子客票号/副营产品券号
    /// </summary>

    public string ElectronicTicketNo { get; set; } = string.Empty;

    /// <summary>
    /// 核单状态
    /// </summary>

    public string ConfirmState { get; set; } = string.Empty;

    /// <summary>
    /// 销售单行id
    /// </summary>

    public string ProductIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// 关联单据号
    /// </summary>

    public string RefIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// AB单，升舱换开，急速改签，免费改签
    /// </summary>

    public string ChangeTypeId { get; set; } = string.Empty;

    /// <summary>
    /// 产品形态
    /// </summary>

    public string ProductForm { get; set; } = "10110421001";

    /// <summary>
    /// 结算分类
    /// </summary>

    public string SettleCategoryId { get; set; } = string.Empty;

    /// <summary>
    /// 推送数据分类
    /// </summary>

    public string PushType { get; set; } = "10510391001";

    /// <summary>
    /// 结算单对应的销售单的单据号
    /// </summary>

    public string SalesRefIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// 强退需要传入退票费
    /// </summary>

    public decimal RefundAmount { get; set; }

    /// <summary>
    /// 退票费-本币
    /// </summary>

    public decimal RefundCurrencyAmount { get; set; }

    /// <summary>
    /// 产生应收类型【强退，重复支付，退款，升舱换开，急速改签】
    /// </summary>

    public string ReceiveSource { get; set; } = string.Empty;

    /// <summary>
    /// 航信价
    /// </summary>

    public decimal FlightPrice { get; set; }

    /// <summary>
    /// 基建费
    /// </summary>

    public decimal AirportBuildFee { get; set; }

    /// <summary>
    /// 燃油费
    /// </summary>

    public decimal FuelTax { get; set; }

    /// <summary>
    /// 出票处理费
    /// </summary>

    public decimal TicketHandleAmount { get; set; }

    /// <summary>
    /// 出票处理费-本币
    /// </summary>

    public decimal TicketHandleCurrencyAmount { get; set; }

    /// <summary>
    /// 打包分类
    /// </summary>

    public string SaleType { get; set; } = string.Empty;

    /// <summary>
    /// 退款单号-负结算单推送
    /// 目前只有退改无忧险存在
    /// add by sxf39200
    /// </summary>

    public string RefundIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// 订单变更类型
    /// 10110131001  普通订单  10 客人支付成功后推送订单 类型为普通 编辑日志
    /// 10110131002  变更(非退款)  20 非退款导致的变更单，按创建时间入账 编辑日志
    /// 10110131003  变更(退款)
    /// add by sxf39200
    /// </summary>

    public string OrderType { get; set; } = string.Empty;

    /// <summary>
    /// 产品数量
    /// </summary>

    public int NumOfProduct { get; set; }

    /// <summary>
    /// 供应信息扩展（白金会员任务添加）
    /// </summary>

    public SupplierExtend SupplierOrderExtend { get; set; } = new SupplierExtend();

    /// <summary>
    /// 客票类型(儿童拆单任务添加)
    /// </summary>

    public string TicketType { get; set; } = string.Empty;

    /// <summary>
    /// 关联订单号(金蟾新增分片字段任务添加)
    /// </summary>

    public string RefOrderSerialId { get; set; } = string.Empty;

    public class SupplierExtend
    {
        /// <summary>
        /// 出港地名称
        /// </summary>

        public string DepartureName { get; set; } = string.Empty;

        /// <summary>
        /// 出港地三字码
        /// </summary>

        public string DepartureId { get; set; } = string.Empty;

        /// <summary>
        /// 下单资源名称
        /// </summary>

        public string ResourceName { get; set; } = string.Empty;

        /// <summary>
        /// 产品名称
        /// </summary>

        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 起飞时间
        /// </summary>

        public string TakeOffDate { get; set; } = string.Empty;

        /// <summary>
        /// 抵达地
        /// </summary>

        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// 舱位
        /// </summary>

        public string AircraftSpace { get; set; } = string.Empty;

        /// <summary>
        /// 航班号
        /// </summary>

        public string FlightNo { get; set; } = string.Empty;

        /// <summary>
        /// 折扣
        /// </summary>

        public string Discount { get; set; } = string.Empty;

        /// <summary>
        /// 代理供应商
        /// </summary>

        public string RealSupplierId { get; set; } = string.Empty;

        /// <summary>
        /// 标签
        /// </summary>

        public string LabelType { get; set; } = string.Empty;

        /// <summary>
        /// 关联账扣销售单唯一号
        /// </summary>

        public string CustomerRebateUniqueNumber { get; set; } = string.Empty;

        /// <summary>
        /// 卡券面值
        /// </summary>

        public decimal CardWorthAmount { get; set; }
    }

    /// <summary>
    /// 核账订单号
    /// </summary>

    public string CheckOrderSerialId { get; set; } = string.Empty;

    /// <summary>
    /// 销售单币种
    /// </summary>

    public string SaleCurrency { get; set; } = "CNY";

    /// <summary>
    /// 内销成本价
    /// </summary>

    public decimal InnerAmount { get; set; }

    /// <summary>
    /// 订单标识
    /// </summary >

    public string OrderIdentify { get; set; } = string.Empty;

    /// <summary>
    /// 火车票扩展信息
    /// </summary>

    public SupplierExtendTrain SupplierOrderExtendTrain { get; set; } = new();

    public class SupplierExtendTrain
    {
        /// <summary>
        /// 出发时间
        /// </summary>

        public string DepartDate { get; set; } = string.Empty;

        /// <summary>
        /// 车次
        /// </summary>

        public string TrainNo { get; set; } = string.Empty;

        /// <summary>
        /// 出发省
        /// </summary>

        public string DepartProvince { get; set; } = string.Empty;

        /// <summary>
        /// 出发城市
        /// </summary>

        public string DepartCity { get; set; } = string.Empty;

        /// <summary>
        /// 出发站
        /// </summary>

        public string DepartStation { get; set; } = string.Empty;

        /// <summary>
        /// 抵达省
        /// </summary>

        public string ArriveProvince { get; set; } = string.Empty;

        /// <summary>
        /// 抵达市
        /// </summary>

        public string ArriveCity { get; set; } = string.Empty;

        /// <summary>
        /// 抵达站点
        /// </summary>

        public string ArriveStation { get; set; } = string.Empty;

        /// <summary>
        /// 确认时间
        /// </summary>

        public string ConfirmDate { get; set; } = string.Empty;

        /// <summary>
        /// 行程信息
        /// </summary>

        public int DrivingDistanceNo { get; set; }
    }

    /// <summary>
    /// 出发时间
    /// </summary>

    public string DepartDate { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间
    /// </summary>

    public string ConfirmDate { get; set; } = string.Empty;

    /// <summary>
    /// 是否可取消 2 可取消 3 不可取消 
    /// </summary>

    public int IsCreateCalculation { get; set; }

    /// <summary>
    /// 收入成本确认日期 
    /// </summary>

    public string RevenueDate { get; set; } = string.Empty;

    /// <summary>
    /// 出票渠道
    /// </summary>

    public string TicketChannel { get; set; } = string.Empty;

    /// <summary>
    /// 是否驳回单
    /// </summary>

    public bool IsReject { get; set; }

    /// <summary>
    /// 租车扩展信息
    /// </summary>
    public SupplierExtendBus SupplierOrderExtendBus { get; set; } = new();

    public class SupplierExtendBus
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public string DepartureName { get; set; } = string.Empty;

        /// <summary>
        /// 出发地三字码
        /// </summary>
        public string DepartureId { get; set; } = string.Empty;

        /// <summary>
        /// 下单资源名称
        /// </summary>
        public string ResourceName { get; set; } = string.Empty;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 发车时间
        /// </summary>
        public string DepartDate { get; set; } = string.Empty;

        /// <summary>
        /// 到达地
        /// </summary>
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// 座位号
        /// </summary>
        public string SeatNo { get; set; } = string.Empty;

        /// <summary>
        /// 车次号
        /// </summary>
        public string TrainNo { get; set; } = string.Empty;

        /// <summary>
        /// 出发地省份
        /// </summary>
        public string DepartProvince { get; set; } = string.Empty;

        /// <summary>
        /// 出发地城市
        /// </summary>
        public string DepartCity { get; set; } = string.Empty;

        /// <summary>
        /// 出发站点
        /// </summary>
        public string DepartStation { get; set; } = string.Empty;

        /// <summary>
        /// 出票价
        /// </summary>
        public decimal TicketPrice { get; set; }

        /// <summary>
        /// 到达地城市
        /// </summary>
        public string ArriveCity { get; set; } = string.Empty;

        /// <summary>
        /// 第三方
        /// </summary>
        public string ThirdParty { get; set; } = string.Empty;
    }
}