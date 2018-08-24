using System;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Xperiments.Persistence
{
    public class MyProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(string) && type.GetCustomAttribute(typeof(MultilingualPropertyAttribute)) != null)
            {
                return new MyStringSerializer();
            }

            return null;
        }
    }

    public class MyStringSerializer : SerializerBase<string>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            context.Writer.WriteString("DOOM");
        }
    }
}