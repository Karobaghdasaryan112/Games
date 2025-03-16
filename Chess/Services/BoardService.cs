using Chess.Entities.Figures;
using Chess.Entities;
using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chess.Services
{
    public class BoardService : IBoardService
    {
        public static List<BoardBlock> BoardBlocks = new List<BoardBlock>();
        public static List<BoardBlock> BoardPaintedToMoveBlocks = new List<BoardBlock>();
        public static BoardBlock[] FigureAndMoveBlocks = new BoardBlock[2];
        public static bool IsChecked = false;
        public static bool WrongMoveIfKingIsChecked = false;
        public static HashSet<BoardBlock> AttackedFigureOnKing = new HashSet<BoardBlock>();
        public static Turn Turn;
        private readonly IBoardBlockService _boardBlockService;
        public static List<BoardBlock> TryingToUnCheckedBoardBlocks = new List<BoardBlock>();

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
                    bool isWhite = (row + col) % 2 == 0;
                    SolidColorBrush BoardnormalColor = isWhite ? BoardBlock.WHITE_COLOR : BoardBlock.BLACK_COLOR;

                    var FigureColor = (row == 0 || row == 1) ? Enums.Color.White :
                                      (row == 7 || row == 6) ? Enums.Color.Black : Enums.Color.None;

                    IFigure newFigure = null;

                    if (row == 1 || row == 6)
                        newFigure = new Pawn(FigureColor);
                    else if (row == 0 || row == 7)
                    {
                        if (col == 0 || col == boardSize - 1)
                            newFigure = new Rook(FigureColor);
                        if (col == 1 || col == boardSize - 2)
                            newFigure = new Knight(FigureColor);
                        if (col == 2 || col == boardSize - 3)
                            newFigure = new Bishop(FigureColor);
                        if (col == 3)
                            newFigure = new Queen(FigureColor);
                        if (col == 4)
                            newFigure = new King(FigureColor);
                    }

                    var NewBoardBlock = _boardBlockService.SetBoardBlockOnBoard<IFigure>(
                        BoardnormalColor,
                        newFigure,
                        new Position((VerticalOrientation)row, (HorizontalOrientation)col),
                        BoardGrid);

                    BoardBlocks.Add(NewBoardBlock);
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
    }
}
