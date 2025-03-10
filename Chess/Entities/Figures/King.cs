using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;

namespace Chess.Entities.Figures
{
    public class King : FigureBase<King>,IFigure
    {
        public King(Enums.Color color) : base(color)
        {
        }

        public List<BoardBlock>[] MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            int row = (int)verticalOrientation;
            int  col = (int)horizontalOrientation;

            if (col + 1 < IBoardService.BOARD_SIZE && row + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row + 1, col + 1);

            if (col - 1 >= 0 && row - 1 >= 0)
                MoveCondition(row + 1, col - 1);

            if (col - 1 >= 0 && row + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row + 1, col - 1);

            if (col + 1 < IBoardService.BOARD_SIZE && row - 1 >= 0)
                MoveCondition(row - 1, col + 1);

            if (col + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row, col + 1);

            if (row + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row + 1, col);

            if (col - 1 >= 0)
                MoveCondition(row, col - 1);

            if (col + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row, col + 1);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
