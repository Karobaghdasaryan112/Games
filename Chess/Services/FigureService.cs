using Chess.Entities;
using Chess.Entities.Figures;
using Chess.Enums;
using Chess.Extentions;
using Chess.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

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
        public static bool IsPawnCastling = false;
        public IAnimationService _animationService;
        private List<IFigure> AllBoardBlocksOfFigure = new List<IFigure>();
        public static Grid? CastlingGrid;
        public static Grid? boardGrid;
        public static BoardBlock? pawnBoardBlock;
        public static BoardBlock? emptyOrCuttedBoardBlock;
        public static Color castlingFigureColor;

        public FigureService(IAnimationService animationService)
        {
            _animationService = animationService;
            AllBoardBlocksOfFigure.Add(new Pawn(Color.White));
            AllBoardBlocksOfFigure.Add(new Pawn(Color.Black));
            AllBoardBlocksOfFigure.Add(new Rook(Color.White));
            AllBoardBlocksOfFigure.Add(new Rook(Color.Black));
            AllBoardBlocksOfFigure.Add(new Bishop(Color.White));
            AllBoardBlocksOfFigure.Add(new Bishop(Color.Black));
            AllBoardBlocksOfFigure.Add(new Knight(Color.White));
            AllBoardBlocksOfFigure.Add(new Knight(Color.Black));
            AllBoardBlocksOfFigure.Add(new Queen(Color.White));
            AllBoardBlocksOfFigure.Add(new Queen(Color.Black));

            foreach (var item in AllBoardBlocksOfFigure)
                item.Initialize();
        }


        public async Task FigureCut(BoardBlock cuttingBoardblock, BoardBlock cuttedBoardblock, Grid boardGrid)
        {
            if (cuttedBoardblock?.Figure == default || cuttingBoardblock?.Figure == default)
                return;

            if (cuttedBoardblock.RectangleForAnimation.Fill.ToString() == BoardBlock.CUT_COLOR.ToString())
            {
                ReInitializeFiguresAndEmptiBoardBlocks(cuttingBoardblock, cuttedBoardblock, default, default);

                await PawnCastling(cuttedBoardblock, cuttingBoardblock, boardGrid);

                CheckToMovableInRookAndKing(cuttedBoardblock);
                await _animationService.MovableBlocksAnimationDisable();
                BoardService.TurnSwitch();
            }
        }

        public async Task<bool> FigureMove(BoardBlock figureBoardBlock, BoardBlock moveBoardBlock, Grid boardGrid)
        {
            if (figureBoardBlock == null || moveBoardBlock == null)
                return false;

            if (figureBoardBlock.Figure == null)
                return false;

            if (moveBoardBlock.Figure != null)
                return false;

            if (BoardService.BoardPaintedToMoveBlocks.Contains(moveBoardBlock) && moveBoardBlock.RectangleForAnimation.Fill == BoardBlock.EVENT_COLOR)
            {
                ReInitializeFiguresAndEmptiBoardBlocks(figureBoardBlock, moveBoardBlock, default, default);
                await PawnCastling(moveBoardBlock, figureBoardBlock, boardGrid);
                CheckToMovableInRookAndKing(moveBoardBlock);
                await _animationService.MovableBlocksAnimationDisable();
                BoardService.TurnSwitch();
                return true;
            }
            return false;
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
            var WhitekingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", Enums.Color.White);
            var BlackkingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", Enums.Color.Black);

            if (WhitekingBlock != null)
            {
                var verticalOrientationOfWhiteKingBlock = WhitekingBlock.Position.GetVerticalOrientation();
                var horizontalOrientationOfWhiteKingBlock = WhitekingBlock.Position.GetHorizontalOrientation();

                await CheckedLogic(Color.White, WhitekingBlock, verticalOrientationOfWhiteKingBlock, horizontalOrientationOfWhiteKingBlock);

                if (BoardService.IsChecked && !((King)BlackkingBlock?.Figure).IsChecked)
                    return true;
            }
            if (BlackkingBlock != null)
            {
                var verticalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetVerticalOrientation();
                var horizontalOrientationOfBlackKingBlock = BlackkingBlock.Position.GetHorizontalOrientation();

                await CheckedLogic(Color.Black, BlackkingBlock, verticalOrientationOfBlackKingBlock, horizontalOrientationOfBlackKingBlock);

                if (BoardService.IsChecked && !((King)WhitekingBlock?.Figure).IsChecked)
                    return true;
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
                    BoardService.AttackedFigureOnKing.Add(block);
            }
            return AttackedFigureBoardBlocks.Any();
        }

        private async Task CheckedLogic(Enums.Color color, BoardBlock KingBoardBlock, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
           
            BoardService.AttackedFigureOnKing.Clear();

            if (
                IsAttackedBy(new King(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { KING }) ||
               IsAttackedBy(new Pawn(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { PAWN }) ||
               IsAttackedBy(new Rook(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { ROOK, QUEEN }) ||
               IsAttackedBy(new Bishop(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { BISHOP, QUEEN }) ||
               IsAttackedBy(new Knight(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { KNIGHT }) ||
               IsAttackedBy(new Queen(color), KingBoardBlock, verticalOrientation, horizontalOrientation, new List<string> { QUEEN }))
            {
                var KingBlock = BoardService.AttackedFigureOnKing.Where(board => board.Figure.GetType() == typeof(King)).FirstOrDefault();
                if (BoardService.Turn.ToString() == KingBoardBlock.Figure.GetColor().ToString() && KingBlock == null)
                {
                    BoardService.WrongMoveIfKingIsChecked = false;
                    ((King)KingBoardBlock.Figure).IsChecked = true;
                    BoardService.BoardPaintedToMoveBlocks.Add(KingBoardBlock);

                    await _animationService.AnimateCell(KingBoardBlock, BoardBlock.CHECKED_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                }
                else
                {
                    BoardService.WrongMoveIfKingIsChecked = true;

                    if (KingBlock != null && !((King)KingBoardBlock.Figure).IsChecked)
                        BoardService.WrongCheckedKing = true;
                    else
                        BoardService.WrongCheckedKing = false;
                }
                BoardService.IsChecked = true;
            }
            else
            {
                ((King)KingBoardBlock.Figure).IsChecked = false;
                BoardService.IsChecked = false;
                await _animationService.AnimateCell(KingBoardBlock, KingBoardBlock.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
            }
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
                secondBlockOnBoard.Figure.SetImage(secondBlockOnBoard.FigureImage);

                firstBlockOnBoard.Figure = firstBoardBlock.Figure;
                firstBlockOnBoard.FigureImage = firstBoardBlock.FigureImage;
                firstBlockOnBoard.Figure.SetImage(firstBoardBlock.FigureImage);
                firstBlockOnBoard.SetFigureImageIntoRectangle(firstBoardBlock.FigureImage);
                firstBlockOnBoard.Figure.SetImage(firstBlockOnBoard.FigureImage);

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
            var FigureColor = BoardService.Turn == Turn.White ? Enums.Color.White : Enums.Color.Black;
            var CheckedKingBlock = BoardService.BoardBlocks.GetBoardBlockWithFigureName("King", FigureColor);
            if (BoardService.WrongMoveIfKingIsChecked && BoardService.IsChecked)
            {
                await _animationService.AnimateCell(CheckedKingBlock, BoardBlock.CHECKED_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                return;
            }
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
                    await _animationService.AnimateCell(CheckedKingBlock, BoardBlock.CHECKED_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                    return;
                }
                BoardService.IsMateState = true;
                MessageBox.Show("Mate");
                GameBegin(boardGrid);
            }
            await _animationService.AnimateCell(CheckedKingBlock, CheckedKingBlock.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
        }

        private async Task<bool> TryingToUncheckedKing(Grid boardGrid, List<BoardBlock> tryingBoardBlocks, Enums.Color figureColor)
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
                    //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                    await FigureMove(block1, block2, boardGrid);
                    //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                    BoardService.BoardPaintedToMoveBlocks.Clear();

                    if (!await IsKingCheckedCondition())
                    {
                        //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                        VirtualFigureMoving(block1, block2, boardGrid);
                        //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                        return true;
                    }
                    if (block2 != null)
                    {
                        //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                        VirtualFigureMoving(block1, block2, boardGrid);
                        //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                    }
                    CutAndMoveBlocks = tryingBoardBlocks[baseIndex].Figure.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);
                    MoveBlocks = CutAndMoveBlocks[0];
                }

                CutAndMoveBlocks = tryingBoardBlocks[baseIndex].Figure.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);
                CutBlocks = CutAndMoveBlocks[1];
                MoveBlocks = CutAndMoveBlocks[0];
                for (int index = 0; index < CutBlocks.Count; index++)
                {
                    var block1 = BoardService.BoardBlocks.Where(B => B == tryingBoardBlocks[baseIndex]).FirstOrDefault();
                    var block2 = BoardService.BoardBlocks.Where(B => B == CutBlocks[index]).FirstOrDefault();

                    var newCuttedBlock = (BoardBlock)((ICloneable)block2).Clone();
                    block2.RectangleForAnimation.Fill = BoardBlock.CUT_COLOR;
                    await FigureCut(block1, block2, boardGrid);
                    if (!await IsKingCheckedCondition())
                    {
                        var MoveBlocksAgain = CutAndMoveBlocks[0];
                        var CutBlocksAgain = CutAndMoveBlocks[1];
                        VirtualFigureCuting(block1, block2, newCuttedBlock, boardGrid);
                        return true;
                    }
                    //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                    VirtualFigureCuting(block1, block2, newCuttedBlock, boardGrid);
                    //await SimulationforVirtualCuttingAndMovingFigures(block2, 300);
                    CutAndMoveBlocks = tryingBoardBlocks[baseIndex].Figure.MovableBlocks(tryingBoardBlocks[baseIndex].Position.GetVerticalOrientation(), tryingBoardBlocks[baseIndex].Position.GetHorizontalOrientation(), figureColor);
                    CutBlocks = CutAndMoveBlocks[1];
                }
            }
            return false;
        }
        public async void VirtualFigureMoving(BoardBlock block1, BoardBlock block2, Grid boardGrid)
        {
            BoardService.TryingToUnCheckedBoardBlocks.Add(block1);
            BoardService.TryingToUnCheckedBoardBlocks.Add(block2);
            block1.RectangleForAnimation.Fill = BoardBlock.EVENT_COLOR;
            BoardService.BoardPaintedToMoveBlocks.Add(block1);
            await FigureMove(block2, block1, boardGrid);
            BoardService.BoardPaintedToMoveBlocks.Clear();
            block1.RectangleForAnimation.Fill = block1.ActualColor;
        }
        public async void VirtualFigureCuting(BoardBlock block1, BoardBlock block2, BoardBlock block2Clone, Grid boardGrid)
        {
            BoardService.TryingToUnCheckedBoardBlocks.Add(block1);
            block1.RectangleForAnimation.Fill = BoardBlock.EVENT_COLOR;
            BoardService.BoardPaintedToMoveBlocks.Add(block1);
            await FigureMove(block2, block1, boardGrid);
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

            if (KingBlock.Figure is not King king)
                return default;

            if (KingBlock.Figure == null)
                return default;

            if (((King)KingBlock.Figure).IsMoved)
                return default;

            for (int index = (int)HorizontalOrientation.position0 + 1; index < (int)HorizontalOrientation.position4; index++)
            {
                var Blocks = BoardService.BoardBlocks.GetElement(new Position(verticalPosition, (HorizontalOrientation)index));
                if (Blocks.Figure != null)
                    break;
                if (index == (int)HorizontalOrientation.position3)
                    FindAndAddEmptyBlocksIntoCollectionForCastling(ListOfBoardBlocks, HorizontalOrientation.position4 - 2, verticalPosition, KingBlock, leftRook);
            }
            for (int index = (int)HorizontalOrientation.position4 + 1; index < (int)HorizontalOrientation.position7; index++)
            {
                var Blocks = BoardService.BoardBlocks.GetElement(new Position(verticalPosition, (HorizontalOrientation)index));
                if (Blocks.Figure != null)
                    break;
                if (index == (int)HorizontalOrientation.position6)
                    FindAndAddEmptyBlocksIntoCollectionForCastling(ListOfBoardBlocks, HorizontalOrientation.position4 + 2, verticalPosition, KingBlock, rightRook);
            }
            return ListOfBoardBlocks;
        }
        public void FindAndAddEmptyBlocksIntoCollectionForCastling(List<BoardBlock[]> listOfBoardBlocks, HorizontalOrientation emptyBoardHorizontalPosition, VerticalOrientation emptyBoardVerticalPosition, BoardBlock kingBoardBlock, BoardBlock rookBoardBlock)
        {
            listOfBoardBlocks.Add(new BoardBlock[2] { kingBoardBlock, rookBoardBlock });
            var AnimateBlock = BoardService.BoardBlocks.GetElement(new Position(emptyBoardVerticalPosition, (HorizontalOrientation)emptyBoardHorizontalPosition));
            _animationService.AnimateCell(AnimateBlock, BoardBlock.CASTLING_COLOR, BoardBlock.MOUSE_ENTER_OPACITY, BoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
            BoardService.BoardPaintedToMoveBlocks.Add(AnimateBlock);
        }
        public void CastlingLogic(List<BoardBlock[]> listOfBoardBlocks, BoardBlock clickedBoardBlock, Grid boardGrid)
        {
            if (listOfBoardBlocks != null)
            {
                if (listOfBoardBlocks.Count == 0)
                    return;
                foreach (var BoardBlocks in listOfBoardBlocks)
                {
                    var RookHorizontalEmptyBoardPosition = (Math.Abs(BoardBlocks[0].Position.GetHorizontalOrientation() - BoardBlocks[1].Position.GetHorizontalOrientation()) == 4 ? 3 : 5);
                    var KingHorizontalEmptyBoardPosition = (Math.Abs(BoardBlocks[0].Position.GetHorizontalOrientation() - BoardBlocks[1].Position.GetHorizontalOrientation()) == 4 ? 2 : 6);
                    var EventBoardBlock = BoardService.BoardBlocks.GetElement(new Position(BoardBlocks[0].Position.GetVerticalOrientation(), (HorizontalOrientation)KingHorizontalEmptyBoardPosition));
                    if (EventBoardBlock == clickedBoardBlock || BoardService.IsChecked)
                    {
                        var RookEmptyBoard = BoardService.BoardBlocks.GetElement(new Position(EventBoardBlock.Position.GetVerticalOrientation(), (HorizontalOrientation)RookHorizontalEmptyBoardPosition));
                        VirtualFigureMoving(EventBoardBlock, BoardBlocks[0], boardGrid);
                        VirtualFigureMoving(RookEmptyBoard, BoardBlocks[1], boardGrid);
                        BoardService.BeengCastleAndEmptyBlocks.Add(new BoardBlock[] { BoardBlocks[0], clickedBoardBlock });
                        BoardService.BeengCastleAndEmptyBlocks.Add(new BoardBlock[] { BoardBlocks[1], RookEmptyBoard });
                    }
                }
                BoardService.TurnSwitch();
            }
        }
        public void GameBegin(Grid boardGrid)
        {
            BoardService.BoardBlocks?.Clear();
            BoardService.BoardPaintedToMoveBlocks?.Clear();
            BoardService.KingAndRookBlocks?.Clear();
            BoardService.BeengCastleAndEmptyBlocks?.Clear();
            BoardService.FigureAndMoveBlocks[0] = null;
            BoardService.FigureAndMoveBlocks[1] = null;
            BoardService.IsChecked = false;
            BoardService.IsMateState = false;
            BoardService.AttackedFigureOnKing?.Clear();
            BoardService.Turn = Turn.White;
            BoardService.TryingToUnCheckedBoardBlocks?.Clear();

            boardGrid.Children.Clear();
            boardGrid.RowDefinitions.Clear();
            boardGrid.ColumnDefinitions.Clear();
            BoardService.Turn = Turn.White;
            IBoardBlockService boardBlockService = new BoardBlockService(_animationService, this);
            IBoardService boardService = new BoardService(boardBlockService);
            boardService.BoardInitialize(boardGrid, BOARD_SIZE);
        }
        //private async Task SimulationforVirtualCuttingAndMovingFigures(BoardBlock block, double dimmedOpacity)
        //{
        //    if (block == null) return;
        //    double initialOpacity = block.RectangleForAnimation.Opacity;
        //    await _animationService.AnimateOpacity(block, dimmedOpacity, 300);
        //    await Task.Delay(200);
        //    await _animationService.AnimateOpacity(block, initialOpacity, 300);
        //}

        public async Task PawnCastling(BoardBlock pawnBoardBlock, BoardBlock emptyOrCuttedBoardBlock, Grid boardGrid)
        {
            if (pawnBoardBlock == null || emptyOrCuttedBoardBlock == null)
                return;

            if (pawnBoardBlock.Figure?.GetType() != typeof(Pawn))
                return;

            var Position = pawnBoardBlock.Position;

            if ((Position.GetVerticalOrientation() == VerticalOrientation.h && pawnBoardBlock.Figure.GetColor() == Enums.Color.White)
                || (Position.GetVerticalOrientation() == VerticalOrientation.a && pawnBoardBlock.Figure.GetColor() == Enums.Color.Black))
            {
                var Main = MainWindow.GetWindow(boardGrid);
                FigureService.CastlingGrid = Main.FindName("CastlingGrid") as Grid;
                FigureService.castlingFigureColor = pawnBoardBlock.Figure.GetColor();

                FigureService.CastlingGrid.Visibility = Visibility.Visible;
                boardGrid.Opacity = 0.5;
                boardGrid.IsHitTestVisible = false;
                FigureService.boardGrid = boardGrid;
                FigureService.pawnBoardBlock = pawnBoardBlock;
                FigureService.emptyOrCuttedBoardBlock = emptyOrCuttedBoardBlock;

                var buttons = CastlingGrid.Children
                    .OfType<Border>()
                    .FirstOrDefault()?
                    .Child as StackPanel;
            }
        }

        public async Task ClickHandler(object sender, RoutedEventArgs e, Grid CastlingGrid, Grid boardGrid, BoardBlock pawnBoardBlock, BoardBlock emptyOrCuttedBoardBlock, Color FigureColor)
        {
            IFigure CastlingFigure = default;

            var Button = sender as Button;

            if (Button != null)
            {
                foreach (var Figure in AllBoardBlocksOfFigure)
                {
                    if (Button.Content.ToString() == Figure.GetType().Name && Figure.GetColor() == FigureColor)
                    {
                        CastlingFigure = (IFigure)Activator.CreateInstance(Figure.GetType(), FigureColor);
                        CastlingFigure.Initialize();
                    }
                }

                if (CastlingFigure == null)
                    return;

                var realPawnBoardCastle = BoardService.BoardBlocks.GetElement(pawnBoardBlock.Position);

                var newBoardblock = (ICloneable)realPawnBoardCastle.Clone();

                Grid cellContainer = new Grid();
                Rectangle RectangleForAnimation = new Rectangle()
                {
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 1.0,
                    Opacity = 1.0,
                    Fill = ((BoardBlock)newBoardblock).ActualColor
                };

                BoardBlock newBoardBlockWithFigure = new BoardBlock()
                {
                    RectangleGrid = cellContainer,
                    RectangleForAnimation = RectangleForAnimation,
                    ActualColor = ((BoardBlock)newBoardblock).ActualColor,
                    Position = new Position(((BoardBlock)newBoardblock).Position.GetVerticalOrientation(), ((BoardBlock)newBoardblock).Position.GetHorizontalOrientation()),
                    Figure = CastlingFigure,
                    FigureImage = CastlingFigure.GetImage()
                };

                var existingCell = boardGrid.Children
                    .OfType<Grid>()
                    .FirstOrDefault(g => Grid.GetRow(g) == (int)((BoardBlock)newBoardblock).Position.GetVerticalOrientation() &&
                                         Grid.GetColumn(g) == (int)((BoardBlock)newBoardblock).Position.GetHorizontalOrientation());

                if (existingCell != null)
                    boardGrid.Children.Remove(existingCell);

                if (((BoardBlock)newBoardblock).Figure?.GetImage() != null)
                {
                    if (!cellContainer.Children.OfType<Image>().Any())
                    {
                        var newImage = new Image() { Source = CastlingFigure.GetImage().Source };
                        Panel.SetZIndex(newImage, 1);
                        cellContainer.Children.Add(newImage);
                    }
                }
                else
                {
                    if (!cellContainer.Children.OfType<Image>().Any())
                        cellContainer.Children.Add(new Image());
                }

                cellContainer.Children.Add(RectangleForAnimation);

                Panel.SetZIndex(RectangleForAnimation, 0);

                Grid.SetRow(cellContainer, (int)((BoardBlock)newBoardblock).Position.GetVerticalOrientation());
                Grid.SetColumn(cellContainer, (int)((BoardBlock)newBoardblock).Position.GetHorizontalOrientation());

                boardGrid.Children.Add(cellContainer);

                BoardService.BoardBlocks.Remove(realPawnBoardCastle);
                BoardService.BoardBlocks.Add(newBoardBlockWithFigure);

                IFigureService figureService = new FigureService(_animationService);
                BoardBlockService boardBlockService = new BoardBlockService(_animationService, figureService);

                newBoardBlockWithFigure.RectangleForAnimation.MouseLeftButtonUp += async (s, e) => { e.Handled = true; await boardBlockService.SetEmptyBoardAnimations(newBoardBlockWithFigure, boardGrid); };

                if (newBoardBlockWithFigure.Figure != null)
                {
                    cellContainer.Children.OfType<Image>().First().MouseLeftButtonUp += async (s, e) =>
                    {
                        e.Handled = true;
                        await boardBlockService.SetFigureAnimations(
                            boardGrid,
                            newBoardBlockWithFigure.Position.GetVerticalOrientation(),
                            newBoardBlockWithFigure.Position.GetHorizontalOrientation(),
                            newBoardBlockWithFigure.Figure,
                            newBoardBlockWithFigure.RectangleGrid,
                            newBoardBlockWithFigure);
                    };


                    cellContainer.Children.OfType<Image>().First().MouseEnter += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseEnterEvent });
                    cellContainer.Children.OfType<Image>().First().MouseLeave += (s, e) => newBoardBlockWithFigure.RectangleForAnimation?.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseLeaveEvent });
                }
                else
                    newBoardBlockWithFigure.RectangleForAnimation.MouseLeftButtonUp += async (s, e) => { e.Handled = true; await boardBlockService.SetEmptyBoardAnimations(newBoardBlockWithFigure, boardGrid); };
                
                boardBlockService.SetMouseLeaveAndEnterAnimations(newBoardBlockWithFigure);
                await boardBlockService.IsKingCheckedForeMove(boardGrid, newBoardBlockWithFigure, emptyOrCuttedBoardBlock);
                CastlingGrid.Visibility = Visibility.Hidden;
                boardGrid.IsHitTestVisible = true;
                boardGrid.Opacity = 1;
            }
        }
    }
}