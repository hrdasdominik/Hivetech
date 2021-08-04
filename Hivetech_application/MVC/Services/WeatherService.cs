using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WeatherModel.Root> ReadAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://api.openweathermap.org/");

            string stringResult;
            using (var response = await client.GetAsync($"data/2.5/weather?q=Zagreb&units=metric&appid=2049ea424ff8aeb43573849396eab4df"))
            {
                stringResult = await response.Content.ReadAsStringAsync();
            }
            var obj = JsonConvert.DeserializeObject<WeatherModel.Root>(stringResult);
            return obj;
        }
    }
}
