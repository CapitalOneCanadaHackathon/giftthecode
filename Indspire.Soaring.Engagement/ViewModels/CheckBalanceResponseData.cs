using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class CheckBalanceResponseData
    {
        public string UserNumber { get; set; }
        public string ExternalID { get; set; }
        public int PointsBalance { get; set; }
    }
}
