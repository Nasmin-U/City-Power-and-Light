
namespace City.Helpers
{
    // Helper class to log messages to the console
    public static class Log
    {
  
        public static void Header(string title) // Method to display header message 
        {
            Console.WriteLine($"\n\n*************************** {title.ToUpper()} ***************************\n");
        }

        public static void SubHeader(string title) // Method to display subheader message
        {
            Console.WriteLine($"\n----- {title} -----\n");
        }

    } // End of Log class
}
