namespace Agent.Core;

using System;

public class Orchestrator(string apiKey, bool verbose)
{

    // create test call to gemini ai
    public async Task GenerateContent(string content)
    {

        var geminiService = new GeminiClient(apiKey);

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
