using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using City.Services;
using Microsoft.Extensions.Configuration;
using City.Helpers;

namespace City
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the current directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load the appsettings.json file
                .Build(); // Build the configuration

            // Get the connection string
            string connectionString = configuration.GetConnectionString("Dataverse");

            // Create a new instance of the ServiceClient
            using (var serviceClient = new ServiceClient(connectionString))
            {
                if (!(serviceClient as ServiceClient).IsReady)
                {
                    Log.Header("Connection Failed");
                    Console.WriteLine(serviceClient.LastError);
                    return;
                }

                Log.Header($"Connected to {serviceClient.ConnectedOrgFriendlyName}");

                var entityService = new EntityService(serviceClient);

                try
                {
                    // Perform CRUD operations
                    AccountOperations(entityService);
                    ContactOperations(entityService);
                    CaseOperations(entityService);
                }
                catch (Exception ex)
                {
                    // Log any errors
                    Log.Header("Error");
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }

        } // End of Main

        // Account CRUD operations
        static void AccountOperations(EntityService entityService)
        {
            Log.Header("Account CRUD Operations");

            // Create a new account
            Guid accountId = Guid.Empty;

            try
            {
                // Create
                Log.SubHeader("Creating Account");

                // Create an attribute collection for the account
                var accountAttributes = new AttributeCollection
                {
                    { "name", "Account 1" },
                    { "telephone1", "1234567890" },
                    { "emailaddress1", "Account1@account.com" }
                };
          
                accountId = entityService.CreateEntity("account", accountAttributes);

                // Read
                Log.SubHeader("Reading Account");
                var readAccount = entityService.ReadEntity("account", accountId);
                entityService.DisplayEntityAttributes(readAccount); 

                // Update
                Log.SubHeader("Updating Account");

                // Create an attribute collection for the updated account
                var updatedAccountAttributes = new AttributeCollection
                {
                    { "name", "UPDATED Account1" },
                    { "telephone1", "9876543210" }
                };
                entityService.UpdateEntity("account", accountId, updatedAccountAttributes);

                // Confirm update using the Read operation
                var updatedAccount = entityService.ReadEntity("account", accountId);
                entityService.DisplayEntityAttributes(updatedAccount);
            }
            catch (Exception ex)
            {
                // Log any errors
                Console.WriteLine($"Error during account operations: {ex.Message}");
            }
            finally
            {
                // Delete the account if it was created
                if (accountId != Guid.Empty)
                {
                    Log.SubHeader("Deleting Account");
                    entityService.DeleteEntity("account", accountId);
                }
            }

        } // End of AccountOperations

        // Contact CRUD operations
        static void ContactOperations(EntityService entityService)
        {
            Log.Header("Contact CRUD Operations");

            // Create a new contact
            Guid contactId = Guid.Empty;

            try
            {
                // Create
                Log.SubHeader("Creating Contact");

                // Create an attribute collection for the contact
                var contactAttributes = new AttributeCollection
                {
                    { "firstname", "Jane" },
                    { "lastname", "Doe" },
                    { "emailaddress1", "jane@doe.com" }
                };
                contactId = entityService.CreateEntity("contact", contactAttributes);

                // Read
                Log.SubHeader("Reading Contact");
                var readContact = entityService.ReadEntity("contact", contactId);
                entityService.DisplayEntityAttributes(readContact);

                // Update
                Log.SubHeader("Updating Contact");

                // Create an attribute collection for the updated contact
                var updatedContactAttributes = new AttributeCollection
                {
                    { "firstname", "UPDATED" },
                    { "emailaddress1", "updated@doe.com" }
                };
                entityService.UpdateEntity("contact", contactId, updatedContactAttributes);

                // Confirm update using the Read operation
                var updatedContact = entityService.ReadEntity("contact", contactId);
                entityService.DisplayEntityAttributes(updatedContact);
            }
            catch (Exception ex)
            {
                // Log any errors
                Console.WriteLine($"Error during contact operations: {ex.Message}");
            }
            finally
            {
                // Delete the contact if it was created
                if (contactId != Guid.Empty)
                {
                    Log.SubHeader("Deleting Contact");
                    entityService.DeleteEntity("contact", contactId);
                }
            }

        } // End of ContactOperations

        // Case CRUD operations
        static void CaseOperations(EntityService entityService)
        {
            Log.Header("Case CRUD Operations");

            // Create a new case and customer (account or contact) for the case
            Guid customerId = Guid.Empty;
            Guid caseId = Guid.Empty;
            string customerType = string.Empty; // account or contact

            try
            {
                // Determine the customer type (account or contact) for the case to be created by the user
                Console.WriteLine("Would you like to create a case for an account or a contact? (Enter 'account' or 'contact')");
                customerType = Console.ReadLine()?.ToLower();

                // Create the customer based on the user input
                if (customerType == "account")
                {
                    Log.SubHeader("Creating Account for Case");

                    // Create Customer Account for Case
                    var accountAttributes = new AttributeCollection
                    {
                        { "name", "Jane Accounts" },
                        { "emailaddress1", "jane@accounts.com" },
                        { "telephone1", "123-456-7890" }
                    };

                    customerId = entityService.CreateEntity("account", accountAttributes);
                }

                else if (customerType == "contact")
                {
                    Log.SubHeader("Creating Contact for Case");

                    // Create Customer Contact for Case
                    var contactAttributes = new AttributeCollection
                    {
                        { "firstname", "Casey" },
                        { "lastname", "Contact" },
                        { "emailaddress1", "casey@contact.com" }
                    };

                    customerId = entityService.CreateEntity("contact", contactAttributes);
                }
                else
                {
                    // Throw an exception if the user input is invalid (not 'account' or 'contact')
                    throw new ArgumentException("Invalid customer type. Please enter 'account' or 'contact'.");
                }


                // Create Case
                Log.SubHeader("Creating Case");

                // Create an attribute collection for the case
                var caseAttributes = new AttributeCollection
                {
                    { "title", "Case Title for Customer" },
                    { "description", "Description for the case reported by the customer." },
                    { "customerid", new EntityReference(customerType, customerId) } // Link to the selected customer type
                };

                caseId = entityService.CreateEntity("incident", caseAttributes);

                // Read
                Log.SubHeader("Reading Case");
                var readCase = entityService.ReadEntity("incident", caseId);
                entityService.DisplayEntityAttributes(readCase);

                // Update
                Log.SubHeader("Updating Case");

                // Create an attribute collection for the updated case
                var updatedCaseAttributes = new AttributeCollection
                {
                    { "title", "Updated Case Title for Customer" },
                    { "description", "Case updated with new customer information." }
                };

                entityService.UpdateEntity("incident", caseId, updatedCaseAttributes);

                // Confirm update using the Read operation
                Log.SubHeader("Retrieving Updated Case");
                var updatedCase = entityService.ReadEntity("incident", caseId);
                entityService.DisplayEntityAttributes(updatedCase);
            }
            catch (Exception ex)
            {
                // Log any errors
                Console.WriteLine($"Error during case operations: {ex.Message}");
            }
            finally
            {
                // Delete the case and customer if they were created
                if (caseId != Guid.Empty)
                {
                    Log.SubHeader("Deleting Case");
                    entityService.DeleteEntity("incident", caseId);
                }

                if (customerId != Guid.Empty)
                {
                    Log.SubHeader($"Deleting {((customerId == Guid.Empty) ? "Account" : "Contact")}");
                    entityService.DeleteEntity(customerType, customerId);
                }
            }

        } // End of CaseOperations

    } // End of Program

}// End of namespace City
