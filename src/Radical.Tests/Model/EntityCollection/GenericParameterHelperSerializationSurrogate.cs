namespace Radical.Tests.Model.EntityCollection
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Runtime.Serialization;

    public class GenericParameterHelperSerializationSurrogate : ISerializationSurrogate
    {

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("data", ((GenericParameterHelper)obj).Data);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var data = info.GetInt32("data");
            ((GenericParameterHelper)obj).Data = data;

            return obj;
        }

    }
}
