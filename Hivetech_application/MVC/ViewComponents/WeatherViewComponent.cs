using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.ViewComponents
{
    public class WeatherViewComponent : ViewComponent
    {
        private readonly IWeatherService _weatherService;
        public WeatherViewComponent(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var weather = await _weatherService.ReadAll();
            return View(weather);
        }
    }
}
