using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;
using static City.Helpers.ConsoleFormatter;

namespace City.Helpers
{
    /// <summary>
    /// Provides utility methods for configuration and client initialization
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Load application configuration
        /// </summary>
        /// <returns>IConfiguration instance</returns>
        public static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// Initialize and validate a ServiceClient for Dataverse
        /// </summary>
        /// <param name="connectionString">Dataverse connection string</param>
        /// <returns>Valid ServiceClient or null if connection fails</returns>
        public static ServiceClient InitializeServiceClient(string connectionString)
        {
            try
            {
                var serviceClient = new ServiceClient(connectionString);
                if (!serviceClient.IsReady)
                {
                    ConsoleLogger.Error("Dataverse connection failed: " + serviceClient.LastError);
                    return null;
                }

                ConsoleLogger.Success($"Connected to {serviceClient.ConnectedOrgFriendlyName}");
                return serviceClient;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Exception while creating ServiceClient: {ex.Message}");
                return null;
            }
        }
    }
}
