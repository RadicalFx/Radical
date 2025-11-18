using System;
using System.Collections;
using System.Linq;

namespace Radical.Tests;

public class TestTypeHelper : IComparable, IEnumerable, ICloneable
{
    public int? Data { get; set; }

    public TestTypeHelper() { }
    public TestTypeHelper(int? data) => Data = data;

    public int CompareTo(object obj) => Data == (obj as TestTypeHelper)?.Data ? 0 : -1;

    public IEnumerator GetEnumerator()
    {
        if (Data == null)
        {
            yield break;
        }

        Enumerable.Range(0, Data.Value).GetEnumerator();
    }

    public object Clone() => new TestTypeHelper(Data);
    
    public override bool Equals(object obj) => 
        obj is TestTypeHelper other && Data == other.Data;
    
    public override int GetHashCode() => Data.GetHashCode();
}