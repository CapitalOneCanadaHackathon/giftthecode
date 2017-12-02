namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.ComponentModel;

    public class RedemptionLog
    {
        [DisplayName("Redemption Log ID")]
        public int RedemptionLogID { get; set; }

        [DisplayName("Redemption ID")]
        public int RedemptionID { get; set; }

        [DisplayName("Redemption")]
        public Redemption Redemption { get; set; }

        [DisplayName("User ID")]
        public int UserID { get; set; }

        [DisplayName("User")]
        public User User { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
