using System.Collections;
using System.Collections.Generic;

namespace Calculation.Api.Tests
{
    public class TestData
    {
        public class CalculationTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] { "4+5*2", 14.0 },
                new object[] { "4+5/2", 6.5 },
                new object[] { "4+5/2-1", 5.5 },
                new object[] { "5+5/2", 7.5 },
                new object[] { "99+100/2", 149 },
                new object[] { "1000.111+2", 1002.111 }
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
