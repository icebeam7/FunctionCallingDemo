using FunctionCallingDemo.Models;
using NewsAPI.Models;

namespace FunctionCallingDemo.Services
{
    public interface INewsService
    {
        Task<IEnumerable<Article>?> GetTopHeadlines(string query, string country, string category = "General", int pageSize = 5);
        NewsArguments ExtractNewsArguments(string toolCallArguments);
    }
}
