namespace Indspire.Soaring.Engagement.ViewModels
{
    using Indspire.Soaring.Engagement.Database;

    public class AwardScanViewModel
    {
        public string AwardNumber { get; set; } = string.Empty;

        public bool HasAwardNumber { get; set; } = false;

        public bool AwardNumberInvalid { get; set; } = false;

        public Award Award { get; set; }
    }
}
