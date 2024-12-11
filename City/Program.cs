using Microsoft.Extensions.Configuration;
using City.Service;
using City.Helpers;
using City.Services;
using static City.Helpers.ConsoleFormatter;

namespace City
{
    /// <summary>
    /// Main program class for the application 
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method for the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Load configuration and initialize ServiceClient
            IConfiguration configuration = Utility.LoadConfiguration();
            using var serviceClient = Utility.InitializeServiceClient(configuration.GetConnectionString("Dataverse"));

            if (serviceClient == null)
            {
                ConsoleLogger.Error("Unable to connect to Dataverse. Exiting application.");
                return;
            }

            // Initialize services
            var entityService = new EntityService(serviceClient);
            var accountService = new AccountService(entityService);
            var contactService = new ContactService(entityService);
            var caseService = new CaseService(entityService);

            // Execute CRUD operations
            try
            {
                accountService.PerformOperations();
                contactService.PerformOperations();
                caseService.PerformOperations();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
