namespace Indspire.Soaring.Engagement.Models.AwardViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class EditAwardViewModel
    {
        [Required]
        [DisplayName("Award ID")]
        public int AwardID { get; set; }

        [Required]
        [DisplayName("Points")]
        public int Points { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
