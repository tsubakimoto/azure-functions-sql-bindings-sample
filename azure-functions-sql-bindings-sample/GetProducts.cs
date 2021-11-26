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
    public class GetProducts
    {
        private readonly ILogger<GetProducts> _logger;

        public GetProducts(ILogger<GetProducts> log)
        {
            _logger = log;
        }

        // Get all products.
        [FunctionName("GetAllProducts")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult GetAll(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Sql("select * from SalesLT.Product order by ProductId asc",
                CommandType = System.Data.CommandType.Text,
                Parameters = "",
                ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Product> products)
        {
            _logger.LogInformation("GetAllProducts is running.");
            return new OkObjectResult(products);
        }

        // Get products by specifying the color.
        [FunctionName("GetProducts")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "color", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Color** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Sql("select * from SalesLT.Product where Color = @Color order by ProductId asc",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@Color={Query.color}",
                ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Product> products)
        {
            _logger.LogInformation("GetProducts is running.");
            return new OkObjectResult(products);
        }
    }
}

