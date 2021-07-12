using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raimun.Core.Common.Models
{

    public class WeatherApiResponseModel
    {
        [JsonProperty("forecast")]
        public Forecast Forecast { get; set; }
        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class Forecast
    {
        [JsonProperty("forecastday")]
        public Forecastday[] Forecastday { get; set; }
    }

    public class Forecastday
    {
        [JsonProperty("day")]
        public Day Day { get; set; }
    }
    public class Day
    {
        [JsonProperty("avgtemp_c")]
        public double AvgTemp { get; set; }
    }
}
