using System.Text;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using MatriculaSudamericano.Models;

namespace MatriculaSudamericano.Services;

public class RegistroTxtService : IRegistroService
{
    private const string NombreArchivo = "inscripciones.txt";

    public async Task<string> GuardarAsync(Inscripcion inscripcion)
    {
        string carpeta = ObtenerCarpeta();
        Directory.CreateDirectory(carpeta);
        string ruta = Path.Combine(carpeta, NombreArchivo);

        var texto = new StringBuilder();
        texto.AppendLine("==================================================");
        texto.AppendLine("INSCRIPCIÓN - FASE A - GRADO");
        texto.AppendLine($"Fecha de registro : {inscripcion.FechaRegistro:dd/MM/yyyy HH:mm:ss}");
        texto.AppendLine($"Registrado por    : {inscripcion.RegistradoPor}");
        texto.AppendLine("--------------------------------------------------");
        texto.AppendLine($"Identificación    : {inscripcion.TipoIdentificacion} {inscripcion.NumeroIdentificacion}");
        texto.AppendLine($"Apellidos         : {inscripcion.ApellidoPaterno} {inscripcion.ApellidoMaterno}");
        texto.AppendLine($"Nombres           : {inscripcion.Nombres}");
        texto.AppendLine($"Teléfono          : {inscripcion.Telefono}");
        texto.AppendLine($"Correo            : {inscripcion.Correo}");
        texto.AppendLine($"Carrera           : {inscripcion.Carrera}");
        texto.AppendLine($"Modalidad         : {inscripcion.Modalidad}");
        texto.AppendLine($"Campus            : {inscripcion.Campus}");
        texto.AppendLine();

        await File.AppendAllTextAsync(ruta, texto.ToString(), Encoding.UTF8);
        return ruta;
    }

    private static string ObtenerCarpeta()
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            string documentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documentos, "MatriculaSudamericano");
        }

        return FileSystem.Current.AppDataDirectory;
    }
}