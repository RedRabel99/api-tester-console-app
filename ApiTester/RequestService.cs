using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace api_tester_console_app;

public class RequestService(IHttpClientFactory httpClientFactory, MenuActionService menuActionService)
{
    private IHttpClientFactory _httpClientFactory = httpClientFactory;
    private MenuActionService _menuActionService = menuActionService;

    public async Task QuickRequestView()
    {
        var request = BuildQuickRequest();
        var response = await SendRequestAsync(request);

        if (response is null)
        {
            return;
        }

        await PrintResponse(response);
    }

    public async Task AdvancedRequestView()
    {
        var request = BuildAdvancedRequest();
        var response = await SendRequestAsync(request);

        if (response is null)
        {
            return;
        }

        await PrintResponse(response);
    }

    private HttpRequestMessage BuildAdvancedRequest()
    {
        var requestBuilder = new HttpRequestBuilder();

        Console.WriteLine("Enter uri");
        requestBuilder.SetUri(Console.ReadLine() ?? string.Empty);

        var method = GetMethod();
        requestBuilder.SetMethod(method);

        if (method == HttpMethod.Post && method == HttpMethod.Put && method == HttpMethod.Patch)
        {
            if (MenuManager.GetConfirmation("Do you want to add body?"))
            {
                return requestBuilder.Build();
            }
        }

        var headers = GetHeaders();
        if (headers != null)
        {
            requestBuilder.SetHeaders(headers ?? new Dictionary<string, string>());
        }

        if(MenuManager.GetConfirmation("Do you want to set custom user agent?"))
        {
            Console.WriteLine("Enter your custom user agent:");
            requestBuilder.SetUserAgent(Console.ReadLine() ?? string.Empty);
        }

        if (MenuManager.GetConfirmation("Do you want to set authentication?"))
        {
            Console.WriteLine("Enter scheme:");
            var scheme = Console.ReadLine();
            Console.WriteLine("Enter parameter:");
            var parameter = Console.ReadLine();
            requestBuilder.SetAuth(scheme ?? string.Empty, parameter ?? string.Empty);
        }

        Console.WriteLine("Enter content body");
        var content = Console.ReadLine();
        requestBuilder.SetContent(content ?? string.Empty);

        return requestBuilder.Build();
    }

    private async Task<HttpResponseMessage?> SendRequestAsync(HttpRequestMessage httpRequestMessage)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
           return await client.SendAsync(httpRequestMessage);
            
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"An error occurred while sending the request: {e.Message}");
            return null;
        }
    }

    private HttpRequestMessage BuildQuickRequest()
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

        if (!MenuManager.GetConfirmation("Do you want to add body?"))
        {
            return requestBuilder.Build();
        }

        Console.WriteLine("Enter content body");
        var content = Console.ReadLine();
        requestBuilder.SetContent(content ?? string.Empty);

        return requestBuilder.Build();
    }

    private Dictionary<string, string>? GetHeaders()
    {
        var headers = new Dictionary<string, string>();
        if (!MenuManager.GetConfirmation("Do you want to add headers?"))
        {
            return null;
        }

        AddHeaderView(headers);

        while(MenuManager.GetConfirmation("Do you want to add another header?"))
        {
            AddHeaderView(headers);
        }

        return headers;
    }

    private void AddHeaderView(Dictionary<string, string> headers)
    {
        Console.WriteLine("Enter header key");
        var key = Console.ReadLine();
        Console.WriteLine("Enter header value");
        var value = Console.ReadLine();
        if (key is null || value is null)
        {
            Console.WriteLine("Could not add a header. Make sure your input is correct");
            return;
        }
        headers.Add(key, value);
    }

    private HttpMethod GetMethod()
    {
        Console.WriteLine("Select method:");
        Console.WriteLine("1. GET");
        Console.WriteLine("2. POST");
        Console.WriteLine("3. DELETE");
        Console.WriteLine("4. PUT");
        Console.WriteLine("5. PATCH");
        var operation = Console.ReadKey();
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
}