using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Raimun.ConsoleApp.Configurations;
using Raimun.Core.Common.Interfaces;
using Raimun.Core.Common.Models;
using Raimun.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raimun.ConsoleApp.Services
{
    public class WeatherService : IWeatherService
    {
        private IAppDb _appDb;
        private WeatherConfig _weatherConfig;

        public WeatherService(IAppDb appDb, IOptionsMonitor<WeatherConfig> weatherOption)
        {
            _appDb = appDb;
            _weatherConfig = weatherOption.CurrentValue;
        }
        public async Task HandleLocationWeatherWithGeo(double lat, double lon, DateTime dateTime)
        {
            var url = $"{_weatherConfig.BaseUrl}?key={_weatherConfig.ApiKey}&&q={lat},{lon}&date={dateTime.Date}";
            try
            {
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                webrequest.Method = "GET";
                webrequest.ContentType = "application/json";

                using HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                using StreamReader responseStream = new(webresponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                string result = string.Empty;
                result = await responseStream.ReadToEndAsync();
                var model = JsonConvert.DeserializeObject<WeatherApiResponseModel>(result);

                if (model?.Forecast?.Forecastday?.Any() ?? false)
                {
                    if (model.Forecast.Forecastday[0].Day.AvgTemp > 14) 
                    {
                        if (!await _appDb.Cities.AnyAsync(x => x.Name == model.Location.Name))
                        {
                            var city = new City
                            {
                                Name = model.Location.Name,
                                Temp = model.Forecast.Forecastday[0].Day.AvgTemp
                            };

                            await _appDb.Cities.AddAsync(city);
                            await _appDb.SaveChangesAsync(CancellationToken.None);
                        }
                    }

                }
            }
            catch { }


        }
    }
}
