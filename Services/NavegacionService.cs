namespace MatriculaSudamericano.Services;

public class NavegacionService : INavegacionService
{
    public Task IrAMatriculaAsync() => Shell.Current.GoToAsync("//MatriculaPage");

    public Task IrALoginAsync() => Shell.Current.GoToAsync("//LoginPage");
}