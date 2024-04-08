namespace FunctionCallingDemo.Helpers
{
    public class Constants
    {
        public static readonly string AzureOpenAIKey = "";
        public static readonly string AzureOpenAIEndpoint = "";
        public static readonly string AzureOpenAIRegion = "eastus";
        public static readonly string AzureOpenAIDeploymentName = "";

        public static readonly int MaxTokens = 15500;
        public static readonly string SystemPrompt = 
            "You are an assistant that provides news and headlines " +
            "to user requests. Always try to get the latest " +
            "breaking stories using the available function calls.";

        public static readonly string NewsApiKey = "";
    }
}
