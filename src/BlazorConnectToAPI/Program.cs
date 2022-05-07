using System.Text;
using BlazorConnectToAPI;
using BlazorConnectToAPI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;
var currentConfig = config["currentConfig"];
var apiBase = config[$"{currentConfig}:PropertyApiUrl"];
builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));
builder.Services.AddLogging();

builder.Services.AddHttpClient("PropertyApi", client =>
{
    client.BaseAddress = new Uri(apiBase);
});

builder.Services.AddScoped<IPropertyService, PropertyService>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();


