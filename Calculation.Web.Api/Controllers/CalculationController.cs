using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculation.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        private readonly ILogger<CalculationController> _logger;

        public CalculationController(ILogger<CalculationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Provides examples
        /// </summary>
        /// <remarks>Used to retrieve a list of example expressions.</remarks>
        /// <response code="200">The request was handled successfully, response data is contained in the response body</response>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "4 + 5 * 2", "4 + 5 / 2", "4 + 5 / 2 - 1" };
        }

        /// <summary>
        /// Evaluates a string expression consisting of non-negative integers and the + - / * operators.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Calculation
        ///     {
        ///        "expression": "4+5/2"
        ///     }
        ///
        /// </remarks>
        /// <param name="expression"></param>
        /// <returns>Calculation based on the provided expression</returns>
        /// <response code="200">The request was handled successfully.</response>
        /// <response code="400">If the expression is invalid</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostCalculation([FromBody, Required] string expression)
        {
            if (!IsValid(expression.Trim()))
            {
                _logger.LogError("{MethodName}, expression {Expression} ", nameof(PostCalculation), expression);
                return BadRequest();
            }

            var result = Calculate(expression.Trim());

            return Ok(result);
        }

        [NonAction]
        public double Calculate(string expression)
        {
            var result = 0.0;

            var results = Regex.Split(expression, @"\D+[+\-*/.]");

            if (results.Length != 0)
            {
                foreach (var reg in results)
                {
                    return Convert.ToDouble(new DataTable().Compute(reg, null));
                }
            }

            return result;
        }

        [NonAction]
        public bool IsValid(string expression)
        {
            return Regex.IsMatch(expression, @"(?:[0-9 ()]+[*+/-])+[0-9 ()]+");
        }

    }
}