using Microsoft.Extensions.Logging;
using MatriculaSudamericano.Services;
using MatriculaSudamericano.ViewModels;
using MatriculaSudamericano.Views;

namespace MatriculaSudamericano;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servicios: una sola instancia para toda la app
        builder.Services.AddSingleton<AutenticacionService>();
        builder.Services.AddSingleton<IAlertaService, AlertaService>();
        builder.Services.AddSingleton<INavegacionService, NavegacionService>();
        builder.Services.AddSingleton<IRegistroService, RegistroTxtService>();

        // ViewModels y vistas
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MatriculaViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MatriculaPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}