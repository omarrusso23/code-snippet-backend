using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class SuperbaseServices
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _apiKey;

    public SuperbaseServices()
    {
        // Retrieve base URL and API key from environment variables
        _baseUrl = Environment.GetEnvironmentVariable("SUPABASE_BASE_URL") ?? throw new InvalidOperationException("SUPABASE_BASE_URL environment variable is not set.");
        _apiKey = Environment.GetEnvironmentVariable("SUPABASE_API_KEY") ?? throw new InvalidOperationException("SUPABASE_API_KEY environment variable is not set.");

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<HttpResponseMessage> SaveCodeSnippetAsync(CodeSnippet snippet)
    {
        var url = $"{_baseUrl}/Snippets";

        // Configure JSON serialization to use camelCase for property names
        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var jsonContent = JsonConvert.SerializeObject(snippet, jsonSettings);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        return response;
    }

    public async Task<CodeSnippet> GetCodeSnippetAsync(Guid id)
    {
        var url = $"{_baseUrl}/Snippets?id=eq.{id}&select=code,language";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
            return null;
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var snippets = JsonConvert.DeserializeObject<CodeSnippet[]>(jsonResponse);
        return snippets.Length > 0 ? snippets[0] : null;
    }

    public class CodeSnippet
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
