using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Tests.InlineData
{
    public class CultureInLineData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "pt-BR" };
            yield return new object[] { "en-US" };
            yield return new object[] { "es" }; 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
