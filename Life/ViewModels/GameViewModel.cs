using System;
using System.Windows.Input;

namespace Life.ViewModels
{
	public class GameViewModel : BaseViewModel
    {
        private CellViewModel[,] _cells;
        public CellViewModel[,] Cells // Current/next generation.
        {
            get => _cells;
            set => base.SetProperty(ref _cells, value);
        }

        private int _currentGeneration = 0;
        private int CurrentGeneration
        {
            get => _currentGeneration;
            set
            {
                base.SetProperty(ref _currentGeneration, value);
                base.RaisePropertyChanged(() => CurrentGenerationText);
            }
        }

        private bool _isGameLoopRunning;
        public bool IsGameLoopRunning
        {
            get => _isGameLoopRunning;
            set
            {
                base.SetProperty(ref _isGameLoopRunning, value);
                base.RaisePropertyChanged(() => StartStopGameLoopText);
            }
        }

        public string CurrentGenerationText
        {
            get => $"Current Generation: {_currentGeneration}";
        }

        public string StartStopGameLoopText
        {
            get => (IsGameLoopRunning) ? "Stop" : "Start";
        }

        public ICommand LoadNextGenerationCommand { private set; get; }

        public ICommand StopStartGameLoopCommand { private set; get; }

        private const double SecondsBetweenTicks = 0.5;
        private const int Rows = 10;
        private const int Columns = 10;

        public GameViewModel()
        {
            Cells = new CellViewModel[Columns, Rows];
            for (var x = 0; x < Columns; x++)
            {
                for (var y = 0; y < Rows; y++)
                {
                    var isAlive = NextBool(_random); // 50% chance of true or false.
                    Cells[x, y] = new CellViewModel(isAlive);
                }
            }
            LoadNextGenerationCommand = new Command(() => LoadNextGeneration());
            StopStartGameLoopCommand = new Command(() =>
            {
                IsGameLoopRunning = !IsGameLoopRunning;

                IDispatcher dispatcher = Application.Current.Dispatcher;

                dispatcher.StartTimer(TimeSpan.FromSeconds(SecondsBetweenTicks), () =>
                {
                    LoadNextGenerationCommand.Execute(this);
                    return IsGameLoopRunning;
                });
            });
        }

        public void LoadNextGeneration()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            lock (_cells.SyncRoot)
            {
                // Rules:
                // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                // Any live cell with two or three live neighbours lives on to the next generation.
                // Any live cell with more than three live neighbours dies, as if by overpopulation.
                // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                UpdateAllCellsState();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CurrentGeneration += 1;
                    IsBusy = false;
                });
            }
        }

        #region Private methods

        private static readonly Random _random = new Random();

        private bool NextBool(Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }

        private void UpdateAllCellsState()
        {
            var isCurrentThreadHolderOfCellLock = Monitor.IsEntered(_cells);
            var previousCells = (CellViewModel[,])Cells.Clone();

            Parallel.For(0, Columns, x =>
            {
                Parallel.For(0, Rows, y =>
                {
                int numberOfAliveNeighbours = 0;

                for (int xi = x - 1; xi <= x + 1; xi++)
                    for (int yi = y - 1; yi <= y + 1; yi++)
                    {
                        if (xi >= 0 && yi >= 0 && xi < Columns && yi < Rows)
                        {
                            // Ensure it's not our current cell that we're checking. 
                            if (!((y == yi) && (x == xi)))
                            {
                                numberOfAliveNeighbours += previousCells[xi, yi].IsAlive ? 1 : 0;
                            }
                        }
                    }

                var cell = _cells[x, y];

                var isAlive = false;

                if (cell.IsAlive)
                {
                    // Determine if the cell should live or die. 
                    if (numberOfAliveNeighbours < 2)
                    {
                        // Rule 1 - underpopulation
                        isAlive = false;
                    }
                    else if (numberOfAliveNeighbours > 3)
                    {
                        // Rule 3 - overpopulation
                        isAlive = false;
                    }
                    else
                    {
                        // Rule 2 - lives on to the next generation
                        isAlive = true;
                    }
                }
                else
                {
                    if (numberOfAliveNeighbours == 3)
                    {
                        // Rule 4 - reproduction
                        isAlive = true;
                    }
                }

                    MainThread.BeginInvokeOnMainThread(() => cell.IsAlive = isAlive);
                });

            });
            #endregion
        }
    }
}

