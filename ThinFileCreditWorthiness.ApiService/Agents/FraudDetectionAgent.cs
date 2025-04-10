using Microsoft.SemanticKernel;
using System.Text.Json;
using ThinFileCreditWorthiness.ApiService.Models;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class FraudDetectionAgent : AgentBase
    {
        private readonly IConfiguration _configuration;
        private readonly Kernel _kernel;
        private string _borrowerProfile;
        public FraudDetectionAgent(IConfiguration configuration, Kernel kernel) : base(configuration, kernel, "FraudDetectionAgent")
        {
            this._configuration = configuration;
            this._kernel = kernel;
        }

        public void SetBorrowerData(string borrowerProfile)
        {
            this._borrowerProfile = borrowerProfile;
        }

        public override Task<KernelArguments> GetKernelArgsAsync()
        {
            var configBasePath = _configuration["AgentConfigPath"];
            var agentConfig = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, configBasePath, "Data.json"));
            
            var config = JsonSerializer.Deserialize<CreditDecisionConfig>(agentConfig);
            var args = new KernelArguments()
            {
                //{ "borrowerProfile", JsonSerializer.Serialize(this._borrowerProfile) }
            };

            return Task.FromResult(args);
        }
    }
}
