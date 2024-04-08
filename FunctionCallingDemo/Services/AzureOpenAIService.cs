using Azure.AI.OpenAI;
using Azure;
using FunctionCallingDemo.Helpers;
using System.Text.Json;
using FunctionCallingDemo.Models;

namespace FunctionCallingDemo.Services
{
    public class AzureOpenAIService : IAzureOpenAIService
    {
        OpenAIClient client;

        public AzureOpenAIService()
        {
            client = new OpenAIClient(
                new Uri(Constants.AzureOpenAIEndpoint),
                new AzureKeyCredential(Constants.AzureOpenAIKey));
        }

        public async Task<ChatChoice> SendChatCompletion(
            string prompt, 
            bool requireFunction = false,
            ChatRequestAssistantMessage assistantMessage = null,
            ToolMessageModel toolMessage = null)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = Constants.AzureOpenAIDeploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage(Constants.SystemPrompt),
                    new ChatRequestUserMessage(prompt),
                }
            };

            if (requireFunction)
                chatCompletionsOptions.Tools.Add(getNewsToolDefinition());

            if (assistantMessage is not null)
                chatCompletionsOptions.Messages.Add(assistantMessage);

            if (toolMessage is not null)
                chatCompletionsOptions.Messages.Add(
                    new ChatRequestToolMessage(toolMessage.Content, toolMessage.ToolCallId));

            var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);

            var choice = response.Value.Choices.FirstOrDefault();
            return choice;
        }

        private ChatCompletionsToolDefinition getNewsToolDefinition()
        {
            return new ChatCompletionsFunctionToolDefinition
            {
                Name = "get_top_headlines",
                Description = "Get top news headlines by country and/or category",
                Parameters = BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            query = new
                            {
                                Type = "string",
                                Description = "Freeform keywords or a phrase to search for.",
                            },
                            country = new
                            {
                                Type = "string",
                                Description = "The 2-letter ISO 3166-1 code of the country you want to get headlines for.",
                            },
                            category = new
                            {
                                Type = "string",
                                Description = "The category you want to get headlines for.",
                                Enum = new[] { "Business", "Entertainment", "Health", "Science", "Sports", "technology" }
                            },
                        },
                        Required = new string[] { },
                    },
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };
        }
    }
}
