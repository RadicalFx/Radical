using MessagePack;
using MessagePack.Resolvers;
using System;
using System.IO;

namespace Radical.Tests;

public static class ExceptionsExtensions
{
    public static T SerializeAndDeserialize<T>(this T source) where T: Exception
    {
        var bytes = MessagePackSerializer.Serialize(source, ContractlessStandardResolver.Options);
        using MemoryStream ms = new(bytes);
            
        var ex = MessagePackSerializer.Deserialize<T>(ms, ContractlessStandardResolver.Options);
        return ex;
    }
}