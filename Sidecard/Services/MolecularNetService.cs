using Refit;

namespace Sidecard.Services;

public record BodyContent
{
    public object Params { get; init; } = new { };
    public object Meta { get; init; } = new { };
    public object Options { get; init; } = new { };
    public BodyContent(){}
}
public interface MolecularNetService
{
    [QueryUriFormat(UriFormat.Unescaped)]
    [Post("/v1/call/{action}")]
    Task<ApiResponse<string>> Call(string action, [Body] BodyContent content = default);
    
    [QueryUriFormat(UriFormat.Unescaped)]
    [Post("/v1/emit/{eventName}")]
    Task<ApiResponse<string>> Emit(string eventName, [Body] BodyContent content = default);

    public record RegisterResult(string Status);
    [Post("/v1/registry/services")]
    Task<ApiResponse<RegisterResult>> Register(object schema);
    
    [Get("/v1/registry/services")]
    Task<ApiResponse<string>> GetServices();
}