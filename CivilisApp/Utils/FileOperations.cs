namespace CivilisApp.Utils
{
    public static class FileOperations
    {
        public static void DeleteFileOlderThanXDays(string directoryPath, int XDays)
        {
            if (Directory.Exists(directoryPath))
            {
                // Get all files in the directory
                var files = Directory.GetFiles(directoryPath);

                // Loop through each file in the directory
                foreach (var file in files)
                {
                    try
                    {
                        // Get the last modified date of the file
                        DateTime lastModified = File.GetLastWriteTime(file);

                        // Check if the file is older than X days
                        if (lastModified < DateTime.Now.AddDays(-XDays))
                        {
                            // Delete the file
                            //Console.WriteLine($"Deleting file: {file}");
                            File.Delete(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine($"Error processing file {file}: {ex.Message}");
                    }
                }
                //Console.WriteLine("File cleanup completed.");
            }
            else
            {
                //Console.WriteLine($"Directory not found: {directoryPath}");
            }

        }
    }
}
