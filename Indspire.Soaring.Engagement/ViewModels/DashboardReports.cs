using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class AwardsRow
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AwardNumber { get; set; }
        public int TimesAwards { get; set; }

    }

    public class UserRow
    {
        public string UserNamber { get; set; }
        public string ExternalId { get; set; }
        public int Points { get; set; }
    }
    public class DashboardReports
    {
        public List<AwardsRow> AwardsList { get; set; } = new List<AwardsRow>();
        public List<UserRow> UserList { get; set; } = new List<UserRow>();
    }
}
