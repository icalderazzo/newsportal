using NewsPortal.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHackerNewsClient(conf =>
{
    conf.BaseUrl = builder.Configuration["Apis:HackerNews:BaseUrl"];
    conf.Version = builder.Configuration.GetValue<int>("Apis:HackerNews:Version");
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();