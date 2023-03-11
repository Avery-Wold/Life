using Life.ViewModels;

namespace Life;

public partial class MainPage : ContentPage
{
    MainPageViewModel viewModel;

    public MainPage()
    {
        InitializeComponent();

        this.viewModel = new MainPageViewModel();

        viewModel.Navigation = Navigation;
        BindingContext = viewModel;
    }
}