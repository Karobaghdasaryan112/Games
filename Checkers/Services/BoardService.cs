using Checkers.Entities;
using Checkers.Entities.Figures;
using Checkers.Enums;
using Checkers.Interfaces;
using System.Windows.Controls;
namespace Checkers.Services
{
    public class BoardService : IBoardService
    {
        public static Turn Turn = Turn.White;
        public const int BOARD_SIZE = 8;
        public static List<CheckersBoardBlock> BoardBlocks = new List<CheckersBoardBlock>();
        public static List<CheckersBoardBlock[]> CutableBoardBlocks = new List<CheckersBoardBlock[]>();
        //Array[0]  Cutted Figure
        //Array[1] Movable Block
        public static CheckersBoardBlock Clicked_Block { get; set; }   
        public static List<CheckersBoardBlock> BaseCutableBlocks = new List<CheckersBoardBlock>();
        public static List<CheckersBoardBlock> MovableBoardBlocks = new List<CheckersBoardBlock>();
        public static List<CheckersBoardBlock> BoardPaintedToMove = new List<CheckersBoardBlock>();
        public static List<CheckersBoardBlock> BoardPaintedToCut = new List<CheckersBoardBlock>();
        public static CheckersBoardBlock PreviusCheckersBoardBlock;
        public static CheckersBoardBlock CurrentBoardBlock;
        public static CheckersBoardBlock CuttedBoardBlock;

        private readonly IBoardBlockService _boardBlockService;

        public BoardService(IBoardBlockService boardBlockService)
        {
            _boardBlockService = boardBlockService;
        }

        public void BoardInitialize(Grid BoardGrid, int boardSize)
        {
            for (int i = 0; i < boardSize; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition());
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    CheckersBoardBlock checkersBoardBlock = new CheckersBoardBlock();

                    checkersBoardBlock.ActualColor = (row + col) % 2 == 0 ? CheckersBoardBlock.WHITE_COLOR : CheckersBoardBlock.BLACK_COLOR;

                    checkersBoardBlock.Position = new Position((VerticalOrientation)row, (HorizontalOrientation)col);

                    if (checkersBoardBlock.ActualColor == CheckersBoardBlock.BLACK_COLOR)
                    {
                        if (row == 5 || row == 6 || row == 7)
                        {
                            checkersBoardBlock.Figure = new Figure();
                            checkersBoardBlock.Figure.Initialize(Color.White);
                        }
                        if (row == 0 || row == 1 || row == 2)
                        {
                            checkersBoardBlock.Figure = new Figure();
                            checkersBoardBlock.Figure.Initialize(Color.Black);
                        }
                    }

                    _boardBlockService.SetBoardBlockIntoBoard(BoardGrid, checkersBoardBlock);

                }
            }
        }

        public static void TurnSwitch()
        {
            if (Turn == Turn.White)
                Turn = Turn.Black;
            else
                Turn = Turn.White;
        }
        public static bool IsMyTurn(FigureBase figure)
        {
            return figure.GetColor().ToString() == BoardService.Turn.ToString();
        }
    }
}

