
using UAParser;
using ShortenedLinks.Application.Interfaces;

namespace ShortenedLinks.Application.Services.DeviceInfo
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public DeviceInfo GetDeviceType(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);
            DeviceInfo deviceInfo = new DeviceInfo()
            {
                Browser = clientInfo.UA.Family,
                Device = clientInfo.Device.Family,
            };

            return deviceInfo;
        }
    }
}
