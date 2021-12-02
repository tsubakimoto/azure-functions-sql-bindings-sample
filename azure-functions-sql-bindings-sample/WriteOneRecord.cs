// https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-azure-sql-output?tabs=csharp

using azure_functions_sql_bindings_sample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace azure_functions_sql_bindings_sample
{
    public static class WriteOneRecord
    {
        [FunctionName("WriteOneRecord")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "addtodo")] HttpRequest req,
            ILogger log,
            [Sql("dbo.ToDo", ConnectionStringSetting = "SqlConnectionString")] out ToDoItem newItem)
        {
            newItem = new ToDoItem
            {
                Id = req.Query["id"],
                Description = req.Query["desc"]
            };

            log.LogInformation($"C# HTTP trigger function inserted one row");
            return new CreatedResult($"/api/addtodo", newItem);
        }

        [FunctionName("WriteRecordsAsync")]
        public static async Task<IActionResult> Run2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "addtodo-asynccollector")] HttpRequest req,
            ILogger log,
            [Sql("dbo.ToDo", ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<ToDoItem> newItems)
        {
            await newItems.AddAsync(new ToDoItem
            {
                Id = DateTime.UtcNow.Millisecond.ToString(),
                Description = DateTime.UtcNow.ToString()
            });
            await newItems.AddAsync(new ToDoItem
            {
                Id = (DateTime.UtcNow.Millisecond + 100).ToString(),
                Description = DateTime.UtcNow.AddDays(1).ToString()
            });
            // Rows are upserted here
            await newItems.FlushAsync();

            return new CreatedResult($"/api/addtodo-asynccollector", "done");
        }
    }
}
