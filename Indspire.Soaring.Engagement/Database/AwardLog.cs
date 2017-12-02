namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.ComponentModel;

    public class AwardLog
    {
        [DisplayName("Award Log ID")]
        public int AwardLogID { get; set; }

        [DisplayName("Award ID")]
        public int AwardID { get; set; }

        [DisplayName("Award")]
        public Award Award { get; set; }

        [DisplayName("Points")]
        public int Points { get; set; }

        [DisplayName("User ID")]
        public int UserID { get; set; }

        [DisplayName("User")]
        public User User { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
