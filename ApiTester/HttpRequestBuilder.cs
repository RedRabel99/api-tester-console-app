using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace api_tester_console_app;

public class HttpRequestBuilder
{
    private string? _uri;
    private System.Net.Http.HttpMethod? _method;
    private Dictionary<string, string>? _headers;
    private AuthenticationHeaderValue? _authHeader;
    private string? _userAgent;
    private StringContent? _content;
    private readonly string[] contentSpecificHeaders = [
        "content-type",
        "content-length",
        "content-disposition",
        "content-encoding",
        "content-language",
        "content-location",
        "content-range",
        "content-md5"
    ];

    public void SetUri(string uri)
    {
        _uri = uri;
    }

    public void SetMethod(HttpMethod method)
    {
        _method = method;
    }

    public void SetAuth(string scheme, string parameter)
    {
        _authHeader =  new AuthenticationHeaderValue(scheme, parameter);
    }

    public void SetHeaders(Dictionary<string, string> headers)
    {
        _headers = headers;
    }

    public void SetUserAgent(string userAgent)
    {
        _userAgent = userAgent;
    }

    public void SetContent(string content, string mediaType = "text/plain")
    {
        _content = new StringContent(content, Encoding.UTF8, mediaType);
    }

    public HttpRequestMessage Build()
    {
        if (string.IsNullOrEmpty(_uri))
        {
            throw new InvalidOperationException("Uri is not set properly");
        }
        
        if (_method == null)
        {
            throw new InvalidOperationException("Http method must be set");
        }
        
        var httpRequestMessage = new HttpRequestMessage(_method, _uri);

        if (_userAgent != null)
        {
            httpRequestMessage.Headers.UserAgent.ParseAdd(_userAgent);
        }
        
        if (_authHeader != null)
        {
            httpRequestMessage.Headers.Authorization = _authHeader;
        }
        
        if (_content != null)
        {
            httpRequestMessage.Content = _content;
        }

        if (_headers == null) return httpRequestMessage;
        foreach (var header in _headers)
        {
            if(contentSpecificHeaders.Contains(header.Key.ToLower()))
            {
                Console.WriteLine($"Could not add {header.Key} header");
                //TODO: Handle contenty headers accordingly
                continue;
            }
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }

        return httpRequestMessage;
    }
}