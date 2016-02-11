using System.ComponentModel.DataAnnotations;

namespace P2P.Model
{
    public class Device
    {
        [Required(ErrorMessage = "Device Id is required.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "EmailId is required.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "EmailId is required.")]
        public string Name { get; set; }

        [Required]
        public string GcmToken { get; set; }

        public string Clubcard { get; set; }

        public string Mobile { get; set; }
    }
}