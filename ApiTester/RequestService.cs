using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace api_tester_console_app;

public class RequestService
{
    private IHttpClientFactory _httpClientFactory;

    public RequestService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task SendRequestView(HttpMethod method)
    {
        var client = _httpClientFactory.CreateClient();
        Console.WriteLine("Enter Uri");
        var uri = Console.ReadLine();
        switch (method)
        {
            case HttpMethod.Get:
                var response = await client.GetAsync(uri);
                await PrintResponse(response);
                break;
            case HttpMethod.Post:
                break;
            case HttpMethod.Delete:
                break;
            case HttpMethod.Patch:
                break;
            case HttpMethod.Put:
                break;
            default:
                Console.WriteLine("Invalid method");
                break;
        }
    }

    private async Task PrintResponse(HttpResponseMessage response)
    {
      
        Console.WriteLine($"StatusCode: {response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        var endOfContent = content.Length > 300 ? "..." : "";
        Console.WriteLine($"Content: {content.Substring(0, 300)}{endOfContent}");
        var headers = response.Headers;
        Console.Write("Headers: ");
        foreach (var header in headers)
        {
            Console.Write($"[{header.Key}, {header.Value}]");
        }
        Console.WriteLine();
    } 
}