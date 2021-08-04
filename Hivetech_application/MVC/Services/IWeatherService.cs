using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.Services
{
    public interface IWeatherService
    {
        Task<WeatherModel.Root> ReadAll();
    }
}
