using System.ComponentModel.DataAnnotations;

namespace CareHomeInfoTracker.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public string? Password { get; set; }
    }
}
