using Life.ViewModels;

namespace Life.Views;

public partial class HowToPlayView : ContentPage
{
    HowToPlayViewModel viewModel;

    public HowToPlayView()
    {
        InitializeComponent();

        this.viewModel = new HowToPlayViewModel();

        viewModel.Navigation = Navigation;
        BindingContext = viewModel;
    }
}
