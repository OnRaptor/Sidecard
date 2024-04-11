var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

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
    logger.LogInformation("Trying to register the sidecar scheme");
    try
    {
        var client = new HttpClient();
        var address = sp.GetService<IConfiguration>().GetValue<string>("SIDECAR_ADDRESS");
        client.BaseAddress = new Uri(address);

        var result = client.PostAsJsonAsync("/v1/registry/services",
            new
            {
                name = "c#-demo",
                settings = new
                {
                    baseUrl = "http://localhost:5098"
                },
                actions = new
                {
                    hello = "/actions/hello",
                }
            });

        logger.LogInformation("Register response:\n" + await result.Result.Content.ReadAsStringAsync());
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to register the sidecar scheme");
    }
});
app.UseHttpsRedirection();

app.MapControllers();

app.Run();