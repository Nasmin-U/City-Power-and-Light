using Microsoft.Xrm.Sdk;
using City.Services;
using static City.Helpers.ConsoleFormatter;
using static City.Helpers.EntityValidator;

namespace City.Service
{
    /// <summary>
    /// BaseService class providing shared utilities for entity operations
    /// </summary>
    public abstract class BaseService
    {
        protected readonly EntityService _entityService;

        /// <summary>
        /// Constructor for BaseService
        /// </summary>
        /// <param name="entityService"></param>
        public BaseService(EntityService entityService)
        {
            _entityService = entityService;
        }

        /// <summary>
        /// Validates attributes and creates an entity
        /// </summary>
        /// <param name="entityName">The name of the entity</param>
        /// <param name="attributes">The attributes for the entity</param>
        /// <returns>The GUID of the created entity</returns>
        protected Guid ValidateAndCreateEntity(string entityName, Dictionary<string, object> attributes)
        {
            var attributeCollection = CreateAttributes(attributes);
            Validate(entityName, attributeCollection, isCreate: true);
            return _entityService.CreateEntity(entityName, attributeCollection);
        }

        /// <summary>
        /// Validates attributes and updates an entity
        /// </summary>
        /// <param name="entityName">The name of the entity</param>
        /// <param name="entityId">The ID of the entity to update</param>
        /// <param name="attributes">The attributes to update</param>
        protected void ValidateAndUpdateEntity(string entityName, Guid entityId, Dictionary<string, object> attributes)
        {
            var attributeCollection = CreateAttributes(attributes);
            Validate(entityName, attributeCollection, isCreate: false);
            _entityService.UpdateEntity(entityName, entityId, attributeCollection);
        }


        /// <summary>
        /// Create an AttributeCollection from a dictionary of attributes
        /// </summary>
        /// <param name="attributes">Dictionary of attributes</param>
        /// <returns>AttributeCollection</returns>
        protected AttributeCollection CreateAttributes(Dictionary<string, object> attributes)
        {
            var attributeCollection = new AttributeCollection();
            foreach (var kvp in attributes)
            {
                attributeCollection.Add(kvp.Key, kvp.Value);
            }
            return attributeCollection;
        }

        /// <summary>
        /// Safely delete an entity by checking if the ID is not empty
        /// </summary>
        /// <param name="entityName">Entity name</param>
        /// <param name="entityId">Entity ID</param>
        protected void SafelyDelete(string entityName, Guid entityId)
        {
            if (entityId != Guid.Empty)
            {
                ConsoleLogger.Info($"Deleting {entityName} with ID: {entityId}");
                _entityService.DeleteEntity(entityName, entityId);
                ConsoleLogger.Success($"{entityName} {entityId} deleted");

                return;
            }
            else
            {
                ConsoleLogger.Info($"No {entityName} to delete");
            }
        }


    }
}
