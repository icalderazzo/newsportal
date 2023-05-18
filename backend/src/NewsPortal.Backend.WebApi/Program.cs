using Microsoft.OpenApi.Models;
using NewsPortal.Backend.Application;
using NewsPortal.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//  Add services to container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "News Portal API",
        Version = "v1"
    });
});
builder.Services.AddMemoryCache();
builder.Services.AddApplicationServices();
builder.Services.AddHackerNewsClient(conf =>
{
    conf.BaseUrl = builder.Configuration["Apis:HackerNews:BaseUrl"];
    conf.Version = builder.Configuration.GetValue<int>("Apis:HackerNews:Version");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(opt => opt.DefaultModelsExpandDepth(-1));

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(o => true)
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();