using BlazorMinesweeper.Clients;
using BlazorMinesweeper.Components;
using BlazorMinesweeper.Configurations;
using MudBlazor;
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

        builder.Services.AddMudServices(config =>
        {
            // Konfigurace ostatních služeb MudBlazor, vèetnì Snackbar
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
        });


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
