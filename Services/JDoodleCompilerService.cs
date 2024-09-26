using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class JDoodleCompilerService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId = "7f69f6ed74c9629e5e007f926bbb0069";
    private readonly string _clientSecret = "1cb76272f9d20d9142c647b80266711f4bd0629c11213d5404b8bebfcc87e1bb";

    public JDoodleCompilerService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> CompileCodeAsync(string code, string language)
    {
        var url = "https://api.jdoodle.com/v1/execute";

        var requestBody = new
        {
            clientId = _clientId,
            clientSecret = _clientSecret,
            script = code,
            language = language,
            versionIndex = "0"
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error compiling code: {responseBody}");
        }

        return responseBody;

    }
}