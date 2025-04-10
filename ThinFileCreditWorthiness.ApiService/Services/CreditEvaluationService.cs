using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using ThinFileCreditWorthiness.ApiService.Interfaces;
using ThinFileCreditWorthiness.ApiService.Agents;

namespace ThinFileCreditWorthiness.ApiService.Services
{
    #pragma warning disable SKEXP0110
    public class CreditEvaluationService : ICreditEvaluationService
    {
        private readonly AgentExecutionManager _executionManager;
        public CreditEvaluationService(AgentExecutionManager executionManager) 
        {
            this._executionManager = executionManager;
        }

        public async Task<IAsyncEnumerable<ChatMessageContent>> EvaluateCreditWorthinessAsync(string jsonData)
        {
            var userMessage = $"This is the details for loan evaluation: ${jsonData}";
            await this._executionManager.BuildAgents(jsonData);
            return await this._executionManager.ExecuteAsync(userMessage);
        }
    }
}
