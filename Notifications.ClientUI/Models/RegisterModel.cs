using System.ComponentModel.DataAnnotations;

namespace Notifications.ClientUI.Models
{
    public class RegisterModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string Name { get; set; }  
    }
}
