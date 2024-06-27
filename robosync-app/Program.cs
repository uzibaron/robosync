using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        string configPath = "config.json";
        try
        {
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));

            foreach (var serverIP in config!.Servers)
            {
                var fileDestination = $"\\\\{serverIP}{config.ServerSharedRoot}";

                foreach (var localFile in config.LocalFiles)
                {
                    var file = new DirectoryInfo(localFile);
                    var source = new DirectoryInfo(file!.Parent!.ToString());
                    var sourceFolder = source.ToString();
                    var destinationFolder = source.Name == config.LocalRoot ? "" : source.Name;
                    var destination = $"\\\\{serverIP}{config.ServerSharedRoot}\\{destinationFolder}";

                    SynchronizeFile(sourceFolder, destination, file.Name);

                }

                foreach (var localFolder in config.LocalFolders)
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


        var output = process.StandardOutput.ReadToEnd();
        var errors = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(errors))
        {
            Console.WriteLine($"Errors during sync: {errors}");
        }

        Console.WriteLine(output);
    }

    static void SynchronizeFile(string source, string destination, string filename)
    {
        Console.WriteLine($"Syncing {source} to {destination}");

        var startInfo = new ProcessStartInfo()
        {
            FileName = "robocopy",
            Arguments = $"{source} {destination} {filename}  /Z /R:5 /W:5",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var process = Process.Start(startInfo);


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
    public string LocalRoot { get; set; }
    public string[] LocalFiles { get; set; }
    public string ServerSharedRoot { get; set; }
    public string[] Servers { get; set; }
}
