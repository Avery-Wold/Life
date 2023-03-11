using Life.ViewModels;

namespace Life.Views;

public partial class GameView : ContentPage
{
    GameViewModel viewModel;

    public GameView()
    {
        InitializeComponent();

        this.viewModel = new GameViewModel();

        viewModel.Navigation = Navigation;
        BindingContext = viewModel;

        DrawGrid();
    }

    private void DrawGrid()
    {
        var cells = viewModel.Cells;

        var numberOfRows = cells.GetLength(0);
        var numberOfColumns = cells.GetLength(1);

        for (var x = 0; x < numberOfColumns; x++)
        {
            this.CanvasGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }
        for (var y = 0; y < numberOfRows; y++)
        {
            this.CanvasGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                var cell = cells[x, y];
                var itemToAdd = new Frame
                {
                    Padding = 2,
                    CornerRadius = 0,
                    HasShadow = false
                };
                var cellTapGestureRecognizer = new TapGestureRecognizer();
                cellTapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, nameof(cell.ToggleCellStateCommand));
                itemToAdd.GestureRecognizers.Add(cellTapGestureRecognizer);
                itemToAdd.SetBinding(BackgroundColorProperty, nameof(cell.BackgroundColor));

                itemToAdd.BindingContext = cell;

                CanvasGrid.Add(itemToAdd, x, y);
            }
        }
    }
}