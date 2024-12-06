using City.Services;
using Microsoft.Identity.Client;
using Microsoft.Xrm.Sdk;
using static City.Helpers.ConsoleFormatter;
using static City.Helpers.EntityValidator;

namespace City.Service
{
    /// <summary>
    /// Service for managing contact entities
    /// </summary>
    public class ContactService : BaseService
    {
        /// <summary>
        /// Constructor for ContactService
        /// </summary>
        /// <param name="entityService"></param>
        public ContactService(EntityService entityService) : base(entityService) { }

        /// <summary>
        /// Perform CRUD operations for contact entities
        /// </summary>
        public void PerformOperations()
        {
            Header("Contact CRUD Operations");

            // Initialize variables
            Guid contactId = Guid.Empty;
            Guid accountId = Guid.Empty;

            try
            {
                // Create Account
                ConsoleLogger.Info("Creating Account for Contact");
                accountId = ValidateAndCreateEntity("account", new Dictionary<string, object>
                {
                    { "name", "Company B" },
                    { "emailaddress1", "b.company@account.com" },
                    { "telephone1", "12341234" },
                    { "address1_city", "London" }
                });

                // Create Contact
                ConsoleLogger.Info("Creating Contact");
                contactId = ValidateAndCreateEntity("contact", new Dictionary<string, object>
                {
                    { "firstname", "Jane" },
                    { "lastname", "Doe" },
                    { "emailaddress1", "jane.doe@contact.com" },
                    { "telephone1", "1234567890" },
                    { "parentcustomerid", new EntityReference("account", accountId) }
                });

                // Read contact
                ConsoleLogger.Info("Reading Contact");
                var contact = _entityService.ReadEntity("contact", contactId);
                DisplayEntityAttributes(contact);

                // Update contact
                ConsoleLogger.Info("Updating Contact");
                ValidateAndUpdateEntity("contact", contactId, new Dictionary<string, object>
                {
                    { "firstname", "Updated Jane" },
                    { "emailaddress1", "updated.jane@contact.com" }
                });

                // Read updated contact
                var updatedContact = _entityService.ReadEntity("contact", contactId);
                DisplayEntityAttributes(updatedContact);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Error during contact operations: {ex.Message}");
            }
            finally
            {
                // Delete All Created Records
                SafelyDelete("contact", contactId);
                SafelyDelete("account", accountId);
            }
        }
    }
}
