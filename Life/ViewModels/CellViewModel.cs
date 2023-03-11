using System;
using System.Windows.Input;

namespace Life.ViewModels
{
	public class CellViewModel : BaseViewModel
    {
        public ICommand ToggleCellStateCommand { get; private set; }

        private bool _isAlive;
        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                base.SetProperty(ref _isAlive, value);
                RaisePropertyChanged(() => BackgroundColor);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                if (IsAlive)
                {
                    return Color.FromArgb("#8155BA");
                }
                return Color.FromArgb("#fff");
            }
        }

        public CellViewModel(bool isAlive)
        {
            IsAlive = isAlive;

            ToggleCellStateCommand = new Command(() =>
            {
                IsAlive = !IsAlive;
            });
        }
    }
}