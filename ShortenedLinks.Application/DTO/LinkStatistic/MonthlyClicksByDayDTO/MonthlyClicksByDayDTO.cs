namespace ShortenedLinks.Application.DTO.LinkStatistic.MonthlyClicksByDayDTO
{
    public class MonthlyClicksByDayDTO
    {
        public int userId {  get; set; }
        public List<RangedClicksDTO> monthlyClicksByDay { get; set; }
    }
}
