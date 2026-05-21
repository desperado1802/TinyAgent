namespace Agent.Core;

using Agent.Core.Tools;
using Google.GenAI;
using Google.GenAI.Types;

public class GeminiClient(string apiKey)
{
    private readonly Client client = new(false, apiKey);


    public async Task<GenerateContentResponse> GenerateContentResponse(List<Content> chatHistory)
    {
        try
        {
            string systemPrompt = """
                You are a helpful AI coding agent.
                When a user asks a question or makes a request, make function call plan. You can perform the following operations:
                - List files and directories
                All paths you provide should be relative to the working directory. You do not need to specify the working directory in your function calls as it is automatically injected for security reasons.
            """;

            var config = new GenerateContentConfig
            {
                SystemInstruction = new Content
                {
                    Parts = [new Part() { Text = systemPrompt }]
                },
                Tools = ToolRegistry.GetAvailableTools()
            };

            var response = await client.Models.GenerateContentAsync(
                // model: "gemini-2.5-flash",
                // model: "gemini-3.1-flash-lite",
                model: "gemini-3-flash-preview",
                contents: chatHistory,
                config: config
            );

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception($"Gemini generate content fail: {ex.ToString()}");
        }

    }


}
