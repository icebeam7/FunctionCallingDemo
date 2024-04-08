using FunctionCallingDemo.Views;
using FunctionCallingDemo.Services;
using FunctionCallingDemo.ViewModels;

using Microsoft.Extensions.Logging;

namespace FunctionCallingDemo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<INewsService, NewsService>();
            builder.Services.AddSingleton<IAzureOpenAIService, AzureOpenAIService>();
            builder.Services.AddSingleton<NewsView>();
            builder.Services.AddSingleton<NewsViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
