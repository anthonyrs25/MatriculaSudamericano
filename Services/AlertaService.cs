namespace MatriculaSudamericano.Services;

public class AlertaService : IAlertaService
{
    public async Task MostrarAsync(string titulo, string mensaje)
    {
        await Shell.Current.DisplayAlertAsync(titulo, mensaje, "Aceptar");
    }
}