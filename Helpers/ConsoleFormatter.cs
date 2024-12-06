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
                Console.WriteLine($"\nINFO: {message}");
                Console.ResetColor();
            }

            /// <summary>
            /// Log an informational message to the console in red for errors
            /// </summary>
            /// <param name="message">The message to log</param> 
            public static void Error(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {message}");
                Console.ResetColor();
            }

            /// <summary>
            /// Log an informational message to the console in green for success
            /// </summary>
            /// <param name="message">The message to log</param> 
            public static void Success(string message)
            {
                Console.ForegroundColor = ConsoleColor.Green;
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
            var relevantKeysByEntity = GetRelevantColumnsForEntity(entity.LogicalName);

            if (!relevantKeysByEntity.Any())
            {
                Header($"No predefined columns for {entity.LogicalName}. Displaying all attributes.");
                relevantKeysByEntity = entity.Attributes.Keys.ToHashSet();
            }

            foreach (var key in relevantKeysByEntity)
            {
                if (entity.Attributes.ContainsKey(key))
                {
                    Console.WriteLine(FormatAttribute(key, entity.Attributes[key]));
                }
                else
                {
                    Console.WriteLine($"{key}: [N/A]");
                }
            }
        }

        /// <summary>
        /// Get the relevant columns for an entity
        /// </summary>
        /// <param name="entityName">The name of the entity to get relevant columns for</param> 
        /// <returns></returns>
        private static HashSet<string> GetRelevantColumnsForEntity(string entityName)
        {
            return entityName.ToLower() switch
            {
                "incident" => new HashSet<string> { "title", "ticketnumber", "prioritycode", "caseorigincode", "customerid", "statuscode", "createdon" },
                "account" => new HashSet<string> { "name", "telephone1", "address1_city", "primarycontactid" },
                "contact" => new HashSet<string> { "fullname", "emailaddress1", "parentcustomerid", "telephone1" },
                _ => new HashSet<string>()
            };
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
                EntityReference entityRef => $"{key}: {entityRef.Name ?? entityRef.Id.ToString()}",
                OptionSetValue optionSet => $"{key}: {GetOptionSetText(key, optionSet.Value)} - [{optionSet.Value}]",
                DateTime dateTime => $"{key}: {dateTime:G}",
                _ => $"{key}: {value}"
            };
        }

        /// <summary>
        /// Gets the text representation of an OptionSet value
        /// </summary>
        /// <param name="key">The attribute name (e.g., "caseorigincode")</param>
        /// <param name="value">The OptionSet value (e.g., 1)</param>
        /// <returns>The text representation of the OptionSet value</returns>
        private static string GetOptionSetText(string key, int value)
        {
            return key.ToLower() switch
            {
                "statuscode" => value switch
                {
                    1 => "Active",
                    2 => "Resolved",
                    3 => "Canceled",
                    _ => "Unknown Status"
                },
                "caseorigincode" => value switch
                {
                    1 => "Phone Call",
                    2 => "Email",
                    3 => "Web",
                    _ => "Unknown Origin"
                },
                "prioritycode" => value switch
                {
                    1 => "High",
                    2 => "Medium",
                    3 => "Low",
                    _ => "Unknown Priority"
                },
                _ => "Unknown OptionSet"
            };
        }
    } 
}
