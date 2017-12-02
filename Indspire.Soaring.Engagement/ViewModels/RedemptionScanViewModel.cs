using Indspire.Soaring.Engagement.Database;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class RedemptionScanViewModel
    {
        public string RedemptionNumber { get; set; }
        public bool HasRedemptionNumber { get; set; }
        public bool RedemptionNumberInvalid { get; set; }
        public Redemption Redemption { get; set; }
    }
}
