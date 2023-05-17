using NewsPortal.Backend.Application;
using NewsPortal.Backend.Application.Services;
using NewsPortal.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddApplicationServices();
builder.Services.AddHackerNewsClient(conf =>
{
    conf.BaseUrl = builder.Configuration["Apis:HackerNews:BaseUrl"];
    conf.Version = builder.Configuration.GetValue<int>("Apis:HackerNews:Version");
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/stories", async (IItemService itemService) =>
{
    try
    {
        return Results.Ok(await itemService.GetNewestStories());
    }
    catch (Exception)
    {
        return Results.StatusCode(500);
    }
});

app.Run();