using System.IO.Enumeration;

namespace Agent.Core.Services;


public class FileService : IFileService
{

    private static readonly int MAX_CHARS = 10000;

    public string GetFilesInfo(string workingDirectory, string? directory = null)
    {
        var absoluteDir = Path.GetFullPath(workingDirectory);
        var workingDir = String.IsNullOrEmpty(directory) ? absoluteDir : Path.GetFullPath(Path.Combine(absoluteDir, directory));

        string[] contents;


        if (!workingDir.StartsWith(absoluteDir))
        {
            return $"Error {workingDir} is outside the allowed working directory!";
        }

        if (!Directory.Exists(workingDir))
        {
            return $"Error: Directory '{directory}' not found.";
        }

        contents = Directory.GetFileSystemEntries(workingDir);
        string finalResponse = "";
        foreach (var content in contents)
        {
            var contentPath = Path.Combine(absoluteDir, content);

            FileAttributes attr = File.GetAttributes(contentPath);
            bool isDir = attr.HasFlag(FileAttributes.Directory);

            long size = 0;
            if (!isDir)
            {
                size = new FileInfo(contentPath).Length;
            }
            else
            {
                size = Directory.EnumerateFiles(content, "*", SearchOption.AllDirectories).Select(file => new FileInfo(file).Length).Sum();
            }

            finalResponse += ($"- {Path.GetFileName(content)}: file_size={size} bytes, is dir={isDir}\n");
        }
        Console.WriteLine(finalResponse);
        return finalResponse;
    }



    public string GetFileContent(string workingDirectory, string filePath)
    {
        var absoluteDir = Path.GetFullPath(workingDirectory);
        var absoluteFilePath = Path.GetFullPath(Path.Combine(absoluteDir, filePath));

        char[] buffer = new char[MAX_CHARS];
        string content = "";


        if (!absoluteFilePath.StartsWith(absoluteDir))
        {
            return $"Error {filePath} is outside the allowed working directory!";
        }

        if (!File.Exists(absoluteFilePath))
        {
            return $"Error: Directory '{filePath}' not found.";
        }



        using FileStream stream = new(absoluteFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader reader = new(stream);

        if (stream.CanRead)
        {
            int charsRead = reader.ReadBlock(buffer, 0, MAX_CHARS);


            content = new string(buffer, 0, charsRead);

            if (charsRead >= MAX_CHARS)
            {
                content += $"...File {filePath} truncated at 10000 characters";
            }
        }
        else
        {
            content = "could not read the file";
        }
        Console.WriteLine(content);
        return content;
    }

    public string WriteFile(string workingDirectory, string filePath, string content)
    {
        var absoluteDir = Path.GetFullPath(workingDirectory);
        var absoluteFilePath = Path.GetFullPath(Path.Combine(absoluteDir, filePath));

        char[] buffer = new char[MAX_CHARS];

        if (!Directory.Exists(absoluteDir))
        {
            Console.WriteLine($"This absolute dir does not exist {absoluteDir}");
            return $"This absolute dir does not exist {absoluteDir}";
        }

        if (!absoluteFilePath.StartsWith(absoluteDir))
        {
            return $"Error {filePath} is outside the allowed working directory!";
        }


        // if file does not exist, we want to create a new file
        string? parentDir = Path.GetDirectoryName(absoluteFilePath);


        try
        {
            if (parentDir != null && !Directory.Exists(parentDir))
            {
                Directory.CreateDirectory(parentDir);
            }
            File.WriteAllText(absoluteFilePath, content);
            Console.WriteLine($"success writing to file {content.Length} characters");
            return $"success writing to file ${content.Length} characters";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return $"Error while writing to file {filePath}: {ex.Message}";
        }

    }

}
