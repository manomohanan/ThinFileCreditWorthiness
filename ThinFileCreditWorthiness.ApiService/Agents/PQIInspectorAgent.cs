using Microsoft.SemanticKernel;
using System.Text.Json;
using ThinFileCreditWorthiness.ApiService.Models;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class PQIInspectorAgent : AgentBase
    {
        private readonly IConfiguration _configuration;
        private readonly Kernel _kernel;
        private string _borrowerProfile;
        public PQIInspectorAgent(IConfiguration configuration, Kernel kernel) : base(configuration, kernel, "PQIInspectorAgent")
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
            
            var config = JsonSerializer.Deserialize<PQIConfig>(agentConfig);
            var args = new KernelArguments()
            {
                { "borrowerProfile", JsonSerializer.Serialize(this._borrowerProfile) },
                { "pqi_config", JsonSerializer.Serialize(config) }
            };

            return Task.FromResult(args);
        }
    }
}
