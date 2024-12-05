using Microsoft.Xrm.Sdk;
using City.Services;
using static City.Helpers.ConsoleFormatter;

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
