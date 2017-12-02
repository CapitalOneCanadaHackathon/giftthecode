namespace Indspire.Soaring.Engagement.Models.AtendeeViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CreateAtendeeViewModel
    {
        [Required]
        [DisplayName("External ID")]
        public string ExternalID { get; set; }
    }
}
