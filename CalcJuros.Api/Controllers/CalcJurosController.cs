using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CalcJuros.Domain.Service.Contracts;

namespace CalcJuros.Api.Controllers
{
    [ApiController]
    [Route("/calculajuros")]
    public class CalcJurosController : ControllerBase
    {
        private readonly ILogger<CalcJurosController> _logger;
        private IInfoJurosService _infoJurosService;
        private ICalcJurosService _calcJurosService;

        public CalcJurosController(ILogger<CalcJurosController> logger, 
            ICalcJurosService calcJurosService,
            IInfoJurosService infoJurosService)
        {
            _logger = logger;
            _calcJurosService = calcJurosService;
            _infoJurosService = infoJurosService;
        }

        [HttpGet]
        public IActionResult CalcJurosCompostoAsync(decimal valorinicial, int meses)
        {
            try {
                var resultCalcJurosComposto = _calcJurosService.CalcJurosComposto(valorinicial, meses);
                return Ok(new { resultado = resultCalcJurosComposto });
            }
            catch ( Exception ex ) {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}