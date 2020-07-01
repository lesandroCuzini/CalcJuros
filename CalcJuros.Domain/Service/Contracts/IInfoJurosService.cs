using System.Threading.Tasks;
using CalcJuros.Domain.Models;

namespace CalcJuros.Domain.Service.Contracts
{
    public interface IInfoJurosService
    {
         InfoJuros GetPercentualTaxaJuros();
    }
}