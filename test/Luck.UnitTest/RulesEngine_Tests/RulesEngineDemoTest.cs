using System;
using System.Threading.Tasks;
using Luck.Framework.Extensions;
using RulesEngine.Models;
using Xunit;

namespace Luck.UnitTest.RulesEngine_Tests;

public class RulesEngineDemoTest
{
    [Fact]
    public async Task RulesEngineFactory()
    {
        var ruleJsonStr =
            """
            {
                "RuleName": "限制CA-330e138f16ec49d6a3ce4486a74d8a73",
                "Properties": {
                    "ConfigGuid": "330e138f16ec49d6a3ce4486a74d8a73",
                    "MonitorName": "限制CA"
                },
                "Operator": "OrElse",
                "ErrorMessage": "非自营供应商，必须传入供应商Id等参数",
                "Enabled": true,
                "ErrorType": "Warning",
                "RuleExpressionType": "LambdaExpression",
                "WorkflowRulesToInject": null,
                "Rules": [
                    {
                        "RuleName": "Rule_0",
                        "Properties": {
                            "Condition": {
                                "Relation": 0,
                                "SubConditions": [
                                    {
                                        "Left": {
                                            "ElementEnum": 9,
                                            "Value": "9",
                                            "DateType": 1,
                                            "ElementType": 1
                                        },
                                        "Operator": 3,
                                        "Right": {
                                            "ElementEnum": 30,
                                            "Value": "30",
                                            "DateType": 1,
                                            "ElementType": 1
                                        },
                                        "Extra": {
                                            "ElementEnum": -1,
                                            "Value": "CA",
                                            "DateType": 1,
                                            "ElementType": 2
                                        }
                                    }
                                ],
                                "ErrorMsg": "CA不允许，请重出"
                            }
                        },
                        "Operator": null,
                        "ErrorMessage": "CA不允许，请重出",
                        "Enabled": true,
                        "ErrorType": "Warning",
                        "RuleExpressionType": "LambdaExpression",
                        "WorkflowRulesToInject": null,
                        "Rules": null,
                        "LocalParams": null,
                        "Expression": "input.SupplierType == \"10110291001\"",
                        "Actions": null,
                        "SuccessEvent": null
                    }
                ],
                "LocalParams": null,
                "Expression": null,
                "Actions": null,
                "SuccessEvent": null
            }
            """;
        var test = "10110291001";
        var bre = new RulesEngine.RulesEngine();
        try
        {
            var rule = ruleJsonStr.Deserialize<Rule>();
            bre.AddWorkflow(new WorkflowRules()
            {
                WorkflowName = "限制CA",
                Rules = new[] { rule }
            });

            var inputStr =
                """
                {"Identifier":"CXYC005_YCS20250423130246E92B7JEN7451_136EBA52D98","OrderSerialId":"YCS20250423130246E92B7JEN7451","CustomerOrderSerialId":"YCS20250423130246E92B7JEN7451","SupplierOrderSerialId":"","CreateDate":"2025-04-23 13:13:18","BackDate":"2025-04-23 13:13:18","ProjectId":"10210101020","SecondProjectId":"70309","AccountingCompanyId":"1589","SupplierType":"10110291001","InnerPurchaseOrderSerialId":"","InnerPurchaseCompanyId":"","InnerPurchaseProjectId":"","InnerPurchaseSecondProjectId":"","SupplierName":"84027157S476388","SupplierId":"84027157","SceneryId":"","SceneryName":"","ContractNo":"","ResourceType":"C4334","ProductType":"C4334","Currency":"CNY","SaleAmount":2.0,"SaleCurrencyAmount":2.0,"CommissionAmount":0.0,"CommissionCurrencyAmount":0.0,"ContractAmount":4.0,"ContractCurrencyAmount":4.0,"SettlePeriod":"YJ","SettleMode":"HJ","SettleDate":"2025-04-23 13:13:18","AccountingMode":"10210231001","IsComplete":1,"PayType":null,"AccountId":null,"BusinessSerialId":"","PaymentSerialId":"","PNR":null,"CompleteDate":"","ElectronicTicketNo":null,"ConfirmState":"10110321004","ProductIdentifier":"600263","RefIdentifier":"","ChangeTypeId":null,"ProductForm":"","SettleCategoryId":"11020011016","PushType":"10510391001","SalesRefIdentifier":"CXYC001_YCS20250423130246E92B7JEN7451","RefundAmount":0.0,"RefundCurrencyAmount":0.0,"ReceiveSource":"","FlightPrice":0.0,"AirportBuildFee":0.0,"FuelTax":0.0,"TicketHandleAmount":0.0,"TicketHandleCurrencyAmount":0.0,"SaleType":"","RefundIdentifier":"","OrderType":"10110131001","NumOfProduct":1,"SupplierOrderExtend":null,"TicketType":"","RefOrderSerialId":"","CheckOrderSerialId":"","SaleCurrency":"","InnerAmount":0.0,"OrderIdentify":null,"SupplierOrderExtendTrain":null,"DepartDate":"","ConfirmDate":"2025-04-23 13:13:18","IsCreateCalculation":0,"RevenueDate":"2025-04-23 13:13:18","TicketChannel":"","IsReject":false,"TaxRate":0.0,"SupplierOrderExtendBus":null}
                """;
            var input = inputStr.Deserialize<SupplierOrderReceipt>();
            var result = await bre.ExecuteActionWorkflowAsync("限制CA", "限制CA-330e138f16ec49d6a3ce4486a74d8a73", new[]
            {
                new RuleParameter("input", input)
            });

            var sulemesssage = "";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}