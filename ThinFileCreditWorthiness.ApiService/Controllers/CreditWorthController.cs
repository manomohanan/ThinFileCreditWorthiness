using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ThinFileCreditWorthiness.ApiService.Models;
using ThinFileCreditWorthiness.ApiService.Services;
using static System.Net.WebRequestMethods;

namespace ThinFileCreditWorthiness.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditWorthController : ControllerBase
    {
        #pragma warning disable SKEXP0001
        
        //[HttpPost("evaluate")]
        //public async Task<IActionResult> EvaluteCreditWorth(IFormFile file)
        //{
        //    return Ok("");
        //}

        private readonly CreditEvaluationService _creditEvaluationService;
        private readonly ILogger<CreditWorthController> _logger;
        public CreditWorthController(CreditEvaluationService creditEvaluationService, ILogger<CreditWorthController> logger)
        {
            this._creditEvaluationService = creditEvaluationService;
            this._logger = logger;
        }

        [HttpPost("evaluate")]
        public async Task<IActionResult> EvaluteCreditWorth(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = new StreamReader(file.OpenReadStream());
            string jsonContent = await stream.ReadToEndAsync();

            // Deserialize to your model
            var collectionData = JsonSerializer.Deserialize<BorrowerData>(jsonContent);

            if (collectionData == null)
                return BadRequest("Unable to parse JSON.");

            int retries = 0;

            var result = await this._creditEvaluationService.EvaluateCreditWorthinessAsync(jsonContent);

            var messages = new List<CreditWorthResponseModel>();
            while (retries < 10)
            {
                try
                {
                    await foreach (var item in result)
                    {
                        //var buffer = Encoding.UTF8.GetBytes(item.Content ?? string.Empty);
                        //await Response.Body.WriteAsync(buffer, 0, buffer.Length);
                        //await Response.Body.FlushAsync();

                        //var message = $"{item.AuthorName}:{item.Content}";
                        this._logger.LogInformation(item.Content);
                        messages.Add(new CreditWorthResponseModel { Assistant = item.AuthorName, Message = item.Content });
                    }
                    break;
                }
                catch (Microsoft.SemanticKernel.HttpOperationException ex)
                {
                    if (ex.Message.StartsWith("HTTP 429"))
                    {
                        await Task.Delay(15000);
                        retries++;
                    }
                }
            }            

            return new OkObjectResult(messages); // response is already written to the body
        }
    }
}
