namespace Indspire.Soaring.Engagement.ViewModels
{
    public class LogActionJsonViewModel: JsonBaseViewModel
    {
        public LogActionJsonViewModel(int pointsAwarded, int pointsBalance, string userNumber)
        {
            ResponseData = new LogActionResponseData();
            ResponseData.PointsAwarded = pointsAwarded;
            ResponseData.PointsBalance = pointsBalance;
            ResponseData.UserNumber = userNumber;
        }
        public LogActionJsonViewModel() { }
        public LogActionResponseData ResponseData = new LogActionResponseData();
    }   
}
