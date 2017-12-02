namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Award
    {
        public int InstanceID { get; set; }

        [DisplayName("Event")]
        public Instance Instance { get; set; }

        [DisplayName("Award ID")]
        public int AwardID { get; set; }

        [DisplayName("Vendor ID")]
        public int VendorID { get; set; }

        [DisplayName("Award Number")]
        public string AwardNumber { get; set; }

        [DisplayName("Points")]
        public int Points { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Award Logs")]
        public IList<AwardLog> AwardLogs { get; set; }
    }
}