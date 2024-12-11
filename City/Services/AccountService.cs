using City.Services;
using Microsoft.Xrm.Sdk;
using static City.Helpers.ConsoleFormatter;

namespace City.Service
{
    /// <summary>
    /// Service for managing account entities (customers)
    /// </summary>
    public class AccountService : BaseService
    {
        /// <summary>
        /// Constructor for AccountService
        /// </summary>
        /// <param name="entityService"></param>
        public AccountService(EntityService entityService) : base(entityService) { }


        /// <summary>
        /// Perform CRUD operations for account entities (customers)
        /// </summary>
        public void PerformOperations()
        {
            Header("Account CRUD Operations");
            Guid accountId = Guid.Empty;
            Guid contactId = Guid.Empty;

            try
            {
                // Create a primary contact for the account
                ConsoleLogger.Info("Creating Primary Contact");
                contactId = ValidateAndCreateEntity("contact", new Dictionary<string, object>
                {
                    { "firstname", "Jonas" },
                    { "lastname", "Doe" },
                    { "emailaddress1", "jonas.doe@contact.com" },
                    { "telephone1", "9876543210" }
                });

                // Create Account
                ConsoleLogger.Info("Creating Account");
                accountId = ValidateAndCreateEntity("account", new Dictionary<string, object>
                {
                    { "name", "Company A" },
                    { "emailaddress1", "a.company@account.com" },
                    { "telephone1", "1234567890" },
                    { "address1_city", "Abu Dhabi" },
                    { "primarycontactid", new EntityReference("contact", contactId) }
                });

                // Read Account
                ConsoleLogger.Info("Reading Account");
                var account = _entityService.ReadEntity("account", accountId);
                DisplayEntityAttributes(account);

                // Update Account
                ConsoleLogger.Info("Updating Account");
                ValidateAndUpdateEntity("account", accountId, new Dictionary<string, object>
                {
                    { "name", "Updated Company A" },
                    { "emailaddress1", "updated.compnayA@account.com" },
                    { "telephone1", "0987654321" },
                    
                });

                // Read Updated Account
                ConsoleLogger.Info("Reading Updated Account");
                var updatedAccount = _entityService.ReadEntity("account", accountId);
                DisplayEntityAttributes(updatedAccount);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Error during account operations: {ex.Message}");
            }
            finally
            {
                // Delete the account and contact
                SafelyDelete("account", accountId);
                SafelyDelete("contact", contactId);
            }
        
        }

    }
}
