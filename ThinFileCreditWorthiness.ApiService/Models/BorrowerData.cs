using System.Text.Json.Serialization;

public class BorrowerData
{
    [JsonPropertyName("borrower_id")]
    public string BorrowerId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("employment_status")]
    public EmploymentType? EmploymentStatus { get; set; }

    [JsonPropertyName("monthly_income")]
    public int MonthlyIncome { get; set; }

    [JsonPropertyName("existing_loans")]
    public int ExistingLoans { get; set; }

    [JsonPropertyName("credit_score")]
    public int CreditScore { get; set; }

    [JsonPropertyName("property_details")]
    public PropertyDetails PropertyDetails { get; set; }

    [JsonPropertyName("additional_tags")]
    public AdditionalTags AdditionalTags { get; set; }

    [JsonPropertyName("geo_location")]
    public GeoSpatialInfo GeoSpatial { get; set; }
}

public class PropertyDetails
{
    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("market_value")]
    public int? MarketValue { get; set; }

    [JsonPropertyName("structural_condition")]
    public StructuralCondition StructuralCondition { get; set; }

    [JsonPropertyName("location_risk")]
    public RiskLevel LocationRisk { get; set; }

    [JsonPropertyName("disaster_score")]
    public int? DisasterScore { get; set; }

    [JsonPropertyName("previous_claims")]
    public string PreviousClaims { get; set; }
}

public class AdditionalTags
{
    [JsonPropertyName("risk_bucket")]
    public string RiskBucket { get; set; }

    [JsonPropertyName("segment")]
    public string Segment { get; set; }

    [JsonPropertyName("is_first_time_buyer")]
    public bool IsFirstTimeBuyer { get; set; }
}

public class GeoSpatialInfo
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentType
{
    Unemployed = 0,
    Salaried = 1,
    SelfEmployed = 2
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StructuralCondition
{
    Poor = 1,
    Average = 5,
    Good = 10
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskLevel
{
    Low = 1,
    Medium = 5,
    High = 10
}