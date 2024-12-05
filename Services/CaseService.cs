using Microsoft.Xrm.Sdk;
using City.Services;
using static City.Helpers.ConsoleFormatter;
using static City.Helpers.EntityValidator;

namespace City.Service
{
    /// <summary>
    /// Service to perform CRUD operations on cases
    /// </summary>
    public class CaseService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the CaseService class
        /// </summary>
        /// <param name="entityService"></param>
        public CaseService(EntityService entityService) : base(entityService) { }

        /// <summary>
        /// Perform CRUD operations for cases
        /// </summary>
        public void PerformOperations()
        {
            Header("Case (Incident) CRUD Operations");

            Guid accountId = Guid.Empty;
            Guid contactId = Guid.Empty;
            Guid caseForAccountId = Guid.Empty;
            Guid caseForContactId = Guid.Empty;

            try
            {
                // Create Account and Contact
                accountId = CreateAccount();
                contactId = CreateContact();

                // Create cases for Account and Contact
                caseForAccountId = CreateCase("account", accountId, "Case for Account");
                caseForContactId = CreateCase("contact", contactId, "Case for Contact");


                // Read Cases for Account and Contact
                ConsoleLogger.Info("Reading Case for Account");
                var caseForAccount = _entityService.ReadEntity("incident", caseForAccountId);
                DisplayEntityAttributes(caseForAccount);

                ConsoleLogger.Info("Reading Case for Contact");
                var caseForContact = _entityService.ReadEntity("incident", caseForContactId);
                DisplayEntityAttributes(caseForContact);


                // Update Cases
                UpdateCase(caseForAccountId, "Updated Case for Account", "Updated description for Account Case");
                UpdateCase(caseForContactId, "Updated Case for Contact", "Updated description for Contact Case");


                // Read Updated Cases
                ConsoleLogger.Info("Reading Updated Case for Account");
                var updatedCaseForAccount = _entityService.ReadEntity("incident", caseForAccountId);
                DisplayEntityAttributes(updatedCaseForAccount);

                ConsoleLogger.Info("Reading Updated Case for Contact");
                var updatedCaseForContact = _entityService.ReadEntity("incident", caseForContactId);
                DisplayEntityAttributes(updatedCaseForContact);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Error during case operations: {ex.Message}");
            }
            finally
            {
                // Delete All Created Records
                SafelyDelete("incident", caseForAccountId);
                SafelyDelete("incident", caseForContactId);
                SafelyDelete("account", accountId);
                SafelyDelete("contact", contactId);
            }
        }


        /// <summary>
        /// Create a case linked to an account or contact
        /// </summary>
        /// <param name="customerType">The type of customer (account or contact)</param> 
        /// <param name="customerId">The ID of the customer</param> 
        /// <param name="title">The title of the case</param> 
        /// <returns>The ID of the created case</returns> 
        private Guid CreateCase(string customerType, Guid customerId, string title)
        {
            ConsoleLogger.Info($"Creating Case for {customerType}");
            var caseAttributes = new Dictionary<string, object>
            {
                { "title", title },
                { "description", $"This is a case linked to {customerType}" },
                { "customerid", new EntityReference(customerType, customerId) }
            };

            var caseAttributeCollection = CreateAttributes(caseAttributes);
            Validate("incident", caseAttributeCollection, isCreate: true);
            return _entityService.CreateEntity("incident", caseAttributeCollection);
        }

        /// <summary>
        /// Update a case with new details (title and description)
        /// </summary>
        /// <param name="caseId">ID of the case</param>
        /// <param name="newTitle">New title for the case</param>
        /// <param name="newDescription">New description for the case</param>
        private void UpdateCase(Guid caseId, string newTitle, string newDescription)
        {
            ConsoleLogger.Info($"Updating Case with ID: {caseId}");
            var updatedAttributes = new Dictionary<string, object>
            {
                { "title", newTitle },
                { "description", newDescription }
            };

            var updatedAttributeCollection = CreateAttributes(updatedAttributes);
            Validate("incident", updatedAttributeCollection, isCreate: false);

            _entityService.UpdateEntity("incident", caseId, updatedAttributeCollection);
        }

        /// <summary>
        /// Create a sample account for linking with a case
        /// </summary>
        /// <returns>GUID of the created account</returns>
        private Guid CreateAccount()
        {
            ConsoleLogger.Info("Creating Account for Case");
            var accountAttributes = new Dictionary<string, object>
            {
                { "name", "Case Account" },
                { "emailaddress1", "case@account.com" },
                { "telephone1", "1234567890" }
            };

            var accountAttributeCollection = CreateAttributes(accountAttributes);
            Validate("account", accountAttributeCollection, isCreate: true);
            return _entityService.CreateEntity("account", accountAttributeCollection);
        }

        /// <summary>
        /// Create a sample contact for linking with a case
        /// </summary>
        /// <returns>GUID of the created contact</returns>
        private Guid CreateContact()
        {
            ConsoleLogger.Info("Creating Contact for Case");
            var contactAttributes = new Dictionary<string, object>
            {
                { "firstname", "Casey" },
                { "lastname", "Contact" },
                { "emailaddress1", "casey@contact.com" }
            };

            var contactAttributeCollection = CreateAttributes(contactAttributes);
            Validate("contact", contactAttributeCollection, isCreate: true);
            return _entityService.CreateEntity("contact", contactAttributeCollection);
        }
    }
}
