using Sidecard.Services;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var sp = builder.Services.BuildServiceProvider();
var address = sp.GetService<IConfiguration>().GetValue<string>("SIDECAR_ADDRESS");
builder.Services
    .AddRefitClient<MolecularNetService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(address));

builder.Services.AddScoped<MolecularService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Lifetime.ApplicationStarted.Register(async () =>
{
    var sp = builder.Services.BuildServiceProvider();
    var logger = sp.GetService<ILogger<Program>>();    
    try
    {
        var molecularService = sp.GetService<MolecularService>();
        for(var i = 0; i < 5; i++)
        {
            logger.LogInformation($"Trying to register the sidecar scheme, {i+1} try");
            if (await molecularService.RegisterService())
            {
                logger.LogInformation("Successfully registered the sidecar scheme");
                break;
            }

            await Task.Delay(2000);
        };
        
        logger.LogInformation("Retrieving available services");
        var r = await molecularService.GetRegisteredServices();
        if (r != null)
            logger.LogInformation("Result:" + r);
        else
            logger.LogInformation("Unable to retrieve available services");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to register the sidecar scheme, leaving...");
        Environment.Exit(1);
    }
});
app.UseHttpsRedirection();

app.MapControllers();

app.Run();