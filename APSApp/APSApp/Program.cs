using Azure;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Adding Azure Computer Vision service.
builder.Services.AddSingleton<IComputerVisionClient>(provider =>
{
    var key = builder.Configuration["Azure:ComputerVision:SubscriptionKey"];
    var endpoint = builder.Configuration["Azure:ComputerVision:Endpoint"];

    return new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
    {
        Endpoint = endpoint
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

