using City.Services;
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
            Guid contactId = Guid.Empty;

            try
            {
                // Create and validate contact
                ConsoleLogger.Info("Creating Contact");
                var attributes = new Dictionary<string, object>
                {
                    { "firstname", "Jane" },
                    { "lastname", "Doe" },
                    { "emailaddress1", "jane.doe@contact.com" }
                };
                var attributeCollection = CreateAttributes(attributes);
                Validate("contact", attributeCollection, isCreate: true);
                contactId = _entityService.CreateEntity("contact", attributeCollection);


                // Read contact
                ConsoleLogger.Info("Reading Contact");
                var contact = _entityService.ReadEntity("contact", contactId);
                DisplayEntityAttributes(contact);


                // Update and validate contact
                ConsoleLogger.Info("Updating Contact");
                var updatedAttributes = new Dictionary<string, object>
                {
                    { "firstname", "Updated Jane" },
                    { "emailaddress1", "updated.jane@contact.com" }
                };
                var updatedAttributeCollection = CreateAttributes(updatedAttributes);
                Validate("contact", updatedAttributeCollection, isCreate: false);
                _entityService.UpdateEntity("contact", contactId, updatedAttributeCollection);


                // Read updated contact
                var updatedContact = _entityService.ReadEntity("contact", contactId);
                DisplayEntityAttributes(updatedContact);
            }
            catch (ArgumentException ex)
            {
                ConsoleLogger.Error($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Error during contact operations: {ex.Message}");
            }
            finally
            {
                // Delete All Created Records
                SafelyDelete("contact", contactId);
            }
        }
    }
}
