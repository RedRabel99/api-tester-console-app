using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace api_tester_console_app;

public class RequestService(IHttpClientFactory httpClientFactory, MenuActionService menuActionService)
{
    private IHttpClientFactory _httpClientFactory = httpClientFactory;
    private MenuActionService _menuActionService = menuActionService;
    
    private async Task PrintResponse(HttpResponseMessage response)
    {
        Console.WriteLine($"StatusCode: {response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        
        if (content.Length > 300)
        {
            Console.WriteLine($"Content: {content[..300]}...");
        }
        else
        {
            Console.WriteLine($"Content: {content}");
        }
        var headers = response.Headers;
        Console.Write("Headers: ");
        foreach (var header in headers)
        {
            Console.Write($"[{header.Key}, {header.Value.FirstOrDefault()}]");
        }
        Console.WriteLine();
    }

    public async Task SendRequestAsync(HttpRequestMessage httpRequestMessage)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.SendAsync(httpRequestMessage);
            await PrintResponse(response);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"An error occurred while sending the request: {e.Message}");
        }
    }
    
    public HttpRequestMessage QuickRequestView()
    {
        var requestBuilder = new HttpRequestBuilder();
        Console.WriteLine("Enter uri");
        requestBuilder.SetUri(Console.ReadLine() ?? string.Empty);
        var method = GetMethod();
        requestBuilder.SetMethod(method);
        if (method != HttpMethod.Post && method != HttpMethod.Put && method != HttpMethod.Patch)
        {
            return requestBuilder.Build();
        }
        Console.WriteLine("Enter content body");
        var content = Console.ReadLine();
        requestBuilder.SetContent(content ?? string.Empty);

        return requestBuilder.Build();
    }

    private HttpMethod GetMethod()
    {
        var methodMenu = _menuActionService.GetMenuActionsByMenuType(Menu.MethodType);
        var operation = MenuManager.HandleMenu(methodMenu);
        switch (operation.KeyChar)
        {
            case '1':
                return HttpMethod.Get;
            case '2':
                return HttpMethod.Post;
            case '3':
                return HttpMethod.Delete;
            case '4':
                return HttpMethod.Put;
            case '5':
                return HttpMethod.Patch;
            default:
                Console.WriteLine("Invalid selection. Default: GET");
                return HttpMethod.Get;
        }
    }
}