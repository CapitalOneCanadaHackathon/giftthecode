using Indspire.Soaring.Engagement.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Models
{
    public class TopAward
    {
        public int AwardID { get; set; }

        public Award Award { get; set; }

        public int TotalPoints { get; set; }
    }
}
