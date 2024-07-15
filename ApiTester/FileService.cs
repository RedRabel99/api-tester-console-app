namespace api_tester_console_app
{
    public class FileService
    {
        private string? GetFilePath()
        {
            while (true)
            {
                Console.WriteLine("Please enter path:");
                var input = Console.ReadLine();
                if (File.Exists(input))
                {
                    return input;
                }

                Console.WriteLine("File not found. Do you want to try again?");
                if (!MenuManager.GetConfirmation())
                {
                    return null;
                }
            }
        }

        public string? GetFileContentView()
        {
            var path = GetFilePath();
            if (path == null)
            {
                return null;
            }

            try
            {
                var content = File.ReadAllText(path);
                return content;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to access this file.");
                return null;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An I/O error occurred: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}