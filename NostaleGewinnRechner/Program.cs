using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NostaleGewinnRechner.Components;
using NostaleGewinnRechner.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<RechnerState>();
builder.Services.AddScoped<SpeicherService>();

await builder.Build().RunAsync();
