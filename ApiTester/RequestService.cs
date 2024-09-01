

namespace api_tester_console_app;

public class RequestService(IHttpClientFactory httpClientFactory, MenuActionService menuActionService, FileService fileService)
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly MenuActionService _menuActionService = menuActionService;
    private readonly FileService _fileService = fileService;
    
    public async Task SendRequestAsync(HttpRequestMessage httpRequestMessage)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.SendAsync(httpRequestMessage);
            await PrintResponse(response);
            var content = await response.Content.ReadAsStringAsync();
            SaveResponseContent(content);
            SaveResponseMessage(response);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"An error occurred while sending the request: {e.Message}");
        }
    }

    private void SaveResponseMessage(HttpResponseMessage response)
    {
        Console.WriteLine("Do you want to save this response message?");
        if (!MenuManager.GetConfirmation())
        {
            return;
        }
    }

    private void SaveResponseContent(string content)
    {
        Console.WriteLine("Do you want to save the response content?");
        if (!MenuManager.GetConfirmation())
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("Content is empty. Nothing to save.");
            return;
        }
        if (_fileService.SaveFileContent(content))
        {
            Console.WriteLine("File saved successfully.");
        }
        else
        {
            Console.WriteLine("Could not save a file");
        }
    }

    public HttpRequestMessage QuickRequestView()
    {
        var requestBuilder = new HttpRequestBuilder();
        SetUri(requestBuilder);
        var method = GetMethod(requestBuilder);
        if (method != HttpMethod.Post && method != HttpMethod.Put && method != HttpMethod.Patch)
        {
            return requestBuilder.Build();
        }
        SetContent(requestBuilder);
        return requestBuilder.Build();
    }

    private void SetUri(HttpRequestBuilder requestBuilder)
    {
        Console.WriteLine("Enter uri");
        requestBuilder.SetUri(Console.ReadLine() ?? string.Empty);
    }

    private void SetContent(HttpRequestBuilder requestBuilder) {
        Console.WriteLine("Do you want to add content body?");
        if (!MenuManager.GetConfirmation())
        {
            return;
        }
        var contentType = GetContentType();
        Console.WriteLine("Enter content body");
        var content = GetContent();
        requestBuilder.SetContent(content ?? string.Empty, contentType);
    }
    private string? GetContent()
    {
        Console.WriteLine("Do you want to read content from file?");

        if (MenuManager.GetConfirmation())
        {
            return _fileService.GetFileContentView();
        }
        Console.WriteLine("Enter content body");
        return Console.ReadLine();
    }

    private string GetContentType()
    {
        Console.WriteLine("Select content type");
        var contentTypeMenu = _menuActionService.GetMenuActionsByMenuType(Menu.ContentType);
        var operation = MenuManager.HandleMenu(contentTypeMenu);
        switch (operation.KeyChar)
        {
            case '1':
                return "application/json";
            case '2':
                return "text/html";
            case '3':
                return "application/xml";
            case '4':
                return "text/plain";
            case '5':
                return "application/x-www-form-urlcoded";
            default:
                Console.WriteLine("Enter your content type");
                return Console.ReadLine() ?? string.Empty;
        }
    }
    private HttpMethod GetMethod(HttpRequestBuilder requestBuilder)
    {
        var methodMenu = _menuActionService.GetMenuActionsByMenuType(Menu.MethodType);
        var operation = MenuManager.HandleMenu(methodMenu);
        switch (operation.KeyChar)
        {
            case '1':
                requestBuilder.SetMethod(HttpMethod.Get);
                return HttpMethod.Get;
            case '2':
                requestBuilder.SetMethod(HttpMethod.Post);
                return HttpMethod.Post;
            case '3':
                requestBuilder.SetMethod(HttpMethod.Delete);
                return HttpMethod.Delete;
            case '4':
                requestBuilder.SetMethod(HttpMethod.Put);
                return HttpMethod.Put;
            case '5':
                requestBuilder.SetMethod(HttpMethod.Patch);
                return HttpMethod.Patch;
            default:
                Console.WriteLine("Invalid selection. Default: GET");
                requestBuilder.SetMethod(HttpMethod.Get);
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