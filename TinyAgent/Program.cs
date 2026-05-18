using Microsoft.Extensions.Configuration;
using Agent.Core;
using Agent.Core.Services;

class Program
{

    public static async Task CallAI(string[] args)
    {
        bool verbose = false;
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a prompt!");
            return;
        }
        else if (args.Length == 2 && args[1] == "--verbose")
        {
            verbose = true;
        }

        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        string? apiKey = config["Gemini:ApiKey"];
        string content = args[0];

        if (String.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Ops no api key bitch!");
        }
        else
        {
            var orchestrator = new Orchestrator(apiKey, verbose);

            await orchestrator.GenerateContent(content);
        }
    }


    public async static Task Main(string[] args)
    {
        FileService fileService = new();

        // fileService.GetFilesInfo(workingDirectory: "SmallScripts");
        // fileService.GetFileContent(workingDirectory: "SmallScripts", filePath: "lorem.txts");
        // fileService.WriteFile(workingDirectory: "SmallScripts", filePath: "karen.txt", content: """Console.WriteLine("agent writing shit");""");
        // fileService.RunCSFile(workingDirectory: "SmallScripts", filePath: "Test.cs");
        await CallAI(args);
    }
}
