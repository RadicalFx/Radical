using System;
using Radical.ComponentModel;
using Radical.Conversions;

namespace Radical.Model.Services
{
    public class Int32KeyService : IKeyService
    {
        public IKey GenerateEmpty()
        {
            return ( 0 ).AsKey();
        }
    }
}
