

namespace ShortenedLinks.Application.DTO.LinkStatistic
{
    public class LinkStatisticRegisterDTO
    {
        public int LinkId { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitorIp { get; set; }
        public string Country { get; set; }
        public string Device {  get; set; }
        public string Browser { get; set; }
    }
}
