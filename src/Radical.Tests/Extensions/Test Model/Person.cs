using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.Tests.Extensions
{
    class  Person
    {
        public Address Address { get; set; }
        public Person ()
        {

        }

        public Person(String privateValue)
        {
            this.privateValue = privateValue;
        }

        protected String privateValue { get; private set; }

        public int Age { get; set; }
        public String Name { get; set; }
        
    }
}
