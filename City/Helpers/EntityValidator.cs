using Microsoft.Xrm.Sdk;
using System.Text.RegularExpressions;

namespace City.Helpers
{
    /// <summary>
    /// Validates entity attributes
    /// </summary>
    public static class EntityValidator
    {
        private static readonly Dictionary<string, List<string>> MandatoryFieldsByEntity = new()
        {
            { "account", new List<string> { "name", "emailaddress1" } },
            { "contact", new List<string> { "firstname", "lastname", "emailaddress1" } },
            { "incident", new List<string> { "title", "description", "customerid" } }
        };

        /// <summary>
        /// Validates attributes for an entity, including mandatory fields (if create operation) and field formats.
        /// </summary>
        /// <param name="entityName">The entity type</param>
        /// <param name="attributes">The attribute collection</param>
        /// <param name="isCreate">True if validation is for creation; false for updates</param>
        /// <exception cref="ArgumentException">Thrown if validation fails</exception>
        public static void Validate(string entityName, AttributeCollection attributes, bool isCreate = true)
        {
            if (isCreate)
            {
                ValidateMandatoryFields(entityName, attributes);
            }
            ValidateFieldFormats(attributes);
        }

        /// <summary>
        /// Validates mandatory fields for an entity type based on a dictionary of mandatory fields by entity.
        /// </summary>
        /// <param name="entityName">The entity type</param> 
        /// <param name="attributes">The attribute collection</param> 
        /// <exception cref="ArgumentException">Thrown if mandatory fields are missing</exception> 
        private static void ValidateMandatoryFields(string entityName, AttributeCollection attributes)
        {
            if (MandatoryFieldsByEntity.TryGetValue(entityName, out var mandatoryFields))
            {
                var missingFields = new List<string>();
                foreach (var field in mandatoryFields)
                {
                    if (!attributes.Contains(field) || attributes[field] == null)
                    {
                        missingFields.Add(field);
                    }
                }

                if (missingFields.Count > 0)
                {
                    throw new ArgumentException($"Mandatory fields missing for '{entityName}': {string.Join(", ", missingFields)}");
                }
            }
        }

        /// <summary>
        /// Validates field formats for email and phone number fields.
        /// </summary>
        /// <param name="attributes">The attribute collection to validate</param> 
        /// <exception cref="ArgumentException">Thrown if email or phone number fields have invalid formats</exception> 
        private static void ValidateFieldFormats(AttributeCollection attributes)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.Key.EndsWith("emailaddress", StringComparison.OrdinalIgnoreCase))
                {
                    if (!IsValidEmail(attribute.Value.ToString()))
                    {
                        throw new ArgumentException($"Invalid email format for field '{attribute.Key}': {attribute.Value}");
                    }
                }
                else if (attribute.Key.EndsWith("telephone", StringComparison.OrdinalIgnoreCase))
                {
                    if (!IsValidPhoneNumber(attribute.Value.ToString()))
                    {
                        throw new ArgumentException($"Invalid phone number format for field '{attribute.Key}': {attribute.Value}");
                    }
                }
            }
        }

        /// <summary>
        /// Validates an email address
        /// </summary>
        /// <param name="email">The email address to validate</param> 
        /// <returns>True if the email address is valid; false otherwise</returns>
        private static bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }

        /// <summary>
        /// Validates a phone number
        /// </summary>
        /// <param name="phone">The phone number to validate</param> 
        /// <returns>True if the phone number is valid; false otherwise</returns> 
        private static bool IsValidPhoneNumber(string phone)
        {
            var phoneRegex = @"^\+?[0-9]{10,15}$"; 
            return Regex.IsMatch(phone, phoneRegex);
        }
    }
}
