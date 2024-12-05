using Microsoft.Xrm.Sdk;

namespace City.Helpers
{
    /// <summary>
    /// ConsoleFormatter class to format console output
    /// </summary>
    public static class ConsoleFormatter
    {

        /// <summary>
        /// Utility class for logging messages to the console
        /// </summary>
        public static class ConsoleLogger
        {


            /// <summary>
            /// Log an informational message to the console in cyan for information
            /// </summary>
            /// <param name="message">The message to log</param> 
            public static void Info(string message)
            {
                Console.ForegroundColor = ConsoleColor.Cyan; 
                Console.WriteLine($"INFO: {message}");
                Console.ResetColor();
            }

            /// <summary>
            /// Log an informational message to the console in red for errors
            /// </summary>
            /// <param name="message">The message to log</param> 
            public static void Error(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red; // Set text color to red
                Console.WriteLine($"ERROR: {message}");
                Console.ResetColor();
            }

            /// <summary>
            /// Log an informational message to the console in green for success
            /// </summary>
            /// <param name="message">The message to log</param> 
            public static void Success(string message)
            {
                Console.ForegroundColor = ConsoleColor.Green; // Set text color to green
                Console.WriteLine($"SUCCESS: {message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Display a header message
        /// </summary>
        /// <param name="title">The title of the header message</param> 
        public static void Header(string title)
        {
            Console.WriteLine($"\n\n*************************** {title.ToUpper()} ***************************\n");
        }



        /// <summary>
        /// Display the attributes of an entity
        /// </summary>
        /// <param name="entity">The entity to display attributes for</param> 
        public static void DisplayEntityAttributes(Entity entity)
        {
            // Define relevant attributes for each entity type in a dictionary
            var relevantKeysByEntity = new Dictionary<string, HashSet<string>>
            {
                { "account", new HashSet<string> { "name", "createdon", "modifiedon", "emailaddress1", "telephone1" } },
                { "contact", new HashSet<string> { "firstname", "lastname", "createdon", "modifiedon", "emailaddress1", "telephone1" } },
                { "incident", new HashSet<string> { "title", "description", "customerid", "createdon", "modifiedon" } }
            };

            if (!relevantKeysByEntity.TryGetValue(entity.LogicalName, out var relevantKeys))
            {
                Header($"No filter for {entity.LogicalName}. Displaying all attributes.");
                relevantKeys = entity.Attributes.Keys.ToHashSet(); // Display all attributes if no filter is defined
            }

            foreach (var attribute in entity.Attributes)
            {
                if (relevantKeys.Contains(attribute.Key))
                {
                    Console.WriteLine(FormatAttribute(attribute.Key, attribute.Value));
                }
            }
        }

        /// <summary>
        /// Format an attribute for display
        /// </summary>
        /// <param name="key">The key of the attribute to format</param> 
        /// <param name="value">The value of the attribute to format</param> 
        /// <returns>The formatted attribute</returns> 
        private static string FormatAttribute(string key, object value)
        {
            return value switch
            {
                EntityReference entityRef => $"{key}: {entityRef.LogicalName} ({entityRef.Id})",
                OptionSetValue optionSet => $"{key}: {optionSet.Value}",
                _ => $"{key}: {value}"
            };
        }
    } 
}
