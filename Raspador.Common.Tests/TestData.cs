using System.Collections;

namespace Raspador.Common.Tests;

public class TestData : IEnumerable<Data>
{
    public IEnumerator<Data> GetEnumerator() => new List<Data>
    {
        new("Kris", 30),
        new("Ana", 18)
    }.GetEnumerator();


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}