using MatriculaSudamericano.ViewModels;

namespace MatriculaSudamericano.Views;

public partial class MatriculaPage : ContentPage
{
    private const double AnchoDosColumnas = 850;
    private const double AnchoMaximo = 1100;

    public MatriculaPage(MatriculaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        SizeChanged += (_, _) => AjustarDiseno(Width);
    }

    private void AjustarDiseno(double ancho)
    {
        if (ancho <= 0)
            return;

        contenido.WidthRequest = Math.Min(ancho, AnchoMaximo);

        bool pantallaAncha = ancho >= AnchoDosColumnas;

        Grid.SetColumnSpan(formulario, pantallaAncha ? 1 : 2);

        Grid.SetRow(panelInfo, pantallaAncha ? 0 : 1);
        Grid.SetColumn(panelInfo, pantallaAncha ? 1 : 0);
        Grid.SetColumnSpan(panelInfo, pantallaAncha ? 1 : 2);
        panelInfo.Margin = pantallaAncha ? new Thickness(0, 70, 0, 0) : new Thickness(0);
    }
}