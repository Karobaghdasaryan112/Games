using Chess.Entities;
using Chess.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess.Services
{
    public class BoardBlockService : IBoardBlockService
    {
        private IAnimationService _animatonService;
        public const int BOARD_SIZE = 8;
        public BoardBlockService(IAnimationService animatonService)
        {
            _animatonService = animatonService;
        }

        public BoardBlock SetBoardBlockOnBoard<TFigure>(SolidColorBrush boardNormalColor, TFigure figure, Position position, Grid boardGrid) where TFigure : IFigure
        {
            var VerticalOrientation = (int)position.GetVerticalOrientation();
            var HorizontalOrientation = (int)position.GetHorizontalOrientation();

            Grid cellContainer = new Grid();

            Rectangle RectangleForAnimation = new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Opacity = 1.0,
                Fill = boardNormalColor
            };


            BoardBlock newBoardBlockWithFigure = new BoardBlock()
            {
                RectangleGrid = cellContainer,
                RectangleForAnimation = RectangleForAnimation,
                ActualColor = boardNormalColor,
                Position = new Position(position.GetVerticalOrientation(), position.GetHorizontalOrientation()),
                Figure = figure,
                FigureImage = figure?.GetImage()
            };

            newBoardBlockWithFigure.Figure?.Initialize();

            cellContainer.Children.Add(RectangleForAnimation);
            if (figure?.GetImage() != null)
            {
                cellContainer.Children.Add(figure.GetImage());
                figure.GetImage().MouseEnter += (s, e) => RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseEnterEvent });
                figure.GetImage().MouseLeave += (s, e) => RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseLeaveEvent });

                figure.GetImage().MouseLeftButtonUp += (s, e) =>
                _animatonService.MovableBlocksAnimation(
                    figure.MovableBlocks(boardGrid, position.GetVerticalOrientation(), position.GetHorizontalOrientation(), BOARD_SIZE),
                    BoardBlock.EVENT_COLOR,
                    BoardBlock.MOUSE_ENTER_OPACITY,
                    BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);

            }

            RectangleForAnimation.MouseEnter += (s, e) => _animatonService.AnimateCell(RectangleForAnimation, BoardBlock.MOVE_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
            RectangleForAnimation.MouseLeave += (s, e) => _animatonService.AnimateCell(RectangleForAnimation, newBoardBlockWithFigure.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
            
            Grid.SetRow(cellContainer, VerticalOrientation);
            Grid.SetColumn(cellContainer, HorizontalOrientation);
            boardGrid.Children.Add(cellContainer);

            return newBoardBlockWithFigure;
        }
    }
}
