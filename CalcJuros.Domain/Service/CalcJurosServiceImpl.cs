using System;
using System.Threading.Tasks;
using CalcJuros.Domain.Service.Contracts;

namespace CalcJuros.Domain.Service
{
    public class CalcJurosServiceImpl : ICalcJurosService
    {
        private IInfoJurosService _infoJurosService;

        public CalcJurosServiceImpl(IInfoJurosService infoJurosService) =>
            _infoJurosService = infoJurosService;
        public decimal CalcJurosComposto(decimal valorInicial, int qtdeMeses)
        {
            var infoJuros =  _infoJurosService.GetPercentualTaxaJuros();
            var calculo = (decimal) Math.Pow((double)(1 + (infoJuros.percTaxaJurosMensal / 100)), qtdeMeses);
            calculo = valorInicial * calculo;

            return Math.Truncate(calculo * 100) / 100;
        }
    }
}