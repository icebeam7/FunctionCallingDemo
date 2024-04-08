using Azure.AI.OpenAI;
using FunctionCallingDemo.Models;

namespace FunctionCallingDemo.Services
{
    public interface IAzureOpenAIService
    {
        Task<ChatChoice> SendChatCompletion(
            string prompt, 
            bool requireFunction = false, 
            ChatRequestAssistantMessage assistantMessage = null, 
            ToolMessageModel toolMessage = null);
    }
}
