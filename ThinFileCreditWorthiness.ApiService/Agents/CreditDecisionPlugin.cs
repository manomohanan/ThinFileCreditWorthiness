using Microsoft.SemanticKernel;
using System.ComponentModel;
using ThinFileCreditWorthiness.ApiService.Models;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class CreditDecisionPlugin
    {
        [KernelFunction("EvaluateCreditScore")]
        [Description("Evaluate credit score using the property quality index (PQI) and borrower creditworthiness index (BCI) and traditional score")]
        public async Task<int> EvaluateCreditScore(int pqiScore, int bciScore, int traditionalScore, Weights weights)
        {
            Console.WriteLine("Calculating Credit Score");
            Console.WriteLine($"Params received : PQI-{pqiScore}, BCI-{bciScore}, Traditional: {traditionalScore}");
            Console.WriteLine($"Weights received : PQI-{weights.PQI}, BCI-{weights.BCI}, Traditional: {weights.TraditionalScore}");

            var weightedScore = (weights.TraditionalScore * traditionalScore) + (weights.BCI * bciScore) + (weights.PQI * pqiScore);
            var creditScore = 300 + (weightedScore - 300) / (850 - 300);
            int result = (int)Math.Round(creditScore, 0);
            return result;
        }
    }
}
