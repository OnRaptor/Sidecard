using Newtonsoft.Json;

namespace Sidecard.Services;

public class MolecularService
{
    private readonly MolecularNetService _netService;
    private readonly ILogger<MolecularService> _logger;
    public MolecularService(MolecularNetService netService, ILogger<MolecularService> logger)
    {
        _netService = netService;
        _logger = logger;
    }

    public async Task<bool> RegisterService()
    {
        var registerObject = new
        {
            name = "c#-demo",
            settings = new
            {
                baseUrl = "http://aspnetcore-service:8080"
            },
            actions = new
            {
                hello = "/actions/hello",
            },
            events =
                new Dictionary<string, string>()
                {
                    { "sample.event", "/events/sample.event" }
                }
        };
        _logger.LogInformation($"Registering service: {JsonConvert.SerializeObject(registerObject)}");
        var result = await _netService.Register(registerObject);
        
        return result.IsSuccessStatusCode && result.Content.Status == "OK";
    } 
    
    //public record MolecularRegisteredService(object Name, object Settings, object FullName, object Version, object Available, object Nodes);

    public async Task<string?> GetRegisteredServices()
    {
        //var result = await _netService.Call("$node.services");
        var result = await _netService.GetServices();
        return result?.Content;
    }
}