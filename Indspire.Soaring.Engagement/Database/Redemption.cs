namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Redemption
    {
        [DisplayName("Redemption ID")]
        public int RedemptionID { get; set; }

        [DisplayName("Redemption Number")]
        public string RedemptionNumber { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Points Required")]
        public int PointsRequired { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Redemption Logs")]
        public IList<RedemptionLog> RedemptionLogs { get; set; }
    }
}