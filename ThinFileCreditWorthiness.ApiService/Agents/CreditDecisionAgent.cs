using Microsoft.SemanticKernel;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class CreditDecisionAgent : AgentBase
    {
        private readonly IConfiguration _configuration;
        private readonly Kernel _kernel;
        public CreditDecisionAgent(IConfiguration configuration, Kernel kernel) : base(configuration, kernel, "CreditDecisionAgent")
        {
            this._configuration = configuration;
            this._kernel = kernel;
        }
        public override Task<KernelArguments> GetKernelArgsAsync()
        {
            var configBasePath = _configuration["AgentConfigPath"];
            var agentConfig = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, configBasePath, "Data.json"));
            
            var config = JsonSerializer.Deserialize<CreditDecisionConfig>(agentConfig);
            var settings = new PromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };
            var args = new KernelArguments(settings)
            {
                { "credit_decision_config", JsonSerializer.Serialize(config) }
            };
            return Task.FromResult(args);
        }
    }
}
