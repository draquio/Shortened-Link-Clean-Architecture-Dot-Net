using MediatR;
using ShortenedLinks.Application.DTO.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopDevices
{
    public class GetTopDevicesQuery : IRequest<List<DeviceTopDTO>>
    {
        public int UserId {  get; set; }

        public GetTopDevicesQuery(int userId)
        {
            UserId = userId;
        }
    }
}
