using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApiApp;
using Blazored.LocalStorage; // pour faire appel aux LocalStorage
using BlazorApiApp.Services; // Pour faire appel aux Services/FavoritesService.cs

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient par défaut
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Ajout LocalStorage et FavoritesService
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<FavoritesService>();

await builder.Build().RunAsync();
