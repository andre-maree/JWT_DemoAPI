using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace BankHolidaysFunctionApp
{
    public static class BankHolidaysFunction
    {
        [FunctionName("BankHolidaysFunction")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            RetryOptions ro = new(TimeSpan.FromSeconds(10), 100);

            try
            {
                await context.CallActivityWithRetryAsync<string>(nameof(SaveBankHolidaysActivity), ro, null);
            }
            catch (Exception ex)
            {
                //log exception after all retries failed and continue
            }

#if DEBUG
            await context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(15), default);
#else
            await context.CreateTimer(context.CurrentUtcDateTime.AddHours(12), default);
#endif

            context.ContinueAsNew(null);
        }

        [FunctionName(nameof(SaveBankHolidaysActivity))]
        public static async Task SaveBankHolidaysActivity([ActivityTrigger] ILogger log)
        {
            await BankHolidaysDA.SaveBankHolidays();
        }

        [FunctionName("BankHolidaysFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = "BankHolidaysFunction";
            DurableOrchestrationStatus resp = await starter.GetStatusAsync(instanceId);

            if (resp != null)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Conflict) { Content = new StringContent($"The process state is: {resp.RuntimeStatus}.") };
            }

            await starter.StartNewAsync("BankHolidaysFunction", instanceId);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        [FunctionName(nameof(ClearInstanceHistory))]
        public static async Task ClearInstanceHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            await starter.PurgeInstanceHistoryAsync("BankHolidaysFunction");
        }
    }
}