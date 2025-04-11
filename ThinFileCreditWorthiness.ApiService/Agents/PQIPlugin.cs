using Microsoft.SemanticKernel;
using System.ComponentModel;
using ThinFileCreditWorthiness.ApiService.Models;

namespace ThinFileCreditWorthiness.ApiService.Agents
{
    public class PQIPlugin
    {
        [KernelFunction("EvaluatePQIScore")]
        [Description("Calculate property quality index (PQI) using LocationScore, StructuralScore, MarketTrendScore , DisasterRiskScore, LocationScoreWeight,StructuralScoreWeight, MarketTrendScoreWeight , DisasterRiskScoreWeight")]
        public async Task<int> EvaluatePQIScore(double locationScore, double structuralScore, double marketTrendScore, double disasterRiskScore,
            double locationScoreWeight= 0.25, double structuralScoreWeight = 0.25, double marketTrendScoreWeight = 0.25, double disasterRiskScoreWeight = 0.25)
        {
            return await Task.Run(() =>
            {
                Console.WriteLine("Calculating PQI Score");
                Console.WriteLine($"Params received : PQI-{locationScore}, Structural-{structuralScore}, Market: {marketTrendScore}, Disaster: {disasterRiskScore}");
                Console.WriteLine($"Params Weightreceived : PQI-{locationScoreWeight}, Structural-{structuralScoreWeight}, Market: {marketTrendScoreWeight}, Disaster: {disasterRiskScoreWeight}");


                // Example calculation logic (adjust as needed)
                var pqiScore = (locationScore * locationScoreWeight) + (structuralScore * structuralScoreWeight) + (marketTrendScore * marketTrendScoreWeight) + (disasterRiskScore * disasterRiskScoreWeight);

                int result = (int)Math.Round(pqiScore, 0);
                return result;
            });
        }
    }
}
