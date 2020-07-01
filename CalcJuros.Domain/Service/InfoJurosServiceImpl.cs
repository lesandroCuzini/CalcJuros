using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using CalcJuros.Domain.Models;
using CalcJuros.Domain.Service.Contracts;

namespace CalcJuros.Domain.Service
{
    public class InfoJurosServiceImpl : IInfoJurosService
    {
        private readonly ILogger<InfoJurosServiceImpl> _logger;
        private readonly HttpClient _httpClient;

        public InfoJurosServiceImpl(HttpClient httpClient, ILogger<InfoJurosServiceImpl> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public InfoJuros GetPercentualTaxaJuros()
        {
            var uriAPITaxaJuros = "/taxajuros";

            try {
                var responseString =  _httpClient.GetStringAsync(uriAPITaxaJuros).Result;
                var infoJuros = JsonSerializer.Deserialize<InfoJuros>(responseString);
                return infoJuros;
            }
            catch(HttpRequestException httpReqEx) {
                _logger.LogError("Falha ao obter o percentual da Taxa de Juros Mensal: " + httpReqEx.Message);
                return null;
            }
        }
    }
}