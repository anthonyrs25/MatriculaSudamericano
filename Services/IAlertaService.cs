namespace MatriculaSudamericano.Services;

public interface IAlertaService
{
    Task MostrarAsync(string titulo, string mensaje);
}