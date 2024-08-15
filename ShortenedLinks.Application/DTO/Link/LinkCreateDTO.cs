
using System.ComponentModel.DataAnnotations;

namespace ShortenedLinks.Application.DTO.Link
{
    public class LinkCreateDTO
    {
        [Required]
        public string Link { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
