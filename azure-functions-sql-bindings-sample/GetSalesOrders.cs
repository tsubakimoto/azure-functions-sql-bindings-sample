using azure_functions_sql_bindings_sample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Net;

namespace azure_functions_sql_bindings_sample
{
    public class GetSalesOrders
    {
        private readonly ILogger<GetSalesOrders> _logger;

        public GetSalesOrders(ILogger<GetSalesOrders> log)
        {
            _logger = log;
        }

        private const string COMMAND_TEXT =
@"SELECT H.SalesOrderID, D.SalesOrderDetailId, D.OrderQty
	FROM SalesLT.SalesOrderHeader H
	INNER JOIN SalesLT.SalesOrderDetail D
        ON (H.SalesOrderID = D.SalesOrderID)
    WHERE H.SalesOrderID = @Id";

        [FunctionName("GetSalesOrder")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **ID** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetSalesOrder/{id}")] HttpRequest req,
            [Sql(COMMAND_TEXT,
                CommandType = System.Data.CommandType.Text,
                Parameters = "@Id={id}",
                ConnectionStringSetting = "SqlConnectionString")] IEnumerable<SalesOrder> salesOrders)
        {
            _logger.LogInformation("GetSalesOrders is running.");
            return new OkObjectResult(salesOrders);
        }
    }
}
