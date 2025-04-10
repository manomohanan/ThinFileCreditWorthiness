using Microsoft.AspNetCore.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;

namespace LoanMVP
{
    #pragma warning disable SKEXP0110
    public class EvaluationAgentSelectionStrategy : SelectionStrategy
    {
        private readonly ILogger _logger;
        public EvaluationAgentSelectionStrategy(ILogger logger)
        {
            _logger = logger;
        }

        protected override Task<Agent> SelectAgentAsync(IReadOnlyList<Agent> agents, IReadOnlyList<ChatMessageContent> history, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var lastMessage = history.Where(x => x.Role != AuthorRole.User).LastOrDefault()?.Content?.ToString() ?? string.Empty;
                var targetAgentName = string.IsNullOrEmpty(lastMessage) ? "BorrowerDataCollectionAgent" : lastMessage.Substring(1, lastMessage.IndexOf(']') - 1);
                var agent = agents.First(x => x.Name == targetAgentName);

                this._logger.LogInformation($"Selected agent: {agent.Name} for message: {lastMessage}");
                return agent;
            });
        }
    }
}
