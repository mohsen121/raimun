using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raimun.Core.Common.Models
{
    public class WeatherRequestModel
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public DateTime Date { get; set; }
    }
}
