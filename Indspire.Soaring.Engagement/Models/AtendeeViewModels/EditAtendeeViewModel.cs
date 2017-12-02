namespace Indspire.Soaring.Engagement.Models.AtendeeViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class EditAtendeeViewModel
    {
        [Required]
        [DisplayName("External ID")]
        public string ExternalID { get; set; }

        [DisplayName("User ID")]
        public int UserID { get; set; }
    }
}
