namespace Indspire.Soaring.Engagement.Models.RedemptionViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CreateRedemptionViewModel
    {
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
