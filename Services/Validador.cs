using System.Text.RegularExpressions;

namespace MatriculaSudamericano.Services;

public static class Validador
{
    private const string PatronLetras = @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+( [A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$";
    private const string PatronCorreo = @"^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$";
    private const string PatronPasaporte = @"^[A-Za-z0-9]{5,20}$";

    public static bool SoloNumeros(string texto) =>
        !string.IsNullOrEmpty(texto) && texto.All(char.IsAsciiDigit);

    public static bool SoloLetras(string texto) =>
        Regex.IsMatch(texto, PatronLetras);

    public static bool CorreoValido(string correo) =>
        Regex.IsMatch(correo, PatronCorreo);

    public static bool PasaporteValido(string pasaporte) =>
        Regex.IsMatch(pasaporte, PatronPasaporte);

    public static bool TelefonoValido(string telefono) =>
        telefono.Length == 10 && SoloNumeros(telefono) && telefono.StartsWith("09");

    public static bool CedulaValida(string cedula)
    {
        if (cedula.Length != 10 || !SoloNumeros(cedula))
            return false;

        int provincia = int.Parse(cedula[..2]);
        if (provincia < 1 || (provincia > 24 && provincia != 30))
            return false;

        if (cedula[2] - '0' >= 6)
            return false;

        int suma = 0;
        for (int i = 0; i < 9; i++)
        {
            int digito = cedula[i] - '0';
            int producto = (i % 2 == 0) ? digito * 2 : digito;
            if (producto > 9)
                producto -= 9;
            suma += producto;
        }

        int verificador = (10 - suma % 10) % 10;
        return verificador == cedula[9] - '0';
    }
}