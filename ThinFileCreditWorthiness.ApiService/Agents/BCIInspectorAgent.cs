using Microsoft.SemanticKernel;
using System.Text.Json;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class BCIInspectorAgent : AgentBase
    {
        private readonly IConfiguration _configuration;
        private readonly Kernel _kernel;
        private string _borrowerProfile;
        public BCIInspectorAgent(IConfiguration configuration, Kernel kernel) : base(configuration, kernel, "BCIInspectorAgent")
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
