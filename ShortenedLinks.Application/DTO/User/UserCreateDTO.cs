
using System.ComponentModel.DataAnnotations;

namespace ShortenedLinks.Application.DTO.User
{
    public class UserCreateDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
