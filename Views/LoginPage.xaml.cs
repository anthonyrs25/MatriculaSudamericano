using MatriculaSudamericano.ViewModels;

namespace MatriculaSudamericano.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}