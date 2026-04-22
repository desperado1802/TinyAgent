namespace Agent.Core.Services;

interface IFileService
{
    public string GetFilesInfo(string workingDirectory, string? directory);
}
