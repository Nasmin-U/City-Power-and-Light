using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace City.Services
{
    /// <summary>
    /// Service to perform CRUD operations on entities
    /// </summary>
    public class EntityService
    {
        private readonly IOrganizationService _organizationService;

        /// <summary>
        /// Initializes a new instance of the EntityService class
        /// </summary>
        /// <param name="organizationService">The organization service to use</param> 
        /// <exception cref="ArgumentNullException">Thrown when organizationService is null</exception>
        public EntityService(IOrganizationService organizationService)
        {
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        /// <summary>
        /// Create an entity record
        /// </summary>
        /// <param name="entityName">The name of the entity to create</param>
        /// <param name="attributes">The attributes for the entity</param>
        /// <returns>The ID of the created entity</returns>
        public Guid CreateEntity(string entityName, AttributeCollection attributes)
        {
            var entity = new Entity(entityName) { Attributes = attributes };
            return _organizationService.Create(entity);
        }

        /// <summary>
        /// Retrieve an entity record
        /// </summary>
        /// <param name="entityName">The name of the entity to retrieve</param>
        /// <param name="entityId">The ID of the entity to retrieve</param>
        /// <returns>The retrieved entity</returns>
        public Entity ReadEntity(string entityName, Guid entityId)
        {
            return _organizationService.Retrieve(entityName, entityId, new ColumnSet(true));
        }

        /// <summary>
        /// Update an entity record
        /// </summary>
        /// <param name="entityName">The name of the entity to update</param>
        /// <param name="entityId">The ID of the entity to update</param>
        /// <param name="attributes">The attributes to update</param>
        public void UpdateEntity(string entityName, Guid entityId, AttributeCollection attributes)
        {
            var entity = new Entity(entityName) { Id = entityId, Attributes = attributes };
            _organizationService.Update(entity);
        }

        /// <summary>
        /// Delete an entity record
        /// </summary>
        /// <param name="entityName">The name of the entity to delete</param>
        /// <param name="entityId">The ID of the entity to delete</param>
        public void DeleteEntity(string entityName, Guid entityId)
        {
            _organizationService.Delete(entityName, entityId);
        }
    }
}
