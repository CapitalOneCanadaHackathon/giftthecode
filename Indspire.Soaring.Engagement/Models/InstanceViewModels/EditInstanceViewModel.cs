namespace Indspire.Soaring.Engagement.Models.InstanceViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class EditInstanceViewModel
    {
        [Required]
        public int InstanceID { get; set; }

        [Required]
        [DisplayName("Event Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Default Instance")]
        public bool DefaultInstance { get; set; }
    }
}
