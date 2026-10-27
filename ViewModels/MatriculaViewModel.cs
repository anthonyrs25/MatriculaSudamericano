using System.Windows.Input;
using MatriculaSudamericano.Models;
using MatriculaSudamericano.Services;

namespace MatriculaSudamericano.ViewModels;

public class MatriculaViewModel : ViewModelBase
{
    private const string Cedula = "CÉDULA";
    private const string Pasaporte = "PASAPORTE";

    private readonly AutenticacionService _autenticacion;
    private readonly IRegistroService _registro;
    private readonly IAlertaService _alertas;
    private readonly INavegacionService _navegacion;
    private bool _enviando;

    public MatriculaViewModel(AutenticacionService autenticacion, IRegistroService registro,
        IAlertaService alertas, INavegacionService navegacion)
    {
        _autenticacion = autenticacion;
        _registro = registro;
        _alertas = alertas;
        _navegacion = navegacion;
        EnviarCommand = new Command(async () => await EnviarAsync());
        CerrarSesionCommand = new Command(async () => await CerrarSesionAsync());
    }

    // Opciones de los Picker
    public IReadOnlyList<string> TiposIdentificacion { get; } = [Cedula, Pasaporte];
    public IReadOnlyList<string> Carreras { get; } =
        ["Desarrollo de Software", "Diseño Gráfico", "Contabilidad", "Marketing Digital", "Electricidad", "Gastronomía"];
    public IReadOnlyList<string> Modalidades { get; } = ["Presencial", "Semipresencial", "En línea"];
    public IReadOnlyList<string> CampusDisponibles { get; } = ["Campus Matriz", "Campus Norte", "Campus Sur"];

    // Datos del formulario
    private string? _tipoIdentificacion = Cedula;
    public string? TipoIdentificacion { get => _tipoIdentificacion; set => SetProperty(ref _tipoIdentificacion, value); }

    private string _numeroIdentificacion = string.Empty;
    public string NumeroIdentificacion { get => _numeroIdentificacion; set => SetProperty(ref _numeroIdentificacion, value); }

    private string _apellidoPaterno = string.Empty;
    public string ApellidoPaterno { get => _apellidoPaterno; set => SetProperty(ref _apellidoPaterno, value); }

    private string _apellidoMaterno = string.Empty;
    public string ApellidoMaterno { get => _apellidoMaterno; set => SetProperty(ref _apellidoMaterno, value); }

    private string _nombres = string.Empty;
    public string Nombres { get => _nombres; set => SetProperty(ref _nombres, value); }

    private string _telefono = string.Empty;
    public string Telefono { get => _telefono; set => SetProperty(ref _telefono, value); }

    private string _correo = string.Empty;
    public string Correo { get => _correo; set => SetProperty(ref _correo, value); }

    private string _confirmarCorreo = string.Empty;
    public string ConfirmarCorreo { get => _confirmarCorreo; set => SetProperty(ref _confirmarCorreo, value); }

    private string? _carrera;
    public string? Carrera { get => _carrera; set => SetProperty(ref _carrera, value); }

    private string? _modalidad;
    public string? Modalidad { get => _modalidad; set => SetProperty(ref _modalidad, value); }

    private string? _campus;
    public string? Campus { get => _campus; set => SetProperty(ref _campus, value); }

    public ICommand EnviarCommand { get; }
    public ICommand CerrarSesionCommand { get; }

    private async Task EnviarAsync()
    {
        if (_enviando)
            return;

        _enviando = true;
        try
        {
            // 0. Solo un usuario autenticado puede registrar datos
            if (_autenticacion.UsuarioActual is null)
            {
                await _alertas.MostrarAsync("Sesión no válida", "Inicie sesión nuevamente.");
                await _navegacion.IrALoginAsync();
                return;
            }

            string numero = NumeroIdentificacion.Trim();
            string paterno = ApellidoPaterno.Trim();
            string materno = ApellidoMaterno.Trim();
            string nombres = Nombres.Trim();
            string telefono = Telefono.Trim();
            string correo = Correo.Trim();
            string confirmacion = ConfirmarCorreo.Trim();

            var errores = new List<string>();

            // 1. Identificación: cédula (solo números + dígito verificador) o pasaporte
            if (string.IsNullOrEmpty(TipoIdentificacion))
                errores.Add("Tipo de identificación: seleccione una opción.");
            else if (numero.Length == 0)
                errores.Add("Número de identificación: campo obligatorio.");
            else if (TipoIdentificacion == Cedula && !Validador.SoloNumeros(numero))
                errores.Add("Cédula: solo se admiten números.");
            else if (TipoIdentificacion == Cedula && !Validador.CedulaValida(numero))
                errores.Add("Cédula: no es válida (revise los 10 dígitos).");
            else if (TipoIdentificacion == Pasaporte && !Validador.PasaporteValido(numero))
                errores.Add("Pasaporte: solo letras y números, de 5 a 20 caracteres.");

            // 2. Solo letras
            ValidarSoloLetras(paterno, "Apellido paterno", errores);
            ValidarSoloLetras(materno, "Apellido materno", errores);
            ValidarSoloLetras(nombres, "Nombres", errores);

            // 3. Solo números
            if (telefono.Length == 0)
                errores.Add("Teléfono: campo obligatorio.");
            else if (!Validador.SoloNumeros(telefono))
                errores.Add("Teléfono: solo se admiten números.");
            else if (!Validador.TelefonoValido(telefono))
                errores.Add("Teléfono: debe tener 10 dígitos y empezar con 09.");

            // 4. Correos: formato válido y que coincidan
            if (correo.Length == 0)
                errores.Add("Correo: campo obligatorio.");
            else if (!Validador.CorreoValido(correo))
                errores.Add("Correo: el formato no es válido.");

            if (confirmacion.Length == 0)
                errores.Add("Confirmar correo: campo obligatorio.");
            else if (correo.Length > 0 && !string.Equals(correo, confirmacion, StringComparison.OrdinalIgnoreCase))
                errores.Add("Confirmar correo: los correos no coinciden.");

            // 5. Listas obligatorias
            ValidarSeleccion(Carrera, "Carrera", errores);
            ValidarSeleccion(Modalidad, "Modalidad", errores);
            ValidarSeleccion(Campus, "Campus", errores);

            if (errores.Count > 0)
            {
                string detalle = string.Join("\n", errores.Select(e => $"• {e}"));
                await _alertas.MostrarAsync("Revise los datos", detalle);
                return;
            }

            // 6. Datos revisados: se arma la inscripción y se guarda en el TXT
            var inscripcion = new Inscripcion
            {
                TipoIdentificacion = TipoIdentificacion!,
                NumeroIdentificacion = numero.ToUpperInvariant(),
                ApellidoPaterno = paterno,
                ApellidoMaterno = materno,
                Nombres = nombres,
                Telefono = telefono,
                Correo = correo.ToLowerInvariant(),
                Carrera = Carrera!,
                Modalidad = Modalidad!,
                Campus = Campus!,
                FechaRegistro = DateTime.Now,
                RegistradoPor = _autenticacion.UsuarioActual
            };

            string ruta = await _registro.GuardarAsync(inscripcion);
            await _alertas.MostrarAsync("Inscripción enviada",
                $"Los datos de {nombres} {paterno} se guardaron en:\n{ruta}");
            LimpiarFormulario();
        }
        catch (Exception ex)
        {
            await _alertas.MostrarAsync("Error al guardar", $"No se pudo guardar la inscripción.\n{ex.Message}");
        }
        finally
        {
            _enviando = false;
        }
    }

    private async Task CerrarSesionAsync()
    {
        _autenticacion.CerrarSesion();
        LimpiarFormulario();
        await _navegacion.IrALoginAsync();
    }

    private void LimpiarFormulario()
    {
        TipoIdentificacion = Cedula;
        NumeroIdentificacion = string.Empty;
        ApellidoPaterno = string.Empty;
        ApellidoMaterno = string.Empty;
        Nombres = string.Empty;
        Telefono = string.Empty;
        Correo = string.Empty;
        ConfirmarCorreo = string.Empty;
        Carrera = null;
        Modalidad = null;
        Campus = null;
    }

    private static void ValidarSoloLetras(string valor, string campo, List<string> errores)
    {
        if (valor.Length == 0)
            errores.Add($"{campo}: campo obligatorio.");
        else if (!Validador.SoloLetras(valor))
            errores.Add($"{campo}: solo se admiten letras.");
    }

    private static void ValidarSeleccion(string? valor, string campo, List<string> errores)
    {
        if (string.IsNullOrEmpty(valor))
            errores.Add($"{campo}: seleccione una opción.");
    }
}