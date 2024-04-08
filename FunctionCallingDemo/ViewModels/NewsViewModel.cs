using Azure.AI.OpenAI;
using System.Text.Json;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using FunctionCallingDemo.Models;
using FunctionCallingDemo.Services;

namespace FunctionCallingDemo.ViewModels
{
    public partial class NewsViewModel : BaseViewModel
    {
        [ObservableProperty]
        string prompt;

        [ObservableProperty]
        string answer;

        IAzureOpenAIService aoaiService;
        INewsService newsService;

        public NewsViewModel(IAzureOpenAIService aoaiService, INewsService newsService)
        {
            this.aoaiService = aoaiService;
            this.newsService = newsService;
        }

        [RelayCommand]
        async Task AskQuestionAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                Answer = string.Empty;

                var firstChatResponse = await aoaiService.SendChatCompletion(Prompt, true);

                if (firstChatResponse?.FinishReason == "tool_calls")
                {
                    var toolCalls = firstChatResponse.Message.ToolCalls.FirstOrDefault() as ChatCompletionsFunctionToolCall;

                    if (toolCalls.Name == "get_top_headlines")
                    {
                        var newsArgs = newsService.ExtractNewsArguments(toolCalls.Arguments);

                        var articles = await newsService.GetTopHeadlines(newsArgs.Query, newsArgs.Country, newsArgs.Category);
                        var articlesInfo = articles.Select(a => a.Title);
                        var articlesText = JsonSerializer.Serialize(articlesInfo);

                        var toolMessage = new ToolMessageModel()
                        {
                            FunctionName = toolCalls.Name,
                            ToolCallId = toolCalls.Id,
                            Content = articlesText
                        };

                        var assistantMessage = new ChatRequestAssistantMessage(firstChatResponse.Message);

                        var secondChatResponse = await aoaiService.SendChatCompletion(
                            Prompt, true, assistantMessage, toolMessage);

                        Answer = secondChatResponse.Message.Content; 
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
