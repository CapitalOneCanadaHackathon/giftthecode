namespace Indspire.Soaring.Engagement.ViewModels
{
    public class LogRedemptionJsonViewModel : JsonBaseViewModel
    {
        public LogRedemptionJsonViewModel()
        {
            ResponseData = new LogRedemptionResponseData();
        }

        public LogRedemptionResponseData ResponseData = new LogRedemptionResponseData();
    }
}
