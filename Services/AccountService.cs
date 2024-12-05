using City.Services;
using static City.Helpers.ConsoleFormatter;
using static City.Helpers.EntityValidator;

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

            try
            {
                // Create and validate account
                ConsoleLogger.Info("Creating Account");
                var attributes = new Dictionary<string, object>
                {
                    { "name", "Primary Account" },
                    { "emailaddress1", "primary@account.com" },
                    { "telephone1", "1234567890" }
                };
                var attributeCollection = CreateAttributes(attributes);
                Validate("account", attributeCollection, isCreate: true);
                accountId = _entityService.CreateEntity("account", attributeCollection);


                // Read account
                ConsoleLogger.Info("Reading Account");
                var account = _entityService.ReadEntity("account", accountId);
                DisplayEntityAttributes(account);


                // Update and validate account
                ConsoleLogger.Info("Updating Account");
                var updatedAttributes = new Dictionary<string, object>
                {
                    { "name", "Updated Account" },
                    { "emailaddress1", "updated@account.com" },
                    { "telephone1", "9876543210" }
                };
                var updatedAttributeCollection = CreateAttributes(updatedAttributes);
                Validate("account", updatedAttributeCollection, isCreate: false);
                _entityService.UpdateEntity("account", accountId, updatedAttributeCollection);


                // Read updated account
                ConsoleLogger.Info("Reading Updated Account");
                var updatedAccount = _entityService.ReadEntity("account", accountId);
                DisplayEntityAttributes(updatedAccount);
            }
            catch (ArgumentException ex)
            {
                ConsoleLogger.Error($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Error during account operations: {ex.Message}");
            }
            finally
            {
                // Delete All Created Records
                SafelyDelete("account", accountId);
            }
        }
    }
}
