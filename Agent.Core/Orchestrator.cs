namespace Agent.Core;

using System;

public class Orchestrator(string apiKey)
{

    // create test call to gemini ai
    public async Task GenerateContent(string content)
    {

        var geminiService = new GeminiClient(apiKey);

        try
        {
            var res = await geminiService.GenerateContentResponse(content);

            Console.WriteLine($"Gemini says: {res.Text}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }

}
