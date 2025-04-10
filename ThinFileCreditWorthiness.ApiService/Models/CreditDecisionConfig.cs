using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ThinFileCreditWorthiness.ApiService.Models;
public class CreditDecisionConfig
{
    //[JsonPropertyName("scoringMethod")]
    //public string ScoringMethod { get; set; }

    //[JsonPropertyName("description")]
    //public string Description { get; set; }

    [JsonPropertyName("configuration")]
    public Configuration Configuration { get; set; }

    //[JsonPropertyName("additionalMeasures")]
    //public List<AdditionalMeasure> AdditionalMeasures { get; set; }

    //[JsonPropertyName("componentFormulas")]
    //public ComponentFormulas ComponentFormulas { get; set; }

    [JsonPropertyName("finalCompositeFormula")]
    public string FinalCompositeFormula { get; set; }

    //[JsonPropertyName("explanation")]
    //public string Explanation { get; set; }
}

public class Configuration
{
    [JsonPropertyName("weights")]
    public Weights Weights { get; set; }

    [JsonPropertyName("scoreRange")]
    public ScoreRange ScoreRange { get; set; }
}

public class Weights
{
    [JsonPropertyName("TraditionalScore")]
    public double TraditionalScore { get; set; }

    [JsonPropertyName("BCI")]
    public double BCI { get; set; }

    [JsonPropertyName("PQI")]
    public double PQI { get; set; }
}

public class ScoreRange
{
    [JsonPropertyName("min")]
    public int Min { get; set; }

    [JsonPropertyName("max")]
    public int Max { get; set; }
}

public class AdditionalMeasure
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("formula")]
    public string Formula { get; set; }

    [JsonPropertyName("notes")]
    public string Notes { get; set; }
}

public class ComponentFormulas
{
    [JsonPropertyName("BCI")]
    public FormulaDetails BCI { get; set; }

    [JsonPropertyName("PQI")]
    public FormulaDetails PQI { get; set; }
}

public class FormulaDetails
{
    [JsonPropertyName("formula")]
    public string Formula { get; set; }

    [JsonPropertyName("details")]
    public Details Details { get; set; }
}

public class Details
{
    [JsonPropertyName("TraditionalComponent")]
    public string TraditionalComponent { get; set; }

    [JsonPropertyName("AlternateComponent")]
    public string AlternateComponent { get; set; }

    [JsonPropertyName("weightExplanation")]
    public string WeightExplanation { get; set; }

    [JsonPropertyName("LocationScore")]
    public string LocationScore { get; set; }

    [JsonPropertyName("StructuralScore")]
    public string StructuralScore { get; set; }

    [JsonPropertyName("MarketTrendScore")]
    public string MarketTrendScore { get; set; }

    [JsonPropertyName("DisasterRiskScore")]
    public string DisasterRiskScore { get; set; }
}
