using PropertyApi.Config;
using PropertyApi.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddLogging();

builder.Services.AddHttpClient(ConfigValues.PostcodeClientName, client =>
{
    client.BaseAddress = new Uri(ConfigValues.PostcodeLookupUrl);
});

builder.Services.AddHttpClient(ConfigValues.SearchPriceClientName, client =>
{
    client.BaseAddress = new Uri(ConfigValues.SearchPriceLookupUrl);
});


builder.Services.AddScoped<ISoldPriceRepository, SoldPriceRepository>();
builder.Services.AddScoped<IPostcodeService, PostcodeService>();    

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

app.UseHttpsRedirection();
// needed for Blazor to recognise the site
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
