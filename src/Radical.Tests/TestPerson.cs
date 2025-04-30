using System;

namespace Radical.Tests;

public class TestPerson
{
    public string Name { get; set; } = Guid.NewGuid().ToString();
}