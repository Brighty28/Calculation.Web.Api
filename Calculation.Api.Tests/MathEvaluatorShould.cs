using Calculation.Web.Api;
using FluentAssertions;
using Xunit;

namespace Calculation.Api.Tests
{
    public static class MathEvaluatorShould
    {
        [Theory]
        [InlineData("4+5*2", true)]
        [InlineData("-4+5/2", false)]
        [InlineData("4+5/2-1", true)]
        [InlineData("5+5/2", true)]
        [InlineData("(99+100)/2", false)]
        [InlineData("99+100/2", true)]
        [InlineData("1000.ghgh+2", false)]
        [InlineData("0.jhjhj", false)]
        [InlineData("fhgfhfhfh", false)]
        public static void ReturnIsExpression_Result(string value, bool expectedResult)
        {
            var result = MathEvaluator.IsExpression(value);
            result.Should().Be(expectedResult);
        }

        [Theory]
        [ClassData(typeof(TestData.CalculationTestData))]
        public static void ReturnDoubleResult_OnValidExpression(string value, double expectedResult)
        {
            var result = MathEvaluator.Parse(value);
            result.Should().Be(expectedResult);
            result.Should().BeOfType(typeof(double));
        }
    }
}
