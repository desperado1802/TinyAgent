namespace Agent.Core.Services;

interface IFileService
{
    public string GetFilesInfo(string workingDirectory, string? directory);

    public string GetFileContent(string workingDirectory, string filePath);

    public string WriteFile(string workingDirectory, string filePath, string content);
}
