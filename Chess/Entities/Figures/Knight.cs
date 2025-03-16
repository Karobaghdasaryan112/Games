using Chess.Enums;
using Chess.Interfaces;

namespace Chess.Entities.Figures
{
    public class Knight : FigureBase<Knight>,IFigure
    {
        public Knight(Color color) : base(color)
        {

        }
        public override string GetFigureName()
        {
            return typeof(Knight).Name;
        }

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation,Color color)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            int row = (int)verticalOrientation;
            int col = (int)horizontalOrientation;

            if (row + 2 < IBoardService.BOARD_SIZE && col + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row + 2, col + 1);

            if (row + 2 < IBoardService.BOARD_SIZE && col - 1 >= 0)
                MoveCondition(row + 2, col - 1);

            if (row - 2 >= 0 && col + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row - 2, col + 1);

            if (row - 2 >= 0 && col - 1 >= 0)
                MoveCondition(row - 2, col - 1);

            if (col + 2 < IBoardService.BOARD_SIZE && row + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row + 1, col + 2);

            if (col + 2 < IBoardService.BOARD_SIZE && row - 1 >= 0)
                MoveCondition(row - 1, col + 2);

            if (col - 2 >= 0 && row + 1 < IBoardService.BOARD_SIZE)
                MoveCondition(row + 1, col - 2);

            if (col - 2 >= 0 && row - 1 >= 0)
                MoveCondition(row - 1, col - 2);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
