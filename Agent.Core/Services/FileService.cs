using System.IO.Enumeration;

namespace Agent.Core.Services;


public class FileService : IFileService
{

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

}
