
using UAParser;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.DTO.Device;

namespace ShortenedLinks.Application.Services.DeviceInfo
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public DeviceInfoDTO GetDeviceType(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);
            DeviceInfoDTO deviceInfo = new DeviceInfoDTO()
            {
                Browser = clientInfo.UA.Family,
                Device = clientInfo.Device.Family,
            };

            return deviceInfo;
        }
    }
}
