namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class User
    {
        [DisplayName("User ID")]
        public int UserID { get; set; }

        [DisplayName("User Number")]
        public string UserNumber { get; set; }

        [DisplayName("External ID")]
        public string ExternalID { get; set; }

        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Redemption Logs")]
        public IList<RedemptionLog> RedemptionLogs { get; set; }

        [DisplayName("Award Logs")]
        public IList<AwardLog> AwardLogs { get; set; }
    }
}
