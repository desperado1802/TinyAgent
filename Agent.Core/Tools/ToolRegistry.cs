using Google.GenAI;
using Google.GenAI.Types;
using Type = Google.GenAI.Types.Type;

namespace Agent.Core.Tools;

public static class ToolRegistry
{
    public static List<Tool> GetAvailableTools()
    {
        return [
            new Tool {
                FunctionDeclarations = [
                    GetFilesInfoDeclaration,
                    WriteFileDeclaration,
                    GetFileContents,
                    RunCSFileDeclaration
                ]
            }
        ];

    }

    public static readonly FunctionDeclaration GetFilesInfoDeclaration = new()
    {
        Name = "GetFilesInfo",
        Description = "Lists all files and directories in a given path. Use this to explore the project structure, check if files exist, or see file sizes.",
        Parameters = new Schema
        {
            Type = Type.Object,
            Properties = new Dictionary<string, Schema>
            {
                ["directory"] = new Schema { Type = Type.String, Description = "Optional. The sub-directory to list (e.g., 'logs' or 'scripts'). Leave empty to list the root directory." }
            }
        }
    };

    public static readonly FunctionDeclaration GetFileContents = new()
    {
        Name = "GetFileContent",
        Description = "Gets the file's contents, up to 10k chars",
        Parameters = new Schema
        {
            Type = Type.Object,
            Properties = new Dictionary<string, Schema>
            {
                ["filePath"] = new Schema { Type = Type.String, Description = "The file path that we want to read" }
            },
            Required = ["filePath"]
        }
    };

    public static readonly FunctionDeclaration WriteFileDeclaration = new()
    {
        Name = "WriteFile",
        Description = "Creates or overwrites a C# file with the provided code.",
        Parameters = new Schema
        {
            Type = Type.Object,
            Properties = new Dictionary<string, Schema>
            {
                ["filePath"] = new Schema { Type = Type.String, Description = "The file to write in" },
                ["content"] = new Schema { Type = Type.String, Description = "The content that will be written to file" }
            },
            Required = ["filePath", "content"]
        }
    };

    public static readonly FunctionDeclaration RunCSFileDeclaration = new()
    {
        Name = "RunCSFile",
        Description = "Runs a CS(C sharp) file with optional arguments.",
        Parameters = new Schema
        {
            Type = Type.Object,
            Properties = new Dictionary<string, Schema>
            {
                ["filePath"] = new Schema { Type = Type.String, Description = "The CS file to execute" },
                ["args"] = new Schema { Type = Type.Array, Items = new Schema { Type = Type.String }, Description = "Optional arguments" }
            },
            Required = ["filePath"]
        }

    };


}
