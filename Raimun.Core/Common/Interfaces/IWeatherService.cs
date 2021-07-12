using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raimun.Core.Common.Interfaces
{
    public interface IWeatherService
    {
        public Task HandleLocationWeatherWithGeo(double lat, double lon, DateTime dateTime);
    }
}
