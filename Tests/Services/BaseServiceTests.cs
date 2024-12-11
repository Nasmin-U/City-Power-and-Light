using Microsoft.Xrm.Sdk;
using Moq;
using City.Service;
using City.Services;

namespace Tests.Services
{
    [TestFixture]
    public class BaseServiceTests
    {
        private const string AccountEntity = "account";

        /// <summary>
        /// Tests that ValidateAndCreateEntity throws an ArgumentException when attributes are missing
        /// </summary>
        [Test]
        public void ValidateAndCreateEntity_WithMissingAttributes_ThrowsArgumentException()
        {
            // Arrange
            var baseService = CreateTestableBaseService();
            var invalidAttributes = new Dictionary<string, object>(); // Create an empty dictionary of attributes

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                baseService.ValidateAndCreateEntity(AccountEntity, invalidAttributes)
            );
        }

        /// <summary>
        /// Tests that ValidateAndCreateEntity throws an ArgumentException when attributes are invalid for the entity
        /// </summary>
        private class TestBaseService : BaseService
        {
            /// <summary>
            /// Initializes a new instance of the TestBaseService class
            /// </summary>
            /// <param name="entityService">The entity service to use for operations</param> 
            public TestBaseService(EntityService entityService) : base(entityService) { }

            /// <summary>
            /// Validates attributes and creates an entity for testing
            /// </summary>
            /// <param name="entityName">The name of the entity to create</param> 
            /// <param name="attributes">The attributes for the entity</param>  
            /// <returns></returns>
            public Guid ValidateAndCreateEntity(string entityName, Dictionary<string, object> attributes)
            {
                return base.ValidateAndCreateEntity(entityName, attributes);
            }
        }

        /// <summary>
        /// Creates a testable instance of the BaseService class for testing
        /// </summary>
        /// <returns>The testable BaseService instance</returns>  
        private TestBaseService CreateTestableBaseService()
        {
            var mockOrganizationService = new Mock<IOrganizationService>();
            var entityService = new EntityService(mockOrganizationService.Object);
            return new TestBaseService(entityService);
        }
    }
}
