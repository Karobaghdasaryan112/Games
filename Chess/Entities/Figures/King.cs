using Chess.Enums;
using Chess.Interfaces;

namespace Chess.Entities.Figures
{
    public class King : FigureBase<King>,IFigure
    {
        public bool IsChecked = false;
        public bool IsMoved = false;
        public King(Color color) : base(color)
        {
        }

        public override string GetFigureName()
        {
            return typeof(King).Name;
        }

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation,Color color)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            int row = (int)verticalOrientation;
            int col = (int)horizontalOrientation;

            if (col + 1 < IBoardService.BOARD_SIZE && row + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row + 1, col + 1);

            if (col - 1 >= 0 && row - 1 >= 0)
                MoveCondition(row - 1, col - 1);

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

            if (row - 1 >= 0)
                MoveCondition(row - 1, col);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
