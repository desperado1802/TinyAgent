namespace Agent.Core;

using System;
using Agent.Core.Services;
using Agent.Core.Tools;

public class Orchestrator
{
    private readonly string apiKey;
    private readonly bool verbose;
    private readonly FileService fileService;
    private readonly ToolDispatcher toolDispatcher;

    public Orchestrator(string apiKey, bool verbose)
    {
        this.apiKey = apiKey;
        this.verbose = verbose;
        this.fileService = new();
        this.toolDispatcher = new(this.fileService);
    }

    // create test call to gemini ai
    public async Task GenerateContent(string content)
    {

        var geminiService = new GeminiClient(apiKey, toolDispatcher);

        try
        {
            var res = await geminiService.GenerateContentResponse(content);

            Console.WriteLine($"Gemini says: {res.Text}");

            if (verbose)
            {
                Console.WriteLine($"Input tokens used: {res?.UsageMetadata?.PromptTokenCount}");
                Console.WriteLine($"Output tokens used: {res?.UsageMetadata?.CandidatesTokenCount}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }

}
