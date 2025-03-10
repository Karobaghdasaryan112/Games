using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;

namespace Chess.Entities.Figures
{
    public class Pawn : FigureBase<Pawn>, IFigure
    {

        public Pawn(Enums.Color color) : base(color)
        {
        }

        public List<BoardBlock>[] MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            int row = (int)verticalOrientation; int col = (int)horizontalOrientation;

            if (GetColor() == Enums.Color.White)
            {
                if (row == (int)VerticalOrientation.b)
                {
                    if(MoveCondition(row + 1, col))
                    MoveCondition(row + 2, col);
                }
                else
                    if (row + 1 < IBoardService.BOARD_SIZE)
                    MoveCondition(row + 1, col);
            }
            if (GetColor() == Enums.Color.Black)
            {
                if (row == (int)VerticalOrientation.g)
                {
                    if(MoveCondition(row - 1, col))
                    MoveCondition(row - 2, col);
                }
                else
                    if (row + 1 >= 0)
                    MoveCondition(row - 1, col);
            }
            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
