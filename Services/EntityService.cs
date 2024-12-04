using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace City.Services
{
    // EntityService class to perform CRUD operations on entities
    public class EntityService
    {
        // Private IOrganizationService instance to perform CRUD operations on entities in the organization service context 
        private readonly IOrganizationService _organizationService;

        // Constructor to initialize the IOrganizationService instance 
        public EntityService(IOrganizationService organizationService)
        {
            // Validate the organization service instance
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        // Method to create an entity record 
        public Guid CreateEntity(string entityName, AttributeCollection attributes) 
        {
            try
            {
                // Validate the entity name and attributes
                if (entityName == "incident" && !attributes.Contains("customerid"))
                {
                    throw new ArgumentException("The 'customerid' attribute is required for creating an incident.");
                }

                // Create an entity record
                var entity = new Entity(entityName) { Attributes = attributes };
                Guid entityId = _organizationService.Create(entity);
                Console.WriteLine($"Created {entityName} with ID: {entityId}");
                return entityId;
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error creating {entityName}: {ex.Message}");
                throw;
            }

        } // End of CreateEntity method

        // Method to retrieve an entity record
        public Entity ReadEntity(string entityName, Guid entityId) 
        {
            try
            {
                // Retrieve the entity record using the entity name and entity ID
                var entity = _organizationService.Retrieve(entityName, entityId, new ColumnSet(true));
                Console.WriteLine($"Retrieved {entityName} with ID: {entityId}");
                return entity;
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error retrieving {entityName} with ID {entityId}: {ex.Message}");
                throw;
            }

        } // End of ReadEntity method

        // Method to update an entity record
        public void UpdateEntity(string entityName, Guid entityId, AttributeCollection attributes) 
        {
            try
            {
                // Validate the entity name and attributes
                var entity = new Entity(entityName) { Id = entityId, Attributes = attributes };
                _organizationService.Update(entity);
                Console.WriteLine($"Updated {entityName} with ID: {entityId}\n");
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error updating {entityName} with ID {entityId}: {ex.Message}");
                throw;
            }

        } // End of UpdateEntity method

        // Method to delete an entity record
        public void DeleteEntity(string entityName, Guid entityId) 
        {
            try
            {
                // Delete the entity record using the entity name and entity ID
                _organizationService.Delete(entityName, entityId);
                Console.WriteLine($"Deleted {entityName} with ID: {entityId}");
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error deleting {entityName} with ID {entityId}: {ex.Message}");
                throw;
            }

        } // End of DeleteEntity method

        // Method to display entity attributes
        public void DisplayEntityAttributes(Entity entity)
        {

            // Define relevant attributes for each entity type in a dictionary 
            var relevantKeysByEntity = new Dictionary<string, HashSet<string>>
            {
                {
                    "account",
                    new HashSet<string> { "name", "createdon", "modifiedon", "emailaddress1", "telephone1" }
                },
                {
                    "contact",
                    new HashSet<string> { "firstname", "lastname", "createdon", "modifiedon", "emailaddress1", "telephone1" }
                },
                {
                    "incident",
                    new HashSet<string> { "title", "description", "customerid", "createdon", "modifiedon" }
                }
            };

            // Get relevant attributes for the entity type
            if (!relevantKeysByEntity.TryGetValue(entity.LogicalName, out var relevantKeys))
            {
                Console.WriteLine($"No attribute filter defined for entity type: {entity.LogicalName}. Showing all attributes.");
                relevantKeys = entity.Attributes.Keys.ToHashSet(); // Show all attributes if no filter is defined
            }

            // Display the relevant attributes for the entity record
            foreach (var attribute in entity.Attributes)
            {
                // Check if the attribute key is relevant for the entity type
                if (relevantKeys.Contains(attribute.Key))
                {
                   
                    if (attribute.Value is EntityReference entityRef)
                    {
                        Console.WriteLine($"{attribute.Key}: {entityRef.LogicalName} ({entityRef.Id})");
                    }
                    else if (attribute.Value is OptionSetValue optionSet)
                    {
                        Console.WriteLine($"{attribute.Key}: {optionSet.Value}");
                    }
                    else
                    {
                        // Display raw values for other attributes
                        Console.WriteLine($"{attribute.Key}: {attribute.Value}");
                    }
                }
            } 

        } // End of DisplayEntityAttributes method


    }
}
