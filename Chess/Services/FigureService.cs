using Chess.Entities;
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
                _animationService.MovableBlocksAnimationDisable();
                BoardService.TurnSwitch();
            }

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

            var verticalOrientationOfWhiteKingBlock = WhitekingBlock.Position.GetVerticalOrientation();
            var horizontalOrientationOfWhiteKingBlock = WhitekingBlock.Position.GetHorizontalOrientation();

            var verticalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetVerticalOrientation();
            var horizontalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetHorizontalOrientation();

           await CheckedLogic(Color.Black, BlackkingBlock, verticalOrientationOfBlackKingBlock, horizontalOrientationOfBlackKingBlock);
            if (BoardService.IsChecked)
                return true;

           await CheckedLogic(Color.White, WhitekingBlock, verticalOrientationOfWhiteKingBlock, horizontalOrientationOfWhiteKingBlock);

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
               IsAttackedBy(new Pawn(color),KingBoardBlock,verticalOrientation,horizontalOrientation,new List<string> { PAWN }) ||
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
                    _animationService.AnimateCell(KingBoardBlock, BoardBlock.CHECKED_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                }
                else
                {
                    BoardService.WrongMoveIfKingIsChecked = true;

                }
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
            if (firstBoardBlock == null || secondBoardBlock == null)
                return;

            var firstBlockOnBoard = BoardService.BoardBlocks.GetBoardBlockWithPosition(firstBoardBlock.Position);
            var secondBlockOnBoard = BoardService.BoardBlocks.GetBoardBlockWithPosition(secondBoardBlock.Position);

            BoardBlock cuttedFigureBefore = (BoardBlock)((ICloneable)secondBoardBlock).Clone();
            BoardBlock cuttingFigureBefore = (BoardBlock)((ICloneable)firstBoardBlock).Clone();

            (firstBlockOnBoard.Figure, secondBlockOnBoard.Figure) =
            (cuttedFigureBefore.Figure, cuttingFigureBefore.Figure);

            (firstBlockOnBoard.FigureImage, secondBlockOnBoard.FigureImage) =
            (cuttedFigureBefore.FigureImage, cuttingFigureBefore.FigureImage);

            if (secondBlockOnBoard.Figure == null)
            {
                firstBlockOnBoard.SetFigureImageIntoRectangle(firstBlockOnBoard.FigureImage);
                secondBlockOnBoard.SetFigureImageIntoRectangle(secondBlockOnBoard.FigureImage);

                firstBlockOnBoard.Figure.SetImage(firstBlockOnBoard.FigureImage);
            }
            else
            {
                firstBoardBlock.SetFigureImageIntoRectangle(firstBoardBlock.FigureImage);
                secondBoardBlock.SetFigureImageIntoRectangle(secondBoardBlock.FigureImage);

                firstBoardBlock.Figure.SetImage(firstBoardBlock.FigureImage);
                secondBoardBlock.Figure.SetImage(secondBoardBlock.FigureImage);

            }
            _animationService.MovableBlocksAnimationDisable();

            BoardService.TurnSwitch();
        }
        public void SetImageEventIntoBoardBlockEvent(IFigure newFigure, BoardBlock newBoardBlockWithFigure)
        {
            if (newFigure == null)
                return;

            newFigure.GetImage().MouseEnter += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseEnterEvent });
            newFigure.GetImage().MouseLeave += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseLeaveEvent });
        }

        public async Task<bool> IsMateState(Grid boardGrid)
        {
            var FigureColor = BoardService.Turn == Turn.White ? Color.White : Color.Black;
            var CheckedKingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", FigureColor);

            if (!((King)CheckedKingBlock.Figure).IsChecked) 
                return false;


            var PawnsBlock = BoardService.BoardBlocks.GetAllSameFigures(PAWN, FigureColor).ToList();

            var RooksBlock = BoardService.BoardBlocks.GetAllSameFigures(ROOK, FigureColor).ToList();
            var KnightsBlock = BoardService.BoardBlocks.GetAllSameFigures(KNIGHT, FigureColor).ToList();
            var BishopsBlock = BoardService.BoardBlocks.GetAllSameFigures(BISHOP, FigureColor).ToList();
            var QueensBlock = BoardService.BoardBlocks.GetAllSameFigures(QUEEN, FigureColor).ToList();
            var KingsBlock = BoardService.BoardBlocks.GetAllSameFigures(KING, FigureColor).ToList();

            if (await TryingToUncheckedKing(boardGrid, PawnsBlock, FigureColor) &&
            await TryingToUncheckedKing(boardGrid, RooksBlock, FigureColor) &&
            await TryingToUncheckedKing(boardGrid, KnightsBlock, FigureColor) &&
            await TryingToUncheckedKing(boardGrid, BishopsBlock, FigureColor) &&
            await TryingToUncheckedKing(boardGrid, QueensBlock, FigureColor) &&
            await TryingToUncheckedKing(boardGrid, KingsBlock, FigureColor))
                return false;
            return true;

        }
        private async Task<bool> TryingToUncheckedKing(Grid boardGrid, List<BoardBlock> tryingBoardBlocks, Color figureColor)
        {
            for (int baseIndex = 0; baseIndex < tryingBoardBlocks.Count; baseIndex++)
            {
                var CutAndMoveBlocks = tryingBoardBlocks[baseIndex].Figure.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);

                var MoveBlocks = CutAndMoveBlocks[0];
                var CutBlocks = CutAndMoveBlocks[1];

                for (int index = 0; index < MoveBlocks.Count; index++)
                {
                    FigureMove(tryingBoardBlocks[baseIndex], MoveBlocks[index], boardGrid);

                    var BlackkingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", figureColor);
                    var verticalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetVerticalOrientation();
                    var horizontalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetHorizontalOrientation();

                    await CheckedLogic(figureColor, BlackkingBlock, verticalOrientationOfBlackKingBlock, horizontalOrientationOfBlackKingBlock);

                    var CutAndMoveBlocksAgain = tryingBoardBlocks[baseIndex].Figure?.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);
                    if (CutAndMoveBlocksAgain != null)
                    {
                        var MoveBlocksAgain = CutAndMoveBlocks[0];
                        var CutBlocksAgain = CutAndMoveBlocks[1];
                        if (!BoardService.IsChecked)
                            return true;

                        RoleBackAfterMoving(MoveBlocksAgain[index], tryingBoardBlocks[baseIndex], default);
                    }
                }

                for (int index = 0; index < CutBlocks.Count; index++)
                {
                    FigureCut(tryingBoardBlocks[baseIndex], CutBlocks[index], boardGrid);

                    var BlackkingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", figureColor);
                    var verticalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetVerticalOrientation();
                    var horizontalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetHorizontalOrientation();

                    await CheckedLogic(figureColor, BlackkingBlock, verticalOrientationOfBlackKingBlock, horizontalOrientationOfBlackKingBlock);

                    var CutAndMoveBlocksAgain = tryingBoardBlocks[baseIndex].Figure?.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);
                    if (CutAndMoveBlocksAgain != null)
                    {
                        var MoveBlocksAgain = CutAndMoveBlocks[0];
                        var CutBlocksAgain = CutAndMoveBlocks[1];
                        if (!BoardService.IsChecked)
                            return true;

                        RoleBackAfterMoving(MoveBlocksAgain[index], tryingBoardBlocks[baseIndex], default);

                    }
                }
            }
            await Task<bool>.CompletedTask;
            return false;
        }


    }
}
