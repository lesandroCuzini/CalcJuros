using System;
using Moq;
using Xunit;
using CalcJuros.Domain.Service.Contracts;
using CalcJuros.Domain.Service;
using CalcJuros.Domain.Models;

namespace DomainService.UnitTest
{
    public class CalcJurosServiceTest
    {
        readonly Mock<IInfoJurosService> _infoJurosServiceMock;

        public CalcJurosServiceTest()
        {
            _infoJurosServiceMock = new Mock<IInfoJurosService>();
        }

        [Fact]
        public void CalcJurosCompostoOk_Test()
        {
            InfoJuros infoJurosMock = new InfoJuros();
            infoJurosMock.percTaxaJurosMensal = 1;

            _infoJurosServiceMock.Setup(x => x.GetPercentualTaxaJuros()).Returns(infoJurosMock);
            var calcJurosService = new CalcJurosServiceImpl(_infoJurosServiceMock.Object);
            var resultCalcJurosComposto = calcJurosService.CalcJurosComposto(100, 2);
            Assert.Equal(102.01m, resultCalcJurosComposto);
        }
    }
}
