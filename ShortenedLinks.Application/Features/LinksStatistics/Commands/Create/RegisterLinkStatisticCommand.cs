using MediatR;

namespace ShortenedLinks.Application.Features.LinksStatistics.Commands.Create
{
    public class RegisterLinkStatisticCommand : IRequest<Unit>
    {
        public int LinkId { get; set; }
        public string VisitorIp { get; set; }
        public string UserAgent { get; set; }
        public RegisterLinkStatisticCommand(int linkId, string visitorIp, string userAgent)
        {
            LinkId = linkId;
            VisitorIp = visitorIp;
            UserAgent = userAgent;
        }

    }
}
