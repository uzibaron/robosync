using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        string configPath = "config.json";
        try
        {
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));

            foreach (var localFolder in config.LocalFolders)
            {
                foreach (var serverIP in config.Servers)
                {
                    var destination = $"\\\\{serverIP}{config.ServerSharedRoot}\\{new DirectoryInfo(localFolder).Name}";
                    SynchronizeFolder(localFolder, destination);
                }
            }

            Console.WriteLine("Sync completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.Message}");
        }
    }

    static void SynchronizeFolder(string source, string destination)
    {
        Console.WriteLine($"Syncing {source} to {destination}");

        // Ensure destination directory exists (Handled by Robocopy /MIR)
        var startInfo = new ProcessStartInfo()
        {
            FileName = "robocopy",
            Arguments = $"{source} {destination} /MIR /Z /R:5 /W:5",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var process = Process.Start(startInfo);
        process.WaitForExit();

        var output = process.StandardOutput.ReadToEnd();
        var errors = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(errors))
        {
            Console.WriteLine($"Errors during sync: {errors}");
        }

        Console.WriteLine(output);
    }
}

class Config
{
    public string[] LocalFolders { get; set; }
    public string ServerSharedRoot { get; set; }
    public string[] Servers { get; set; }
}
