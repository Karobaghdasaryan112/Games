using Chess.Entities.Figures;
using Chess.Enums;
using Chess.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Color = Chess.Enums.Color;

namespace Chess.Entities
{
    public class Board : IBoard
    {
        private const int BOARD_SIZE = 8;
        private const double MOUSE_ENTER_OPACITY = 0.6;
        private const double NOUSE_LEAVE_OPACITY = 1.0;
        private const int MOUSE_ENTER_RECTANGLE_RADIUS = 16;
        private const int MOUSE_LEAVE_RECTANGLE_RADIUS = 0;
        private const int ANIMATION_MILISECOND = 300;
        private readonly SolidColorBrush EVENT_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFA9A9A9"));
        private readonly SolidColorBrush WHITE_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FF2F4F4F"));
        private readonly SolidColorBrush BLACK_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFDEB887"));

       
        public void CreateExistPositions(object[] Params, Grid BoardGrid)
        {

        }
        public void CreateStartPositions(Grid BoardGrid)
        {
            //SolidColorBrush white = Brushes.BurlyWood;
            BoardGrid.Children.Clear();
            BoardGrid.RowDefinitions.Clear();
            BoardGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition());
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    bool isWhite = (row + col) % 2 == 0;

                    SolidColorBrush normalColor = isWhite ? WHITE_COLOR : BLACK_COLOR;
                    SolidColorBrush hoverColor = EVENT_COLOR;

                    Rectangle cell = new Rectangle
                    {
                        Fill = normalColor,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Opacity = NOUSE_LEAVE_OPACITY,

                    };

                    cell.MouseEnter += (s, e) => AnimateCell((Rectangle)s, EVENT_COLOR, MOUSE_ENTER_OPACITY, MOUSE_ENTER_RECTANGLE_RADIUS);
                    cell.MouseLeave += (s, e) => AnimateCell((Rectangle)s, normalColor, NOUSE_LEAVE_OPACITY, MOUSE_LEAVE_RECTANGLE_RADIUS);


                    SetRowAndColumn(BoardGrid, cell, row, col);

                }
            }
            CreateFigures(BoardGrid);

        }

        private void AnimateCell<TElement>(TElement cell, SolidColorBrush newColor, double newOpacity, double radius) where TElement : UIElement
        {
            var animationOpacity = new DoubleAnimation(newOpacity, TimeSpan.FromMilliseconds(ANIMATION_MILISECOND));
            var animationRadius = new DoubleAnimation(radius, TimeSpan.FromMilliseconds(ANIMATION_MILISECOND));

            cell.BeginAnimation(UIElement.OpacityProperty, animationOpacity);
            cell.BeginAnimation(Rectangle.RadiusXProperty, animationRadius);
            cell.BeginAnimation(Rectangle.RadiusYProperty, animationRadius);

            if (cell is Shape cellShape)
                cellShape.Fill = newColor;
        }

        private void SetRowAndColumn(Grid BoardGrid, Rectangle rectangle, int row, int col)
        {
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, col);
            BoardGrid.Children.Add(rectangle);
        }

        private void SetFigureOnBoard(IFigure figure, Grid boardGrid)
        {

            figure.Initialize();
            Image FigureImage = figure.GetImage();

            int row = (int)figure.
                GetPosition().
                GetVerticalOrientation();
            int col = (int)figure.
                GetPosition().
                GetHorizontalOrientation();

            bool isWhite = (row + col) % 2 == 0;

            SolidColorBrush normalColor = isWhite ? WHITE_COLOR : BLACK_COLOR;

            Grid cellContainer = new Grid();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Rectangle existingCell = boardGrid.Children
                .OfType<Rectangle>()
                .FirstOrDefault(r => Grid.GetRow(r) == row && Grid.GetColumn(r) == col);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.


            if (existingCell != null)
            {
                boardGrid.Children.Remove(existingCell);
                cellContainer.Children.Add(existingCell);
                existingCell.MouseEnter += (s, e) => AnimateCell(existingCell, EVENT_COLOR, MOUSE_ENTER_OPACITY, MOUSE_ENTER_RECTANGLE_RADIUS);
                existingCell.MouseLeave += (s, e) => AnimateCell(existingCell, normalColor, NOUSE_LEAVE_OPACITY, MOUSE_LEAVE_RECTANGLE_RADIUS);
            }

            cellContainer.Children.Add(FigureImage);
            Grid.SetRow(cellContainer, row);
            Grid.SetColumn(cellContainer, col);

            boardGrid.Children.Add(cellContainer);


            FigureImage.MouseEnter += (s, e) => existingCell?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseEnterEvent });
            FigureImage.MouseLeave += (s, e) => existingCell?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseLeaveEvent });
        }


        private void CreateFigures(Grid BoardGrid)
        {
            //white pawn
            //Black pawn
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                SetFigureOnBoard(new Pawn(new Position(VerticalOrientation.b, (HorizontalOrientation)i), Color.White), BoardGrid);
                SetFigureOnBoard(new Pawn(new Position(VerticalOrientation.g, (HorizontalOrientation)i), Color.Black), BoardGrid);
            }
            //white Bishop
            SetFigureOnBoard(new Bishop(new Position(VerticalOrientation.a, HorizontalOrientation.position2), Color.White), BoardGrid);
            SetFigureOnBoard(new Bishop(new Position(VerticalOrientation.a, HorizontalOrientation.position5), Color.White), BoardGrid);
            //Black Bishop
            SetFigureOnBoard(new Bishop(new Position(VerticalOrientation.h, HorizontalOrientation.position2), Color.Black), BoardGrid);
            SetFigureOnBoard(new Bishop(new Position(VerticalOrientation.h, HorizontalOrientation.position5), Color.Black), BoardGrid);

            //white knight
            SetFigureOnBoard(new Knight(new Position(VerticalOrientation.a, HorizontalOrientation.position1), Color.White), BoardGrid);
            SetFigureOnBoard(new Knight(new Position(VerticalOrientation.a, HorizontalOrientation.position6), Color.White), BoardGrid);

            //Black knight
            SetFigureOnBoard(new Knight(new Position(VerticalOrientation.h, HorizontalOrientation.position1), Color.Black), BoardGrid);
            SetFigureOnBoard(new Knight(new Position(VerticalOrientation.h, HorizontalOrientation.position6), Color.Black), BoardGrid);


            //Black Rook
            SetFigureOnBoard(new Rook(new Position(VerticalOrientation.h, HorizontalOrientation.position0), Color.Black), BoardGrid);
            SetFigureOnBoard(new Rook(new Position(VerticalOrientation.h, HorizontalOrientation.position7), Color.Black), BoardGrid);

            //White Rook
            SetFigureOnBoard(new Rook(new Position(VerticalOrientation.a, HorizontalOrientation.position0), Color.White), BoardGrid);
            SetFigureOnBoard(new Rook(new Position(VerticalOrientation.a, HorizontalOrientation.position7), Color.White), BoardGrid);

            //Black Queen
            SetFigureOnBoard(new Queen(new Position(VerticalOrientation.h, HorizontalOrientation.position3), Color.Black), BoardGrid);

            //White Queen
            SetFigureOnBoard(new Queen(new Position(VerticalOrientation.a, HorizontalOrientation.position3), Color.White), BoardGrid);

            //Black King
            SetFigureOnBoard(new King(new Position(VerticalOrientation.h, HorizontalOrientation.position4), Color.Black), BoardGrid);

            //White King
            SetFigureOnBoard(new King(new Position(VerticalOrientation.a, HorizontalOrientation.position4), Color.White), BoardGrid);

        }
    }
}   
