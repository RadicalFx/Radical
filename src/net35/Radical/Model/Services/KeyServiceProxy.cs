using System;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Model.Services
{
    public static class KeyServiceProxy
    {
        public static IKeyService CurrentService
        {
            get;
            set;
        }
    }
}
