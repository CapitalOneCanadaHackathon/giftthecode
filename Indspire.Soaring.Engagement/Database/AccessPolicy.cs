namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.ComponentModel;

    public class AccessPolicy
    {
        [DisplayName("Access Policy ID")]
        public int AccessPolicyID { get; set; }

        [DisplayName("Access Policy Key")]
        public string AccessPolicyKey { get; set; }

        [DisplayName("Domain")]
        public string Domain { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}