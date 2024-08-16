using System;
using System.Collections;

namespace WebApiTest.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "pt-BR" };
        yield return new object[] { "en-US" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
