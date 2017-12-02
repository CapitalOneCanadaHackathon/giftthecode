namespace Indspire.Soaring.Engagement.Models.RedemptionViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class EditRedemptionViewModel
    {
        [Required]
        [DisplayName("Redemption ID")]
        public int RedemptionID { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Points Required")]
        public int PointsRequired { get; set; }
    }
}
