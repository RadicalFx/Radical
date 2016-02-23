namespace Test.Radical.Model.EntityCollection
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class GenericParameterHelperSerializationSurrogate : ISerializationSurrogate
    {
        #region ISerializationSurrogate Members

        public void GetObjectData( object obj, SerializationInfo info, StreamingContext context )
        {
            info.AddValue( "data", ( ( GenericParameterHelper )obj ).Data );
        }

        public object SetObjectData( object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
        {
            var data = info.GetInt32( "data" );
            ( ( GenericParameterHelper )obj ).Data = data;

            return obj;
        }

        #endregion
    }
}
