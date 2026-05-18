namespace Agent.Core;

using Agent.Core.Tools;
using Google.GenAI;
using Google.GenAI.Types;

public class GeminiClient(string apiKey, ToolDispatcher dispatcher)
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


            contents.Parts!.Add(new Part() { Text = content });

            var response = await client.Models.GenerateContentAsync(
                // model: "gemini-2.5-flash",
                // model: "gemini-3.1-flash-lite",
                model: "gemini-3-flash-preview",
                contents: contents,
                config: config
            );

            if (response.FunctionCalls?.Count > 0)
            {
                foreach (var functionCall in response.FunctionCalls)
                {
                    dispatcher.CallFunction(functionCall);
                }
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception($"Gemini generate content fail: {ex.ToString()}");
        }

    }


}
