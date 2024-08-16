
namespace ShortenedLinks.Domain.Entities
{
    public class LinkClicksStatisticTop
    {
        public int LinkId { get; set; }
        public int ClickCount { get; set; }
        public Link Link { get; set; }
        public PeriodType PeriodType { get; set; }
    }
    public enum PeriodType
    {
        Day,
        Week,
        Month
    }

}
