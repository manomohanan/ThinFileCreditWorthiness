using Microsoft.SemanticKernel;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class AgentHelperUtils
    {
        public static PromptTemplateConfig GetPromptTemplateConfig(string templateBasePath, string templateName)
        {
            var templateContent = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, templateBasePath, templateName));
            return KernelFunctionYaml.ToPromptTemplateConfig(templateContent);
        }
    }
}
