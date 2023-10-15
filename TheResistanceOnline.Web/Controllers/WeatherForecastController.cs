﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheResistanceOnline.Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class WeatherForecastController: ApiControllerBase
{
    #region Fields

    private readonly ILogger<WeatherForecastController> _logger;

    private static readonly string[] _summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    #endregion

    #region Construction

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Public Methods

    [HttpGet]
    [Authorize]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                                                      {
                                                          Date = DateTime.Now.AddDays(index),
                                                          TemperatureC = Random.Shared.Next(-20, 55),
                                                          Summary = _summaries[Random.Shared.Next(_summaries.Length)]
                                                      })
                         .ToArray();
    }

    #endregion
}