using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    #pragma warning disable SKEXP0110
    public class EvaluationTerminationStrategy : TerminationStrategy
    {
        protected override Task<bool> ShouldAgentTerminateAsync(Agent agent, IReadOnlyList<ChatMessageContent> history, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                // TODO: Take this from an Enum
                return agent.Name == "BorrowerDataCollectionAgent" && (history.LastOrDefault()?.Content?.Contains("[EVALUATIONCOMPLETE]") ?? false);
            });
        }
    }
}
