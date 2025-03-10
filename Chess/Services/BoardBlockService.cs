using Chess.Entities;
using Chess.Enums;
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
                StrokeThickness = 1.0,
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
                SetFigureAnimations(boardGrid, position.GetVerticalOrientation(), position.GetHorizontalOrientation(), figure, cellContainer, newBoardBlockWithFigure);

            }  
            else
                cellContainer.Children.Add(new Image());

            SetEmptyBoardAnimations(newBoardBlockWithFigure, boardGrid);

            Grid.SetRow(cellContainer, VerticalOrientation);
            Grid.SetColumn(cellContainer, HorizontalOrientation);
            boardGrid.Children.Add(cellContainer);

            return newBoardBlockWithFigure;
        }

        public void FigureMove(BoardBlock figureBoardBlock, BoardBlock moveBoardBlock, Grid boardGrid)
        {


            if (figureBoardBlock == null || moveBoardBlock == null)
                return;

            if (figureBoardBlock.Figure == null)
                return;

            //changed

            if (BoardService.BoardPaintedToMoveBlocks.Contains(moveBoardBlock) && moveBoardBlock.RectangleForAnimation.Fill == BoardBlock.EVENT_COLOR)
            {
                moveBoardBlock.SetFigureImageIntoRectangle(figureBoardBlock.FigureImage);

                moveBoardBlock.Figure = figureBoardBlock.Figure;

                moveBoardBlock.Figure.SetImage(moveBoardBlock.FigureImage);

                SetEmptyBoardAnimations(moveBoardBlock, boardGrid);

                SetFigureAnimations(boardGrid, moveBoardBlock.Position.GetVerticalOrientation(), moveBoardBlock.Position.GetHorizontalOrientation(), moveBoardBlock.Figure, moveBoardBlock.RectangleGrid, moveBoardBlock);

                figureBoardBlock.SetFigureImageIntoRectangle(default);

                figureBoardBlock.Figure = default;

                BoardService.TurnSwitch();
            }
        }

        public void SetFigureAnimations(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, IFigure newFigure, Grid RectangleGridForBlockBoard, BoardBlock newBoardBlockWithFigure)
        {
            if (RectangleGridForBlockBoard.Children.OfType<Image>().FirstOrDefault() == null)
            {
                RectangleGridForBlockBoard.Children.Add(newFigure.GetImage());
            }
            newFigure.GetImage().MouseEnter += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseEnterEvent });
            newFigure.GetImage().MouseLeave += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseLeaveEvent });

            newFigure.GetImage().MouseLeftButtonUp += (s, e) =>
            {
                if (BoardService.Turn.ToString() == newFigure.GetColor().ToString())
                {
                    _animatonService.MovableBlocksAnimation(newBoardBlockWithFigure,
                        newFigure.MovableBlocks(boardGrid, verticalOrientation, horizontalOrientation),
                        BoardBlock.EVENT_COLOR,
                        BoardBlock.CUT_COLOR,
                        BoardBlock.MOUSE_ENTER_OPACITY,
                        BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                }
            };
        }
        public void SetEmptyBoardAnimations(BoardBlock newBoardBlockWithFigure, Grid boardGrid)
        {
            newBoardBlockWithFigure.RectangleForAnimation.MouseLeftButtonUp += (s, e) => FigureMove(BoardService.FigureAndMoveBlocks[0], newBoardBlockWithFigure, boardGrid);
            newBoardBlockWithFigure.RectangleForAnimation.MouseLeftButtonUp += (s, e) => _animatonService.MovableBlocksAnimationDisable();
            newBoardBlockWithFigure.RectangleForAnimation.MouseEnter += (s, e) => _animatonService.AnimateCell(newBoardBlockWithFigure, BoardBlock.MOVE_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
            newBoardBlockWithFigure.RectangleForAnimation.MouseLeave += (s, e) => _animatonService.AnimateCell(newBoardBlockWithFigure, newBoardBlockWithFigure.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
        }
    }
}
