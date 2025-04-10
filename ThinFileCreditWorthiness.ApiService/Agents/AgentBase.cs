using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public abstract class AgentBase
    {
        private readonly IConfiguration _configuration;
        private readonly Kernel _kernel;
        private readonly string _agentName;
        protected AgentBase(IConfiguration configuration, Kernel kernel, string agentName)
        {
            this._configuration = configuration;
            this._kernel = kernel;
            this._agentName = agentName;
        }

        public abstract Task<KernelArguments> GetKernelArgsAsync();

        public async Task<ChatCompletionAgent> GetAgentAsync()
        {
            var templateBasePath = _configuration["PromptTemplateBasePath"];
            var promptTemplateConfig = AgentHelperUtils.GetPromptTemplateConfig(templateBasePath, $"{this._agentName}.yaml");
            var templateFactory = new KernelPromptTemplateFactory();
            var template = templateFactory.Create(promptTemplateConfig);

            var args = await this.GetKernelArgsAsync();
            var instruction = await template.RenderAsync(this._kernel, args);
            return new ChatCompletionAgent()
            {
                Instructions = instruction,
                Name = this._agentName,
                Kernel = this._kernel,
                Arguments = args
            };
        }
    }
}
