using City.Services;
using Microsoft.Xrm.Sdk;
using static City.Helpers.ConsoleFormatter;

namespace City.Service
{
    /// <summary>
    /// Service to perform CRUD operations on cases
    /// </summary>
    public class CaseService : BaseService
    {
        /// <summary>
        /// Constructor for CaseService
        /// </summary>
        /// <param name="entityService"></param>
        public CaseService(EntityService entityService) : base(entityService) { }

        /// <summary>
        /// Perform CRUD operations for case entities
        /// </summary>
        public void PerformOperations()
        {
            Header("Case (Incident) CRUD Operations");

            // Initialize variables
            Guid accountId = Guid.Empty;
            Guid contactId = Guid.Empty;
            Guid caseForAccountId = Guid.Empty;
            Guid caseForContactId = Guid.Empty;

            try
            {
                // Create Account and Contact
                ConsoleLogger.Info("Creating Account and Contact for Cases");
                accountId = ValidateAndCreateEntity("account", new Dictionary<string, object>
                {
                    { "name", "Company C" },
                    { "emailaddress1", "c.companyA@account.com" },
                    { "telephone1", "1234567890" }
                });

                contactId = ValidateAndCreateEntity("contact", new Dictionary<string, object>
                {
                    { "firstname", "John" },
                    { "lastname", "Doe" },
                    { "emailaddress1", "john.doe@contact.com" }
                });

                // Create Cases
                ConsoleLogger.Info("Creating Cases");
                caseForAccountId = CreateCase("account", accountId, "Case Title for Account", 1, 1, "Account Case Description");
                caseForContactId = CreateCase("contact", contactId, "Case Title for Contact", 2, 2, "Contact Case Description");

                // Read Cases
                ConsoleLogger.Info("Reading Case for Account");
                var caseForAccount = _entityService.ReadEntity("incident", caseForAccountId);
                DisplayEntityAttributes(caseForAccount);

                ConsoleLogger.Info("Reading Case for Contact");
                var caseForContact = _entityService.ReadEntity("incident", caseForContactId);
                DisplayEntityAttributes(caseForContact);

                // Update Cases
                ConsoleLogger.Info("Updating Cases");
                UpdateCase(caseForAccountId, new Dictionary<string, object>
                {
                    { "title", "Updated Case Title for Account" },
                    { "description", "Updated Account Description" },
                    { "prioritycode", new OptionSetValue(2) },
                    { "statuscode", new OptionSetValue(2) },   
                });
                UpdateCase(caseForContactId, new Dictionary<string, object>
                {
                    { "title", "Updated Case Title for Contact" },
                    { "description", "Updated Contact Description" },
                    { "statuscode", new OptionSetValue(3) },
                });

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
                // Delete created records
                SafelyDelete("incident", caseForAccountId);
                SafelyDelete("incident", caseForContactId);
                SafelyDelete("account", accountId);
                SafelyDelete("contact", contactId);
            }
        }

        /// <summary>
        /// Creates a case linked to a customer (account or contact)
        /// </summary>
        /// <param name="customerType">The customer type (e.g., "account", "contact")</param>
        /// <param name="customerId">The ID of the customer</param>
        /// <param name="title">The case title</param>
        /// <param name="priorityCode">The numeric priority code (e.g., 1 for High, 2 for Medium)</param>
        /// <param name="originCode">The numeric origin code (e.g., 1 for Phone Call, 2 for Email)</param>
        /// <param name="description">The description of the case</param>
        /// <returns>The GUID of the created case</returns>
        private Guid CreateCase(string customerType, Guid customerId, string title, int priorityCode, int originCode, string description)
        {
            return ValidateAndCreateEntity("incident", new Dictionary<string, object>
            {
                { "title", title },
                { "ticketnumber", Guid.NewGuid().ToString() },
                { "prioritycode", new OptionSetValue(priorityCode) }, 
                { "caseorigincode", new OptionSetValue(originCode) },   
                { "customerid", new EntityReference(customerType, customerId) },
                { "description", description }, 
                { "statuscode", new OptionSetValue(1) },
                { "createdon", DateTime.Now }
            });
        }

        /// <summary>
        /// Updates a case with specified fields
        /// </summary>
        /// <param name="caseId">The ID of the case to update</param>
        /// <param name="fieldsToUpdate">A dictionary of fields to update with their values</param>
        private void UpdateCase(Guid caseId, Dictionary<string, object> fieldsToUpdate)
        {
            if (fieldsToUpdate == null || fieldsToUpdate.Count == 0)
            {
                throw new ArgumentException("No fields provided to update the case.");
            }

            ValidateAndUpdateEntity("incident", caseId, fieldsToUpdate);
        }

    }
}
