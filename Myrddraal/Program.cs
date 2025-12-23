using Myrddraal.Components;
using MudBlazor.Services;
using Blazor.Diagrams;
using Myrddraal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- MYRDDRAAL SERVICES ---
builder.Services.AddMudServices();
builder.Services.AddScoped<BlazorDiagram>();
builder.Services.AddScoped<BladeRunner>();
// --------------------------

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();