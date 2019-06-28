using Radical.ComponentModel;

namespace Radical.Model.Services
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
