using System.Threading.Tasks;

namespace CalcJuros.Domain.Service.Contracts
{
    public interface ICalcJurosService
    {
         decimal CalcJurosComposto(decimal valorInicial, int qtdeMeses);
    }
}