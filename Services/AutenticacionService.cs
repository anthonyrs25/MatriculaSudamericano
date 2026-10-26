using System.Security.Cryptography;
using System.Text;

namespace MatriculaSudamericano.Services;

public class AutenticacionService
{
    private const string UsuarioValido = "admin";
    private static readonly byte[] Sal = Convert.FromBase64String("lXVY8EmCFWYLSeUgyouY3g==");
    private static readonly byte[] HashGuardado = Convert.FromBase64String("ylmofX+ryue2g3AdHhL0pItwp6tykaq5R0Xz1VSs5g4=");
    private const int Iteraciones = 100_000;
    private const int MaxIntentos = 3;
    private static readonly TimeSpan TiempoBloqueo = TimeSpan.FromSeconds(30);

    private int _intentosFallidos;
    private DateTime? _bloqueadoHasta;

    public string? UsuarioActual { get; private set; }

    public bool EstaBloqueado => _bloqueadoHasta.HasValue && DateTime.Now < _bloqueadoHasta.Value;

    public int SegundosRestantes => EstaBloqueado
        ? (int)Math.Ceiling((_bloqueadoHasta!.Value - DateTime.Now).TotalSeconds)
        : 0;

    public bool IniciarSesion(string usuario, string contrasena)
    {
        if (EstaBloqueado)
            return false;

        byte[] hashIngresado = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(contrasena),
            Sal,
            Iteraciones,
            HashAlgorithmName.SHA256,
            HashGuardado.Length);

        bool usuarioCorrecto = string.Equals(usuario.Trim(), UsuarioValido, StringComparison.OrdinalIgnoreCase);
        bool contrasenaCorrecta = CryptographicOperations.FixedTimeEquals(hashIngresado, HashGuardado);

        if (usuarioCorrecto && contrasenaCorrecta)
        {
            _intentosFallidos = 0;
            _bloqueadoHasta = null;
            UsuarioActual = UsuarioValido;
            return true;
        }

        _intentosFallidos++;
        if (_intentosFallidos >= MaxIntentos)
        {
            _bloqueadoHasta = DateTime.Now.Add(TiempoBloqueo);
            _intentosFallidos = 0;
        }

        return false;
    }

    public void CerrarSesion() => UsuarioActual = null;
}