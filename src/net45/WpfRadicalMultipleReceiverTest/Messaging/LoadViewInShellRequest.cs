using System;

namespace WpfRadicalMultipleReceiver.Messaging
{
    public class LoadViewInShellRequest
    {
        public Type ViewType { get; set; }

        public LoadViewInShellRequest(Type viewType)
        {
            this.ViewType = viewType;
        }
    }
}
