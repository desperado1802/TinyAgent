namespace Agent.Core;

using System;
using Agent.Core.Services;
using Agent.Core.Tools;
using Google.GenAI.Types;

public class Orchestrator
{
    private readonly string apiKey;
    private readonly bool verbose;
    private readonly FileService fileService;
    private readonly ToolDispatcher toolDispatcher;

    private readonly Content contents = new()
    {
        Role = "user",
        Parts = []
    };

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

        var geminiService = new GeminiClient(apiKey);

        contents.Parts!.Add(new Part() { Text = content });
        var chatHistory = new List<Content> { contents };
        try
        {
            for (var i = 0; i < 20; i++)
            {

                var response = await geminiService.GenerateContentResponse(chatHistory);

                if (response.FunctionCalls?.Count > 0)
                {
                    if (response.Candidates?.Count > 0 && response?.Candidates?[0].Content != null)
                    {
                        foreach (var candidate in response.Candidates)
                        {
                            if (candidate.Content != null)
                            {
                                chatHistory.Add(candidate.Content);
                            }
                        }
                    }
                    else
                    {

                        var modelTurn = new Content()
                        {
                            Role = "model",
                            Parts = response?.FunctionCalls.Select(func => new Part { FunctionCall = func }).ToList()
                        };
                        chatHistory.Add(modelTurn);
                    }



                    foreach (var functionCall in response!.FunctionCalls)
                    {
                        var functionCallResponse = toolDispatcher.CallFunction(functionCall);

                        chatHistory.Add(functionCallResponse);

                    }
                }

                if (!string.IsNullOrEmpty(response?.Text))
                {

                    Console.WriteLine($"Gemini says: {response.Text}");
                    break;
                }

                if (verbose)
                {
                    Console.WriteLine($"Input tokens used: {response?.UsageMetadata?.PromptTokenCount}");
                    Console.WriteLine($"Output tokens used: {response?.UsageMetadata?.CandidatesTokenCount}");
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }

}
