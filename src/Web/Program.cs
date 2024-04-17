using BevMan.Web;
using BevMan.Web.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.SetupConfiguration();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(settings =>
    {
        settings.DocumentTitle = "BevMan API";
        settings.SwaggerEndpoint("/swagger/v1/swagger.json", "BevMan API V1");
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();

app.UseExceptionHandler(options => { });

app.MapEndpoints();

app.Run();
