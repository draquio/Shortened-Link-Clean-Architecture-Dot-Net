
namespace ShortenedLinks.Application.DTO.ShortLink
{
    public class ShortLinkDetailDTO
    {
        public int Id { get; set; }
        public string OriginalLink { get; set; }
        public string ShortenedLink { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
