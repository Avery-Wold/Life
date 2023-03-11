using System;
using Life.Views;

namespace Life.ViewModels
{
	public class MainPageViewModel : BaseViewModel
    {
        public Command HowToPlayCommand { get; private set; }
        public Command PlayCommand { get; private set; }

        public MainPageViewModel()
        {
            HowToPlayCommand = new Command(async () => await HowToPlay());
            PlayCommand = new Command(async () => await Play());

            Description = "This is an implementation of the Game of Life for Xamarin.Forms. Life is a cellular automaton invented by mathematician John Conway in 1970.";
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        private async Task HowToPlay()
        {
            await Navigation.PushAsync(new HowToPlayView());
        }

        private async Task Play()
        {
            await Navigation.PushAsync(new GameView());
        }
    }
}

