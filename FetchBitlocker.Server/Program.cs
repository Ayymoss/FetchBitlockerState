var apiKey = Environment.OSVersion.Platform == PlatformID.Unix
    ? Environment.GetEnvironmentVariable("FETCH_BL_API_KEY")
    : Environment.GetEnvironmentVariable("FETCH_BL_API_KEY", EnvironmentVariableTarget.User);

if (apiKey == null)
{
    Console.WriteLine("Please set the environment variable (String) FETCH_BL_API_KEY to your API key.");
    Console.ReadKey();
    Environment.Exit(1);
}

var builder = WebApplication.CreateBuilder(args);

#if !DEBUG
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
    options.ListenAnyIP(5001, configure => configure.UseHttps());
});
#endif

#if DEBUG
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
    options.ListenLocalhost(5001, configure => configure.UseHttps());
});
#endif

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
