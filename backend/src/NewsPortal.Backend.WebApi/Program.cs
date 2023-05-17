using NewsPortal.Backend.Application;
using NewsPortal.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//  Add services to container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddApplicationServices();
builder.Services.AddHackerNewsClient(conf =>
{
    conf.BaseUrl = builder.Configuration["Apis:HackerNews:BaseUrl"];
    conf.Version = builder.Configuration.GetValue<int>("Apis:HackerNews:Version");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();