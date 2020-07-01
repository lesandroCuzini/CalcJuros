using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CalcJuros.Domain.Service.Contracts;
using Microsoft.Extensions.Configuration;

namespace CalcJuros.Api.Controllers
{   
    [ApiController]
    [Route("/showmethecode")]
    public class ShowmethecodeController : ControllerBase
    {
        private IConfiguration _configuration;
        public ShowmethecodeController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetShowmethecode()
        {
            string repositorioUrlFromSettings = _configuration.GetValue<string>("RepositorioUrl");
            return Ok(new { urlRepositorio = repositorioUrlFromSettings });
        }
    }
}