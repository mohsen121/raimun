using Microsoft.AspNetCore.Mvc;
using Raimun.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raimun.Api.Controllers
{
    public class WeatherController : BaseController
    {
        private WeatherSender _weatherSender;

        public WeatherController(WeatherSender weatherSender)
        {
            _weatherSender = weatherSender;
        }
        [HttpPost]
        public IActionResult Post(WeatherRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Hangfire.BackgroundJob.Schedule(() => _weatherSender.Send(model), TimeSpan.FromSeconds(30));
            return Ok();
        }
    }
}
