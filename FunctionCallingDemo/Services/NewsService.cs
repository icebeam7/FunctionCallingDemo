using FunctionCallingDemo.Models;
using FunctionCallingDemo.Helpers;

using System.Text.Json.Nodes;

using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;

namespace FunctionCallingDemo.Services
{
    public class NewsService : INewsService
    {
        NewsApiClient newsClient;

        public NewsService()
        {
            newsClient = new (Constants.NewsApiKey);
        }

        public async Task<IEnumerable<Article>?> GetTopHeadlines(string query, string country, string category = "General", int pageSize = 5)
        {
            var topHeadlinesRequest = new TopHeadlinesRequest();

            if (!string.IsNullOrWhiteSpace(query))
                topHeadlinesRequest.Q = query;

            if (!string.IsNullOrWhiteSpace(country))
            {
                Enum.TryParse(country.ToUpper(), out Countries countryEnum);
                topHeadlinesRequest.Country = countryEnum;
            }

            if (!string.IsNullOrWhiteSpace(category))
                if (category != "General")
                {
                    Enum.TryParse(category, out Categories categoryEnum);
                    topHeadlinesRequest.Category = categoryEnum;
                }
                else
                    topHeadlinesRequest.Q += "&category=general";

            if (pageSize > 0)
                topHeadlinesRequest.PageSize = pageSize;

            var news = await newsClient.GetTopHeadlinesAsync(topHeadlinesRequest);
            return news.Articles;
        }

        public NewsArguments ExtractNewsArguments(string toolCallArguments)
        {
            var jsonNode = JsonNode.Parse(toolCallArguments);

            string query = (string)jsonNode["query"]!;
            string country = (string)jsonNode["country"]!;
            string category = (string)jsonNode["category"]!;

            return new()
            {
                Query = query,
                Country = country,
                Category = category
            };
        }
    }
}
