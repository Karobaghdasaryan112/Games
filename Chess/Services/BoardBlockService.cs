using Chess.Entities;
using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess.Services
{
    public class BoardBlockService : IBoardBlockService
    {
        private IAnimationService _animatonService;
        private IFigureService _figureService;
        public const int BOARD_SIZE = 8;
        public BoardBlockService(IAnimationService animatonService, IFigureService figureService)
        {
            _animatonService = animatonService;
            _figureService = figureService;
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
                if (cellContainer.Children.OfType<Image>().FirstOrDefault() == null)
                {
                    cellContainer.Children.Add(figure.GetImage());
                }
                SetAllAnimationsIntoBoardBlock(newBoardBlockWithFigure, boardGrid);
            }
            else
                cellContainer.Children.Add(new Image());

            newBoardBlockWithFigure.RectangleForAnimation.MouseLeftButtonUp += (s, e) => SetEmptyBoardAnimations(newBoardBlockWithFigure, boardGrid);
            SetMouseLeaveAndEnterAnimations(newBoardBlockWithFigure);

            Grid.SetRow(cellContainer, VerticalOrientation);
            Grid.SetColumn(cellContainer, HorizontalOrientation);

            boardGrid.Children.Add(cellContainer);

            return newBoardBlockWithFigure;
        }

        public void SetFigureAnimations(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, IFigure newFigure, Grid RectangleGridForBlockBoard, BoardBlock newBoardBlockWithFigure)
        {
            if (newBoardBlockWithFigure.Figure == null)
                return;

            if (BoardService.Turn.ToString() == newFigure.GetColor().ToString())
                _animatonService.MovableBlocksAnimation(
                    newBoardBlockWithFigure,
                    newFigure.MovableBlocks(verticalOrientation, horizontalOrientation, newFigure.GetColor()),
                    BoardBlock.EVENT_COLOR,
                    BoardBlock.CUT_COLOR,
                    BoardBlock.MOUSE_ENTER_OPACITY,
                    BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);


            if (BoardService.FigureAndMoveBlocks[0]?.Figure?.GetColor().ToString() !=
                newBoardBlockWithFigure.Figure?.GetColor().ToString() && BoardService.FigureAndMoveBlocks[0]?.Figure != default)
            {
                BoardBlock cuttingFigureBefore = (BoardBlock)((ICloneable)BoardService.FigureAndMoveBlocks[0]).Clone();
                BoardBlock cuttedFigureBefore = (BoardBlock)((ICloneable)newBoardBlockWithFigure).Clone();

                _figureService.FigureCut(BoardService.FigureAndMoveBlocks[0], newBoardBlockWithFigure, boardGrid);

                IsKingCheckedForeMove(boardGrid, cuttingFigureBefore, cuttedFigureBefore);

                if (BoardService.WrongMoveIfKingIsChecked)
                    return;

            }
            SetAllAnimationsIntoBoardBlock(newBoardBlockWithFigure, boardGrid);
        }

        public void SetEmptyBoardAnimations(BoardBlock newBoardBlockWithFigure, Grid boardGrid)
        {
            if (BoardService.FigureAndMoveBlocks[0] != null)
            {
                _figureService.FigureMove(BoardService.FigureAndMoveBlocks[0], newBoardBlockWithFigure, boardGrid);

                IsKingCheckedForeMove(boardGrid, BoardService.FigureAndMoveBlocks[0], newBoardBlockWithFigure);

                SetAllAnimationsIntoBoardBlock(newBoardBlockWithFigure, boardGrid);
            }
        }


        public void SetMouseLeaveAndEnterAnimations(BoardBlock newBoardBlockWithFigure)
        {
            newBoardBlockWithFigure.RectangleForAnimation.MouseEnter += (s, e) =>
            _animatonService.AnimateCell(newBoardBlockWithFigure, BoardBlock.MOVE_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);

            newBoardBlockWithFigure.RectangleForAnimation.MouseLeave += (s, e) =>
            _animatonService.AnimateCell(newBoardBlockWithFigure, newBoardBlockWithFigure.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
        }


        public void IsKingCheckedForeMove(Grid boardGrid, BoardBlock cuttingFigureBefore, BoardBlock cuttedFigureBefore)
        {
            if (_figureService.IsKingCheckedCondition())
            {
                if (BoardService.WrongMoveIfKingIsChecked)
                {
                    _figureService.RoleBackAfterMoving(ref cuttingFigureBefore, ref cuttedFigureBefore, boardGrid);

                    SetAllAnimationsIntoBoardBlock(cuttingFigureBefore, boardGrid);

                    SetAllAnimationsIntoBoardBlock(cuttedFigureBefore, boardGrid);
                }
            }
        }

        public void SetAllAnimationsIntoBoardBlock(BoardBlock boardBlock, Grid boardGrid)
        {
            if (boardBlock.Figure != null)
            {
                boardBlock.Figure.GetImage().MouseLeftButtonUp += (s, e) =>
                SetFigureAnimations(
                    boardGrid,
                    boardBlock.Position.GetVerticalOrientation(),
                    boardBlock.Position.GetHorizontalOrientation(),
                    boardBlock.Figure,
                    boardBlock.RectangleGrid,
                    boardBlock);


                _figureService.SetImageEventIntoBoardBlockEvent(boardBlock.Figure, boardBlock);

                SetMouseLeaveAndEnterAnimations(boardBlock);

            }
        }
    }
}

