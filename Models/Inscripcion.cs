namespace MatriculaSudamericano.Models;

public class Inscripcion
{
    public string TipoIdentificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Carrera { get; set; } = string.Empty;
    public string Modalidad { get; set; } = string.Empty;
    public string Campus { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public string RegistradoPor { get; set; } = string.Empty;
}