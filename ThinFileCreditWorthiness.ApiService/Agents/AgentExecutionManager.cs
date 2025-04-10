using LoanMVP;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Data;
using System.Text.Json.Nodes;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class AgentExecutionManager
    {
        #pragma warning disable SKEXP0110
        #pragma warning disable SKEXP0001

        private List<Agent> agents = new();
        private readonly IConfiguration _configuration;
        private readonly BorrowerDataCollectionAgent _borrowerDataCollectionAgent;
        private readonly CreditDecisionAgent _creditDecisionAgent;
        private readonly FraudDetectionAgent _fraudDetectionAgent;
        private readonly PQIInspectorAgent _pqiInspectorAgent;
        private readonly BCIInspectorAgent _bciInspectorAgent;
        private readonly Kernel _kernel;
        public AgentExecutionManager(IConfiguration configuration, 
            Kernel kernel,
            BorrowerDataCollectionAgent borrowerDataCollectionAgent,
            FraudDetectionAgent fraudDetectionAgent,
            PQIInspectorAgent pqiInspectorAgent,
            BCIInspectorAgent bciInspectorAgent,
            CreditDecisionAgent creditDecisionAgent)
        {
            this._configuration = configuration;
            this._creditDecisionAgent = creditDecisionAgent;
            this._borrowerDataCollectionAgent = borrowerDataCollectionAgent;
            this._kernel = kernel;
            this._pqiInspectorAgent = pqiInspectorAgent;
            this._bciInspectorAgent = bciInspectorAgent;
            this._fraudDetectionAgent = fraudDetectionAgent;
        }

        public async Task BuildAgents(string inputData)
        {
            this._borrowerDataCollectionAgent.SetBorrowerData(inputData);
            var bcdAgent = await this._borrowerDataCollectionAgent.GetAgentAsync();
            agents.Add(bcdAgent);

            var fdAgent = await this._fraudDetectionAgent.GetAgentAsync();
            agents.Add(fdAgent);

            var pqiAgent = await this._pqiInspectorAgent.GetAgentAsync();
            agents.Add(pqiAgent);

            var bciAgent = await this._bciInspectorAgent.GetAgentAsync();
            agents.Add(bciAgent);

            var cdAgent = await this._creditDecisionAgent.GetAgentAsync();
            agents.Add(cdAgent);
        }

        public async Task<IAsyncEnumerable<ChatMessageContent>> ExecuteAsync(string userMessage)
        {
            var selectionStrategy = await GetSelectionStrategy(); // new EvaluationAgentSelectionStrategy();
            var groupChat = new AgentGroupChat(agents.ToArray())
            {
                ExecutionSettings = new AgentGroupChatSettings
                {
                    TerminationStrategy = new EvaluationTerminationStrategy
                    {
                        MaximumIterations = 100,
                        AutomaticReset = true,
                        Agents = agents.ToArray()
                    },
                    SelectionStrategy = selectionStrategy
                }
            };
            groupChat.AddChatMessage(new ChatMessageContent()
            {
                Role = AuthorRole.User,
                Content = userMessage,
            });

            // TODO: Remove
            //var pqi_score = new JsonObject();
            //pqi_score.Add("score_pqi", 600);
            //var bci_score = new JsonObject();
            //bci_score.Add("score_bci", 800);
            //groupChat.AddChatMessage(new ChatMessageContent { Content = "PQI Score is " + pqi_score.ToJsonString(), Role = AuthorRole.User });
            //groupChat.AddChatMessage(new ChatMessageContent { Content = "BCI Score is " + bci_score.ToJsonString(), Role = AuthorRole.User });
            
            return groupChat.InvokeAsync();

            //await foreach (var item in agent.InvokeAsync())
            //{
            //    await Task.Delay(15000);
            //    Console.WriteLine(item.Content);
            //}
        }

        // TODO: Rework on the selection strategy
        private async Task<KernelFunctionSelectionStrategy> GetSelectionStrategy()
        {
            var templateBasePath = _configuration["PromptTemplateBasePath"];
            var selectionPromptConfig = AgentHelperUtils.GetPromptTemplateConfig(templateBasePath, "AgentSelection.yaml");
            var selectionFunction = KernelFunctionFactory.CreateFromPrompt(selectionPromptConfig.Template);

            // TODO: Move this to enum
            var initalAgent = this.agents.FirstOrDefault(a => a.Name == "BorrowerDataCollectionAgent")
                ?? throw new InvalidOperationException($"Required agent BorrowerDataCollectionAgent not found.");

            // TODO: Identify the proper reduction strategy
            return new KernelFunctionSelectionStrategy(selectionFunction, this._kernel)
            {
                InitialAgent = initalAgent,
                HistoryReducer = new ChatHistoryTruncationReducer(10)
            };
        }
    }
}
