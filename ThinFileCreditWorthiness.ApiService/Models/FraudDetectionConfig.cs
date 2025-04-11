namespace ThinFileCreditWorthiness.ApiService.Models
{
    public class FraudDetectionConfig
    {
        public IdentityVerification identityverification { get; set; }
        public IncomeVerification incomeverification { get; set; }
        public CreditScoreAnomalies creditscoreanomalies { get; set; }
        public PropertyValuationChecks propertyvaluationchecks { get; set; }
        public MultiLoanActivity multiloanactivity { get; set; }
        public GeolocationMismatch geolocationmismatch { get; set; }
        public DocumentTampering documenttampering { get; set; }
        public HistoricalDisasterScoreCheck historicaldisasterscorecheck { get; set; }
    }

    public class IdentityVerification
    {
        public int mismatchthreshold { get; set; }
        public bool duplicateentriescheck { get; set; }
    }

    public class IncomeVerification
    {
        public int incomesuddenincreasethreshold { get; set; }
        public bool employmentverificationrequired { get; set; }
    }

    public class CreditScoreAnomalies
    {
        public int multipleinquiriesthreshold { get; set; }
        public int suddencreditscoreincrease { get; set; }
    }

    public class PropertyValuationChecks
    {
        public int overstatedvaluethreshold { get; set; }
        public bool unverifiedvaluationreports { get; set; }
    }

    public class MultiLoanActivity
    {
        public int loanapplicationcountthreshold { get; set; }
        public int timeperioddays { get; set; }
    }

    public class GeolocationMismatch
    {
        public int allowedvariancekm { get; set; }
    }

    public class DocumentTampering
    {
        public bool imagemetadataverification { get; set; }
        public bool digitalsignaturemandatory { get; set; }
    }

    public class HistoricalDisasterScoreCheck
    {
        public int highriskthreshold { get; set; }
        public int previousclaimthreshold { get; set; }
    }

}
