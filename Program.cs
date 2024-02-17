using BlazorMinesweeper.Clients;
using BlazorMinesweeper.Components;
using BlazorMinesweeper.Configurations;
using MudBlazor.Services;

namespace BlazorMinesweeper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddMudServices();

        builder.Services.Configure<ServiceUrlConfig>(
            builder.Configuration.GetSection(nameof(ServiceUrlConfig).Replace("Config", string.Empty)));
        builder.Services.AddHttpClient<GamesClient>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
