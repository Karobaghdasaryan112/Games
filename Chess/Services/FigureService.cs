﻿using Chess.Entities;
using Chess.Entities.Figures;
using Chess.Enums;
using Chess.Extentions;
using Chess.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
        
namespace Chess.Services
{
    public class FigureService : IFigureService
    {
        public const string PAWN = "Pawn";
        public const string KING = "King";
        public const string QUEEN = "Queen";
        public const string BISHOP = "Bishop";
        public const string ROOK = "Rook";
        public const string KNIGHT = "Knight";
        public const int BOARD_SIZE = 8;
        public bool IsChecking = true;
        public IAnimationService _animationService;

        public FigureService(IAnimationService animationService)
        {
            _animationService = animationService;
        }

        public void FigureCut(BoardBlock cuttingBoardblock, BoardBlock cuttedBoardblock, Grid boardGrid)
        {
            if (cuttedBoardblock?.Figure == default || cuttingBoardblock?.Figure == default)
                return;

            if (cuttedBoardblock.RectangleForAnimation.Fill.ToString() == BoardBlock.CUT_COLOR.ToString())
            {

                ReInitializeFiguresAndEmptiBoardBlocks(cuttingBoardblock, cuttedBoardblock, default, default);
                CheckToMovableInRookAndKing(cuttedBoardblock);
                _animationService.MovableBlocksAnimationDisable();
                BoardService.TurnSwitch();
            }
        }

        public void FigureMove(BoardBlock figureBoardBlock, BoardBlock moveBoardBlock, Grid boardGrid)
        {
            if (figureBoardBlock == null || moveBoardBlock == null)
                return;

            if (figureBoardBlock.Figure == null)
                return;

            if (BoardService.BoardPaintedToMoveBlocks.Contains(moveBoardBlock) && moveBoardBlock.RectangleForAnimation.Fill == BoardBlock.EVENT_COLOR)
            {
                ReInitializeFiguresAndEmptiBoardBlocks(figureBoardBlock, moveBoardBlock, default, default);
                CheckToMovableInRookAndKing(moveBoardBlock);
                _animationService.MovableBlocksAnimationDisable();
                BoardService.TurnSwitch();
            }
        }
        private void CheckToMovableInRookAndKing(BoardBlock boardBlock)
        {
            if (boardBlock == null) return;
            if (boardBlock.Figure == null) return;

            if (boardBlock.Figure is King king)
                king.IsMoved = true;
            if (boardBlock.Figure is Rook rook)
                rook.IsMoved = true;

        }

        public void ReInitializeFiguresAndEmptiBoardBlocks(BoardBlock figureBoardBlock, BoardBlock moveBoardBlock, Image setImage, IFigure setNewFigure)
        {

            moveBoardBlock.SetFigureImageIntoRectangle(figureBoardBlock.FigureImage);

            moveBoardBlock.Figure = figureBoardBlock.Figure;

            moveBoardBlock.Figure?.SetImage(moveBoardBlock.FigureImage);

            figureBoardBlock.SetFigureImageIntoRectangle(setImage);

            figureBoardBlock.Figure = setNewFigure;
        }

        public async Task<bool> IsKingCheckedCondition()
        {
            var WhitekingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", Color.White);
            var BlackkingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", Color.Black);

            if (WhitekingBlock != null)
            {
                var verticalOrientationOfWhiteKingBlock = WhitekingBlock.Position.GetVerticalOrientation();
                var horizontalOrientationOfWhiteKingBlock = WhitekingBlock.Position.GetHorizontalOrientation();

                await CheckedLogic(Color.White, WhitekingBlock, verticalOrientationOfWhiteKingBlock, horizontalOrientationOfWhiteKingBlock);


                if (BoardService.IsChecked)
                    return true;
            }

            if (BlackkingBlock != null)
            {
                var verticalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetVerticalOrientation();
                var horizontalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetHorizontalOrientation();

                await CheckedLogic(Color.Black, BlackkingBlock, verticalOrientationOfBlackKingBlock, horizontalOrientationOfBlackKingBlock);
            }

            return BoardService.IsChecked;
        }

        private bool IsAttackedBy(IFigure virtualFigure, BoardBlock kingBlock, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, List<string> attackingFigures)
        {
            var attackBlocks = virtualFigure.MovableBlocks(verticalOrientation, horizontalOrientation, virtualFigure.GetColor())[1];

            var AttackedFigureBoardBlocks = attackBlocks
                .Where(block => block.Figure != null && attackingFigures.Contains(block.Figure.GetFigureName()));

            if (AttackedFigureBoardBlocks.Count() != 0)
            {
                foreach (var block in AttackedFigureBoardBlocks)
                {
                    BoardService.AttackedFigureOnKing.Add(block);
                }
            }
            return AttackedFigureBoardBlocks.Any();
        }

        private async Task CheckedLogic(Color color, BoardBlock KingBoardBlock, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            BoardService.AttackedFigureOnKing.Clear();

            if (
               IsAttackedBy(new Pawn(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { PAWN }) ||
               IsAttackedBy(new Rook(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { ROOK, QUEEN }) ||
               IsAttackedBy(new Bishop(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { BISHOP, QUEEN }) ||
               IsAttackedBy(new Knight(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { KNIGHT }) ||
               IsAttackedBy(new Queen(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { QUEEN }) ||
               IsAttackedBy(new King(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { KING }))
            {
                if (BoardService.Turn.ToString() == KingBoardBlock.Figure.GetColor().ToString())
                {
                    BoardService.WrongMoveIfKingIsChecked = false;
                    ((King)KingBoardBlock.Figure).IsChecked = true;
                    BoardService.BoardPaintedToMoveBlocks.Add(KingBoardBlock);
                    _animationService.AnimateCell(KingBoardBlock, BoardBlock.CHECKED_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                }
                else
                    BoardService.WrongMoveIfKingIsChecked = true;
                BoardService.IsChecked = true;
            }
            else
            {
                BoardService.IsChecked = false;
                _animationService.AnimateCell(KingBoardBlock, KingBoardBlock.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
            }
            await Task.CompletedTask;
        }
        public void RoleBackAfterMoving(BoardBlock firstBoardBlock, BoardBlock secondBoardBlock, Grid boardGrid)
        {
            if (firstBoardBlock.Figure != null && secondBoardBlock.Figure != null)
            {
                var firstBlockOnBoard = BoardService.BoardBlocks.GetBoardBlockWithPosition(firstBoardBlock.Position);
                var secondBlockOnBoard = BoardService.BoardBlocks.GetBoardBlockWithPosition(secondBoardBlock.Position);

                secondBlockOnBoard.Figure = secondBoardBlock.Figure;
                secondBlockOnBoard.FigureImage = secondBoardBlock.FigureImage;
                secondBlockOnBoard.Figure.SetImage(secondBoardBlock.FigureImage);
                secondBlockOnBoard.SetFigureImageIntoRectangle(secondBoardBlock.FigureImage);

                firstBlockOnBoard.Figure = firstBoardBlock.Figure;
                firstBlockOnBoard.FigureImage = firstBoardBlock.FigureImage;
                firstBlockOnBoard.Figure.SetImage(firstBoardBlock.FigureImage);
                firstBlockOnBoard.SetFigureImageIntoRectangle(firstBoardBlock.FigureImage);

                BoardService.TryingToUnCheckedBoardBlocks.Add(firstBlockOnBoard);
                BoardService.TryingToUnCheckedBoardBlocks.Add(secondBlockOnBoard);
                BoardService.TurnSwitch();
            }
            if (firstBoardBlock.Figure == null)
            {
                VirtualFigureMoving(firstBoardBlock, secondBoardBlock, boardGrid);
            }
            _animationService.MovableBlocksAnimationDisable();

        }
        public void SetImageEventIntoBoardBlockEvent(IFigure newFigure, BoardBlock newBoardBlockWithFigure)
        {
            if (newFigure == null)
                return;

            newFigure.GetImage().MouseEnter += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseEnterEvent });
            newFigure.GetImage().MouseLeave += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseLeaveEvent });
        }

        public async Task IsMateState(Grid boardGrid)
        {

            var FigureColor = BoardService.Turn == Turn.White ? Color.White : Color.Black;
            var CheckedKingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", FigureColor);

            if (((King)CheckedKingBlock.Figure).IsChecked)
            {
                BoardService.TryingToUnCheckedBoardBlocks.Clear();
                BoardService.IsMateState = false;

                var PawnsBlock = BoardService.BoardBlocks.GetAllSameFigures(PAWN, FigureColor).ToList();

                var RooksBlock = BoardService.BoardBlocks.GetAllSameFigures(ROOK, FigureColor).ToList();
                var KnightsBlock = BoardService.BoardBlocks.GetAllSameFigures(KNIGHT, FigureColor).ToList();
                var BishopsBlock = BoardService.BoardBlocks.GetAllSameFigures(BISHOP, FigureColor).ToList();
                var QueensBlock = BoardService.BoardBlocks.GetAllSameFigures(QUEEN, FigureColor).ToList();
                var KingsBlock = BoardService.BoardBlocks.GetAllSameFigures(KING, FigureColor).ToList();


                if (await TryingToUncheckedKing(boardGrid, PawnsBlock, FigureColor) ||
                    await TryingToUncheckedKing(boardGrid, RooksBlock, FigureColor) ||
                    await TryingToUncheckedKing(boardGrid, KnightsBlock, FigureColor) ||
                    await TryingToUncheckedKing(boardGrid, BishopsBlock, FigureColor) ||
                    await TryingToUncheckedKing(boardGrid, QueensBlock, FigureColor) ||
                    await TryingToUncheckedKing(boardGrid, KingsBlock, FigureColor))
                {
                    BoardService.BoardPaintedToMoveBlocks.Add(CheckedKingBlock);
                    _animationService.AnimateCell(CheckedKingBlock, BoardBlock.CHECKED_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                    return;
                }
                BoardService.IsMateState = true;
                MessageBox.Show("Mate");
                GameBegin(boardGrid);
            }
            _animationService.AnimateCell(CheckedKingBlock, CheckedKingBlock.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
        }

        private async Task<bool> TryingToUncheckedKing(Grid boardGrid, List<BoardBlock> tryingBoardBlocks, Color figureColor)
        {
            for (int baseIndex = 0; baseIndex < tryingBoardBlocks.Count; baseIndex++)
            {
                var CutAndMoveBlocks = tryingBoardBlocks[baseIndex].Figure.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);

                var MoveBlocks = CutAndMoveBlocks[0];
                var CutBlocks = CutAndMoveBlocks[1];
                //Done
                for (int index = 0; index < MoveBlocks.Count; index++)
                {
                    var block1 = BoardService.BoardBlocks.Where(B => B == tryingBoardBlocks[baseIndex]).FirstOrDefault();
                    var block2 = BoardService.BoardBlocks.Where(B => B == MoveBlocks[index]).FirstOrDefault();
                    if (block2 == null)
                        continue;
                    block2.RectangleForAnimation.Fill = BoardBlock.EVENT_COLOR;
                    BoardService.BoardPaintedToMoveBlocks.Add(block2);
                    FigureMove(block1, block2, boardGrid);
                    BoardService.BoardPaintedToMoveBlocks.Clear();

                    if (!await IsKingCheckedCondition())
                    {

                        VirtualFigureMoving(block1, block2, boardGrid);

                        return true;
                    }
                    if (block2 != null)
                    {

                        VirtualFigureMoving(block1, block2, boardGrid);
                    }
                    CutAndMoveBlocks = tryingBoardBlocks[baseIndex].Figure.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);
                    MoveBlocks = CutAndMoveBlocks[0];
                }

                CutAndMoveBlocks = tryingBoardBlocks[baseIndex].Figure.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);
                CutBlocks = CutAndMoveBlocks[1];

                for (int index = 0; index < CutBlocks.Count; index++)
                {
                    var block1 = BoardService.BoardBlocks.Where(B => B == tryingBoardBlocks[baseIndex]).FirstOrDefault();
                    var block2 = BoardService.BoardBlocks.Where(B => B == CutBlocks[index]).FirstOrDefault();

                    var newCuttedBlock = (BoardBlock)((ICloneable)block2).Clone();
                    block2.RectangleForAnimation.Fill = BoardBlock.CUT_COLOR;
                    if(block2.Figure.GetType() == typeof(King))
                    {
                        var x = 10;
                    }
                    FigureCut(block1, block2, boardGrid);

                    if (!await IsKingCheckedCondition())
                    {
                        var MoveBlocksAgain = CutAndMoveBlocks[0];
                        var CutBlocksAgain = CutAndMoveBlocks[1];

                        VirtualFigureCuting(block1, block2, newCuttedBlock, boardGrid);
                        return true;
                    }

                    VirtualFigureCuting(block1, block2, newCuttedBlock, boardGrid);
                }
            }
            return false;
        }
        public void VirtualFigureMoving(BoardBlock block1, BoardBlock block2, Grid boardGrid)
        {
            BoardService.TryingToUnCheckedBoardBlocks.Add(block1);
            BoardService.TryingToUnCheckedBoardBlocks.Add(block2);
            block1.RectangleForAnimation.Fill = BoardBlock.EVENT_COLOR;
            BoardService.BoardPaintedToMoveBlocks.Add(block1);
            FigureMove(block2, block1, boardGrid);
            BoardService.BoardPaintedToMoveBlocks.Clear();
            block1.RectangleForAnimation.Fill = block1.ActualColor;
        }
        public void VirtualFigureCuting(BoardBlock block1, BoardBlock block2, BoardBlock block2Clone, Grid boardGrid)
        {
            BoardService.TryingToUnCheckedBoardBlocks.Add(block1);

            block1.RectangleForAnimation.Fill = BoardBlock.EVENT_COLOR;
            BoardService.BoardPaintedToMoveBlocks.Add(block1);
            FigureMove(block2, block1, boardGrid);
            BoardService.BoardPaintedToMoveBlocks.Clear();
            block2.Figure = block2Clone.Figure;
            block2.FigureImage = block2Clone.FigureImage;
            block2.SetFigureImageIntoRectangle(block2Clone.Figure.GetImage());
            block2.Figure.SetImage(block2.FigureImage);
            block2.RectangleForAnimation.Fill = block2.ActualColor;
            BoardService.TryingToUnCheckedBoardBlocks.Add(block2);

        }

        public List<BoardBlock[]> CastlingMoveAnimation(Grid boardGrid)
        {
            var Color = BoardService.Turn == Turn.Black ? Turn.Black : Turn.White;
            var ListOfBoardBlocks = new List<BoardBlock[]>();
            var verticalPosition = Color == Turn.Black ? VerticalOrientation.h : VerticalOrientation.a;

            var horizontalPosition = HorizontalOrientation.position4;

            var KingBlock = BoardService.BoardBlocks.GetElement(new Position(verticalPosition, horizontalPosition));
            var leftRook = BoardService.BoardBlocks.GetElement(new Position(verticalPosition, HorizontalOrientation.position0));
            var rightRook = BoardService.BoardBlocks.GetElement(new Position(verticalPosition, HorizontalOrientation.position7));
            if (KingBlock.Figure == null)
                return default;

            if (((King)KingBlock.Figure).IsMoved)
                return default;

            for(int index = (int)HorizontalOrientation.position0 +1;index < (int)HorizontalOrientation.position5; index++)
            {

                var Blocks = BoardService.BoardBlocks.GetElement(new Position(verticalPosition, (HorizontalOrientation)index));
                if (Blocks.Figure != null)
                    break;

                FindAndAddEmptyBlocksIntoCollectionForCastling(ListOfBoardBlocks, HorizontalOrientation.position4 - 2, verticalPosition, KingBlock, leftRook);

            }
            for (int index = (int)HorizontalOrientation.position4 + 1; index < (int)HorizontalOrientation.position7; index++)
            {
                var Blocks = BoardService.BoardBlocks.GetElement(new Position(verticalPosition, (HorizontalOrientation)index));
                if (Blocks.Figure != null )
                    break;

                FindAndAddEmptyBlocksIntoCollectionForCastling(ListOfBoardBlocks, HorizontalOrientation.position4 + 2, verticalPosition, KingBlock, rightRook);
            }

            return ListOfBoardBlocks;
        }
        public void FindAndAddEmptyBlocksIntoCollectionForCastling(List<BoardBlock[]> listOfBoardBlocks,HorizontalOrientation emptyBoardHorizontalPosition,VerticalOrientation emptyBoardVerticalPosition,BoardBlock kingBoardBlock,BoardBlock rookBoardBlock)
        {
            listOfBoardBlocks.Add(new BoardBlock[2] { kingBoardBlock, rookBoardBlock });
            var AnimateBlock = BoardService.BoardBlocks.GetElement(new Position(emptyBoardVerticalPosition, (HorizontalOrientation)emptyBoardHorizontalPosition));
            _animationService.AnimateCell(AnimateBlock, BoardBlock.CASTLING_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
            BoardService.BoardPaintedToMoveBlocks.Add(AnimateBlock);
        }

        public void CastlingLogic(List<BoardBlock[]> listOfBoardBlocks, BoardBlock clickedBoardBlock, Grid boardGrid)
        {
            if (listOfBoardBlocks.Count == 0)
                return;
            foreach (var BoardBlocks in listOfBoardBlocks)
            {
                var RookHorizontalEmptyBoardPosition = (Math.Abs(BoardBlocks[0].Position.GetHorizontalOrientation() - BoardBlocks[1].Position.GetHorizontalOrientation()) == 4 ? 3 : 5);
                var KingHorizontalEmptyBoardPosition = (Math.Abs(BoardBlocks[0].Position.GetHorizontalOrientation() - BoardBlocks[1].Position.GetHorizontalOrientation()) == 4 ? 2 : 6);
                var EventBoardBlock = BoardService.BoardBlocks.GetElement(new Position(BoardBlocks[0].Position.GetVerticalOrientation(), (HorizontalOrientation)KingHorizontalEmptyBoardPosition));

                if (EventBoardBlock == clickedBoardBlock)
                {
                    var RookEmptyBoard = BoardService.BoardBlocks.GetElement(new Position(EventBoardBlock.Position.GetVerticalOrientation(), (HorizontalOrientation)RookHorizontalEmptyBoardPosition));
                    VirtualFigureMoving(EventBoardBlock, BoardBlocks[0], boardGrid);
                    VirtualFigureMoving(RookEmptyBoard, BoardBlocks[1], boardGrid);
                    BoardService.BeengCastleAndEmptyBlocks.Add(new BoardBlock[] { BoardBlocks[0], EventBoardBlock });
                    BoardService.BeengCastleAndEmptyBlocks.Add(new BoardBlock[] { BoardBlocks[1], RookEmptyBoard });

                }
            }
            BoardService.TurnSwitch();
        }

        public void GameBegin(Grid boardGrid)
        {
            BoardService.BoardBlocks.Clear();
            boardGrid.Children.Clear();
            boardGrid.RowDefinitions.Clear();
            boardGrid.ColumnDefinitions.Clear();
            BoardService.Turn = Turn.White;
            IBoardBlockService boardBlockService = new BoardBlockService(_animationService, this);
            IBoardService boardService = new BoardService(boardBlockService);
            boardService.BoardInitialize(boardGrid, BOARD_SIZE);
        }
    }
}
