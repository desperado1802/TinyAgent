using Agent.Core.Services;
using Google.GenAI.Types;

namespace Agent.Core.Tools;


public class ToolDispatcher(FileService _fileService)
{
    private readonly FileService fileService = _fileService;


    public Content CallFunction(FunctionCall functionCall, bool verbose = false)
    {
        string workingDir = "SmallScripts";
        string result;
        if (verbose)
        {
            Console.WriteLine($"Calling function {functionCall.Name} ({functionCall.Args?.ToString()})");
        }
        else
        {
            Console.WriteLine($"Calling function {functionCall.Name}");
        }
        // GetFilesInfo, WriteFile, GetFileContent, RunCSFile
        switch (functionCall.Name)
        {
            case "GetFilesInfo":
                result = fileService.GetFilesInfo(workingDir, functionCall?.Args?["directory"]?.ToString() ?? "");
                break;

            case "WriteFile":
                result = fileService.WriteFile(workingDir, functionCall?.Args?["filePath"].ToString() ?? "", functionCall?.Args?["content"].ToString() ?? "");
                break;

            case "GetFileContent":
                result = fileService.GetFileContent(workingDir, functionCall?.Args?["filePath"].ToString() ?? "");
                break;

            case "RunCSFile":
                string[] args = [];
                object? rawArgs = null;
                functionCall.Args?.TryGetValue("args", out rawArgs);
                if (rawArgs is IEnumerable<object> jsonArray)
                {
                    args = jsonArray.Select(x => x?.ToString() ?? "")?.ToArray() ?? [];
                }
                result = fileService.RunCSFile(workingDir, functionCall?.Args?["filePath"].ToString() ?? "", args);
                break;

            default:
                return new()
                {
                    Role = "tool",
                    Parts = [
                        Part.FromFunctionResponse(name: functionCall?.Name ?? "", response: new Dictionary<string, object>(){
                        ["result"] = $"error: unknown function: {functionCall?.Name}"
                    })
                    ]
                };

        }

        return new()
        {
            Role = "tool",
            Parts = [
                Part.FromFunctionResponse(name: functionCall?.Name ?? "", response: new Dictionary<string, object>(){
                    ["result"] = result
                })
            ]
        };


    }
}
