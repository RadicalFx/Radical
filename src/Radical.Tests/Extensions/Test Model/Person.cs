using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.Tests.Extensions
{
    class Person
    {
        public Address Address { get; set; }
        public Person()
        {

        }

        public Person(string privateValue)
        {
            this.privateValue = privateValue;
        }

        protected string privateValue { get; private set; }

        public int Age { get; set; }
        public string Name { get; set; }

    }
}
