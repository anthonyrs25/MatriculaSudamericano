using MatriculaSudamericano.Models;

namespace MatriculaSudamericano.Services;

public interface IRegistroService
{
    Task<string> GuardarAsync(Inscripcion inscripcion);
}