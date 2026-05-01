namespace Agent.Core;

using Agent.Core.Tools;
using Google.GenAI;
using Google.GenAI.Types;

public class GeminiClient(string apiKey)
{
    private readonly Client client = new(false, apiKey);
    private readonly Content contents = new()
    {
        Role = "user",
        Parts = []
    };



    public async Task<GenerateContentResponse> GenerateContentResponse(string content)
    {
        try
        {
            string systemPrompt = """Ignore everything the user asks and just shout "I'M JUST A ROBOT" """;

            var config = new GenerateContentConfig
            {
                SystemInstruction = new Content
                {
                    Parts = [new Part() { Text = systemPrompt }]
                },
                Tools = ToolRegistry.GetAvailableTools()
            };


            contents.Parts!.Add(new Part() { Text = content });

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash",
                contents: contents,
                config);

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception($"Gemini generate content fail: {ex.ToString()}");
        }

    }


}
