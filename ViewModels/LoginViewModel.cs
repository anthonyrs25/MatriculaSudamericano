using System.Windows.Input;
using MatriculaSudamericano.Services;

namespace MatriculaSudamericano.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly AutenticacionService _autenticacion;
    private readonly IAlertaService _alertas;
    private readonly INavegacionService _navegacion;

    public LoginViewModel(AutenticacionService autenticacion, IAlertaService alertas, INavegacionService navegacion)
    {
        _autenticacion = autenticacion;
        _alertas = alertas;
        _navegacion = navegacion;
        IngresarCommand = new Command(async () => await IngresarAsync());
    }

    private string _usuario = string.Empty;
    public string Usuario { get => _usuario; set => SetProperty(ref _usuario, value); }

    private string _contrasena = string.Empty;
    public string Contrasena { get => _contrasena; set => SetProperty(ref _contrasena, value); }

    public ICommand IngresarCommand { get; }

    private async Task IngresarAsync()
    {
        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrEmpty(Contrasena))
        {
            await _alertas.MostrarAsync("Datos incompletos", "Ingrese su usuario y contraseña.");
            return;
        }

        if (_autenticacion.EstaBloqueado)
        {
            await MostrarBloqueoAsync();
            return;
        }

        bool accesoCorrecto = _autenticacion.IniciarSesion(Usuario, Contrasena);
        Contrasena = string.Empty;

        if (accesoCorrecto)
        {
            Usuario = string.Empty;
            await _navegacion.IrAMatriculaAsync();
            return;
        }

        if (_autenticacion.EstaBloqueado)
            await MostrarBloqueoAsync();
        else
            await _alertas.MostrarAsync("Acceso denegado", "Usuario o contraseña incorrectos.");
    }

    private Task MostrarBloqueoAsync() =>
        _alertas.MostrarAsync("Acceso bloqueado",
            $"Demasiados intentos fallidos. Espere {_autenticacion.SegundosRestantes} segundos.");
}