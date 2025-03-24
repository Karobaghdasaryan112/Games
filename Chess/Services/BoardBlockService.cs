using Chess.Entities;
using Chess.Entities.Figures;
using Chess.Enums;
using Chess.Extentions;
using Chess.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess.Services
{
    public class BoardBlockService : IBoardBlockService
    {
        private bool _isProcessingForFigureEvent = false;
        private bool _isProcessingForEmptyBoard = false;
        private IAnimationService _animatonService;
        private IFigureService _figureService;

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

            newBoardBlockWithFigure.RectangleForAnimation.MouseLeftButtonUp += async (s, e) => { e.Handled = true; await SetEmptyBoardAnimations(newBoardBlockWithFigure, boardGrid); };
            SetMouseLeaveAndEnterAnimations(newBoardBlockWithFigure);

            Grid.SetRow(cellContainer, VerticalOrientation);
            Grid.SetColumn(cellContainer, HorizontalOrientation);

            boardGrid.Children.Add(cellContainer);

            return newBoardBlockWithFigure;
        }

        public async Task SetFigureAnimations(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, IFigure newFigure, Grid RectangleGridForBlockBoard, BoardBlock newBoardBlockWithFigure)
        {
            if (newBoardBlockWithFigure.Figure == null)
                return;

            if (BoardService.IsChecked)
            {
                if (BoardService.FigureAndMoveBlocks[0] == null)
                {
                    if (newBoardBlockWithFigure.Figure.GetColor().ToString() != BoardService.Turn.ToString())
                        return;
                }
                else
                {
                    if (BoardService.FigureAndMoveBlocks[0]?.Figure == null)
                    {
                        if (newBoardBlockWithFigure.Figure.GetColor().ToString() == BoardService.Turn.ToString() ||
                            BoardService.FigureAndMoveBlocks[0].Figure.GetColor().ToString() != BoardService.Turn.ToString())
                            return;
                    }
                }
            }

            if (BoardService.FigureAndMoveBlocks[0] == newBoardBlockWithFigure)
            {
                await _animatonService.MovableBlocksAnimationDisable();
                BoardService.FigureAndMoveBlocks[0] = null;
                return;
            }

            if (BoardService.Turn.ToString() == newFigure.GetColor().ToString())
            {
                await _animatonService.MovableBlocksAnimation(
                    newBoardBlockWithFigure,
                    newFigure.MovableBlocks(verticalOrientation, horizontalOrientation, newFigure.GetColor()),
                    BoardBlock.EVENT_COLOR,
                    BoardBlock.CUT_COLOR,
                    BoardBlock.MOUSE_ENTER_OPACITY,
                    BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);

                if (newFigure.GetType() == typeof(King))
                {
                    var ListOfBoardBlocks = _figureService.CastlingMoveAnimation(boardGrid);
                    BoardService.KingAndRookBlocks = ListOfBoardBlocks;
                }
            }


            if (BoardService.FigureAndMoveBlocks[0]?.Figure?.GetColor().ToString() !=
                newBoardBlockWithFigure.Figure?.GetColor().ToString() && BoardService.FigureAndMoveBlocks[0]?.Figure != default &&
                newBoardBlockWithFigure.RectangleForAnimation.Fill == BoardBlock.CUT_COLOR)
            {
                BoardBlock cuttingFigureBefore = (BoardBlock)((ICloneable)BoardService.FigureAndMoveBlocks[0]).Clone();
                BoardBlock cuttedFigureBefore = (BoardBlock)((ICloneable)newBoardBlockWithFigure).Clone();
                cuttingFigureBefore.RectangleForAnimation.Fill = cuttingFigureBefore.ActualColor;
                await  _figureService.FigureCut(BoardService.FigureAndMoveBlocks[0], newBoardBlockWithFigure, boardGrid);
                SetAllAnimationsIntoBoardBlock(newBoardBlockWithFigure, boardGrid);
                FigureService.IsPawnCastling = false;
                
                await IsKingCheckedForeMove(boardGrid, cuttingFigureBefore, cuttedFigureBefore);

                BoardService.FigureAndMoveBlocks[0] = null;

                if (BoardService.WrongMoveIfKingIsChecked)
                {
                    return;
                }
                else
                    BoardService.IsChecked = false;

            }
            SetAllAnimationsIntoBoardBlock(newBoardBlockWithFigure, boardGrid);


        }
        public async Task SetEmptyBoardAnimations(BoardBlock newBoardBlockWithFigure, Grid boardGrid)
        {
            if (BoardService.FigureAndMoveBlocks[0] != null)
            {
                if (BoardService.FigureAndMoveBlocks[0]?.Figure?.GetColor().ToString() == BoardService.Turn.ToString())
                {
                    var IsMovable = await _figureService.FigureMove(BoardService.FigureAndMoveBlocks[0], newBoardBlockWithFigure, boardGrid);
                    SetAllAnimationsIntoBoardBlock(newBoardBlockWithFigure, boardGrid);
                    FigureService.IsPawnCastling = false;
                    


                    if (BoardService.FigureAndMoveBlocks[0]?.Figure?.GetType() == typeof(King))
                    {
                        _figureService.CastlingLogic(BoardService.KingAndRookBlocks, newBoardBlockWithFigure, boardGrid);

                        if (await _figureService.IsKingCheckedCondition())
                        {
                            if (BoardService.WrongMoveIfKingIsChecked)
                            {
                                MessageBox.Show("Wrong Move");


                                foreach (var item in BoardService.BeengCastleAndEmptyBlocks)
                                    _figureService.RoleBackAfterMoving(item[0], item[1], boardGrid);


                                var CheckedKing = BoardService.BoardBlocks.Where(block =>
                                     block.Figure?.GetColor().ToString() != BoardService.Turn.ToString() &&
                                     block.Figure?.GetFigureName() == "King").
                                     FirstOrDefault();

                                BoardService.TurnSwitch();

                                if (CheckedKing != null && BoardService.WrongCheckedKing)
                                    await _animatonService.AnimateCell(CheckedKing, BoardBlock.CHECKED_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);

                            }
                        }
                        foreach (var item in BoardService.BeengCastleAndEmptyBlocks)
                        {

                            SetAllAnimationsIntoBoardBlock(item[0], boardGrid);
                            SetAllAnimationsIntoBoardBlock(item[1], boardGrid);

                        }
                        if (BoardService.KingAndRookBlocks != null)
                        {
                            BoardService.KingAndRookBlocks.Clear();
                        }
                        BoardService.BeengCastleAndEmptyBlocks.Clear();
                        return;
                    }

                    //await Task.Delay(5000);
                    if (IsMovable)
                    {
                        await IsKingCheckedForeMove(boardGrid, BoardService.FigureAndMoveBlocks[0], newBoardBlockWithFigure);
                        await _animatonService.MovableBlocksAnimationDisable();
                        await SetEmptyBoardAnimations(BoardService.FigureAndMoveBlocks[0], boardGrid);
                        BoardService.FigureAndMoveBlocks[0] = null;

                        if (BoardService.WrongMoveIfKingIsChecked)
                        {
                            return;
                        }
                        else
                            BoardService.IsChecked = false;

                        SetAllAnimationsIntoBoardBlock(newBoardBlockWithFigure, boardGrid);
                    }
                }
            }
        }

        public void SetMouseLeaveAndEnterAnimations(BoardBlock newBoardBlockWithFigure)
        {

            newBoardBlockWithFigure.RectangleForAnimation.MouseEnter += (s, e) =>
            {
                if (newBoardBlockWithFigure.RectangleForAnimation.Fill != BoardBlock.CHECKED_COLOR && newBoardBlockWithFigure.RectangleForAnimation.Fill != BoardBlock.CASTLING_COLOR)
                    _animatonService.AnimateCell(newBoardBlockWithFigure, BoardBlock.MOVE_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);

            };

            newBoardBlockWithFigure.RectangleForAnimation.MouseLeave += (s, e) =>
            {
                if (newBoardBlockWithFigure.RectangleForAnimation.Fill != BoardBlock.CHECKED_COLOR && newBoardBlockWithFigure.RectangleForAnimation.Fill != BoardBlock.CASTLING_COLOR)
                    _animatonService.AnimateCell(newBoardBlockWithFigure, newBoardBlockWithFigure.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
            };

        }

        public async Task IsKingCheckedForeMove(Grid boardGrid, BoardBlock cuttingFigureBefore, BoardBlock cuttedFigureBefore)
        {
            if (await _figureService.IsKingCheckedCondition())
            {
                if (BoardService.WrongMoveIfKingIsChecked)
                {
                    MessageBox.Show("Wrong Move");

                    foreach (var item in BoardService.BoardBlocks)
                    {
                        if (item.Figure?.GetColor().ToString() == BoardService.Turn.ToString())
                            item.RectangleForAnimation.Fill = item.ActualColor;
                    }

                    if (!BoardService.IsChecked)
                    {
                        var Kings = BoardService.BoardBlocks.Where(block =>
                            block.Figure?.GetFigureName() == "King");

                        foreach (var item in Kings)
                            item.RectangleForAnimation.Fill = item.ActualColor;
                    }

                    //await Task.Delay(3000);
                    _figureService.RoleBackAfterMoving(cuttingFigureBefore, cuttedFigureBefore, boardGrid);

                    var OriginalCuttingFigureBefore = BoardService.BoardBlocks.GetElement(cuttingFigureBefore.Position);
                    var OriginalCuttedFigureBefore = BoardService.BoardBlocks.GetElement(cuttedFigureBefore.Position);

                    SetAllAnimationsIntoBoardBlock(OriginalCuttingFigureBefore, boardGrid);

                    SetAllAnimationsIntoBoardBlock(OriginalCuttedFigureBefore, boardGrid);

                }
                else
                {
                    MessageBox.Show("Your King Is Checked");
                }

                await _figureService.IsMateState(boardGrid);

                if (!await _figureService.IsKingCheckedCondition())
                {
                    var Kings = BoardService.BoardBlocks.Where(block =>
                           block.Figure?.GetFigureName() == "King");

                    foreach (var item in Kings)
                        item.RectangleForAnimation.Fill = item.ActualColor;
                }

                if (BoardService.TryingToUnCheckedBoardBlocks.Count != 0)
                {
                    foreach (var block in BoardService.TryingToUnCheckedBoardBlocks)
                    {
                        SetAllAnimationsIntoBoardBlock(block, boardGrid);
                    }
                }
            }
            BoardService.WrongMoveIfKingIsChecked = false;
        }

        public void SetAllAnimationsIntoBoardBlock(BoardBlock boardBlock, Grid boardGrid)
        {
            if (boardBlock.Figure != null)
            {

                boardBlock.Figure.GetImage().MouseLeftButtonUp += async (s, e) =>
                {
                    e.Handled = true;
                    await SetFigureAnimations(
                        boardGrid,
                        boardBlock.Position.GetVerticalOrientation(),
                        boardBlock.Position.GetHorizontalOrientation(),
                        boardBlock.Figure,
                        boardBlock.RectangleGrid,
                        boardBlock);
                };
                _figureService.SetImageEventIntoBoardBlockEvent(boardBlock.Figure, boardBlock);
            }
            else
            {
                boardBlock.RectangleForAnimation.MouseLeftButtonUp += async (s, e) => { e.Handled = true; await SetEmptyBoardAnimations(boardBlock, boardGrid); };
            }

            SetMouseLeaveAndEnterAnimations(boardBlock);
        }


    }
}