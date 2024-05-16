using DataAccessLibrary;
using GraduationSlideshows.Components;
using GraduationSlideshows.Hubs;
using MudBlazor.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddSignalR();

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IPeopleData, PeopleData>();
builder.Services.AddTransient<IImageData, ImageData>();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
//app.MapBlazorHub();
app.MapHub<SlideshowHub>("/slideshowhub");
// app.MapFallbackToPage("/");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();