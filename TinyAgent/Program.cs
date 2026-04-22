using Microsoft.Extensions.Configuration;
using Agent.Core;

class Program
{

    public static async Task CallAI(string content)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        string? apiKey = config["Gemini:ApiKey"];

        if (String.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Ops no api key bitch!");
        }
        else
        {
            var orchestrator = new Orchestrator(apiKey);

            await orchestrator.GenerateContent(content);
        }
    }


    public async static Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Please provide a prompt!");
        }
        else
        {
            await CallAI(String.Join(" ", args));
        }
    }
}
