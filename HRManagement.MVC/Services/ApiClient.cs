using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HRManagement.MVC.Models.Common;

namespace HRManagement.MVC.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<ApiResponseVM<T>> GetAsync<T>(string endpoint) => SendAsync<T>(HttpMethod.Get, endpoint);
    public Task<ApiResponseVM<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest body) => SendAsync<TResponse>(HttpMethod.Post, endpoint, body);
    public Task<ApiResponseVM<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest body) => SendAsync<TResponse>(HttpMethod.Put, endpoint, body);
    public Task<ApiResponseVM<TResponse>> PatchAsync<TRequest, TResponse>(string endpoint, TRequest body) => SendAsync<TResponse>(HttpMethod.Patch, endpoint, body);

    private async Task<ApiResponseVM<T>> SendAsync<T>(HttpMethod method, string endpoint, object? body = null)
    {
        using var request = new HttpRequestMessage(method, endpoint);
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: JsonOptions);
        }

        try
        {
            using var response = await _httpClient.SendAsync(request);
            var apiResponse = await ReadApiResponseAsync<T>(response);
            apiResponse.Success = apiResponse.Success && response.IsSuccessStatusCode;

            if (string.IsNullOrWhiteSpace(apiResponse.Message) && !response.IsSuccessStatusCode)
            {
                apiResponse.Message = $"API returned {(int)response.StatusCode}.";
            }

            return apiResponse;
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponseVM<T> { Success = false, Message = $"API is unavailable: {ex.Message}" };
        }
        catch (JsonException ex)
        {
            return new ApiResponseVM<T> { Success = false, Message = $"API response could not be read: {ex.Message}" };
        }
    }

    private static async Task<ApiResponseVM<T>> ReadApiResponseAsync<T>(HttpResponseMessage response)
    {
        await using var stream = await response.Content.ReadAsStreamAsync();
        if (stream.Length == 0)
        {
            return new ApiResponseVM<T>
            {
                Success = response.IsSuccessStatusCode,
                Message = response.IsSuccessStatusCode ? "Request completed." : $"API returned {(int)response.StatusCode}."
            };
        }

        using var document = await JsonDocument.ParseAsync(stream);
        var root = document.RootElement;

        var result = new ApiResponseVM<T>
        {
            Success = response.IsSuccessStatusCode,
            Message = response.IsSuccessStatusCode ? "Request completed." : $"API returned {(int)response.StatusCode}."
        };

        if (TryGetProperty(root, "success", out var successElement) && successElement.ValueKind is JsonValueKind.True or JsonValueKind.False)
        {
            result.Success = successElement.GetBoolean();
        }

        if (TryGetProperty(root, "message", out var messageElement) && messageElement.ValueKind == JsonValueKind.String)
        {
            result.Message = messageElement.GetString() ?? result.Message;
        }

        if (TryGetProperty(root, "data", out var dataElement) && dataElement.ValueKind is not JsonValueKind.Null and not JsonValueKind.Undefined)
        {
            result.Data = dataElement.Deserialize<T>(JsonOptions);
        }

        return result;
    }

    private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }
}
