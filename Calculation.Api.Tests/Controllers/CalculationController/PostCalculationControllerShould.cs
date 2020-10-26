using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Calculation.Api.Tests.Controllers.CalculationController
{
    public class PostCalculationControllerShould
    {
        private readonly Web.Api.Controllers.CalculationController _calculationController;

        public PostCalculationControllerShould()
        {
            _calculationController = new Web.Api.Controllers.CalculationController(
               Substitute.For<ILogger<Web.Api.Controllers.CalculationController>>());
        }

        [Theory]
        [InlineData("4+5*2")]
        [InlineData("4+5/2")]
        [InlineData("4+5/2-1")]
        [InlineData("5+5/2")]
        [InlineData("99+100/2")]
        public void ReturnOkResult_OnValidExpression(string value)
        {
            var result = _calculationController.PostCalculation(value);
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Theory]
        [InlineData("1000.ghgh+2")]
        [InlineData("0.jhjhj")]
        [InlineData("fhgfhfhfh")]
        public void ReturnBadRequest_BasedOnExpression(string value)
        {
            var result = _calculationController.PostCalculation(value);
            result.Should().BeOfType(typeof(BadRequestResult));
        }

        [Fact]
        public void ReturnBadRequest_DivideByZero()
        {
            var result = _calculationController.PostCalculation("3/0");
            result.Should().BeOfType(typeof(BadRequestResult));
        }

    }
}
