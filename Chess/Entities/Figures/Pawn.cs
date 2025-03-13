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

        public override string GetFigureName()
        {
            return typeof(Pawn).Name;
        }

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation,Color color)
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
