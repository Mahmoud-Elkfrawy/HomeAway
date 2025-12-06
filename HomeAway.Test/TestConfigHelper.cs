using Microsoft.Extensions.Configuration;
using System.IO;

public static class TestConfigHelper
{
    public static string GetConnectionString(string name = "DefaultConnection2")
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var config = builder.Build();
        var connectionString = config.GetConnectionString(name);

        if (connectionString == null)
            throw new InvalidOperationException($"Connection string '{name}' not found.");

        return connectionString;
    }
}
