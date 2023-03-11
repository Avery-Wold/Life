namespace Life;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        var mainPage = new NavigationPage(new MainPage())
        {
            BarBackgroundColor = Color.FromArgb("#8155BA")
        };

        MainPage = mainPage;

        if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromArgb("#fff");

        NavigationPage.SetBackButtonTitle(this, "");
    }
}

