using System;
using System.Globalization;

namespace Xperiments.Api
{
    public static class Settings
    {
        private const string Prefix = "XPERIMENTS";

        public static string ServiceName { get; } = Prefix.ToLower(CultureInfo.CurrentCulture);

        public static string MongoConnectionString { get; } =
            Environment.GetEnvironmentVariable($"{Prefix}_MONGO_CONNECTION") ?? "mongodb://localhost:27017";
        
        public static string MongoDatabaseName { get; } =
            Environment.GetEnvironmentVariable($"{Prefix}_MONGO_DATABASE_NAME") ?? "Xperiments";
    }
}