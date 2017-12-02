using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class CheckBalanceJsonViewModel: JsonBaseViewModel
    {
        

        public CheckBalanceJsonViewModel(int pointsAwarded, int pointsBalance, string userNumber)
        {
            ResponseData = new CheckBalanceResponseData();
            ResponseData.PointsBalance = pointsBalance;
            ResponseData.UserNumber = userNumber;
        }
        public CheckBalanceJsonViewModel() { }
        public CheckBalanceResponseData ResponseData = new CheckBalanceResponseData();
    }
}
