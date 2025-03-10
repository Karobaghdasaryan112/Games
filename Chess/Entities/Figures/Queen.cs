using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;

namespace Chess.Entities.Figures
{
    public class Queen : FigureBase<Queen>, IFigure
    {
        public Queen(Color color) : base(color)
        {
        }

        public List<BoardBlock>[] MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            IFigure Rook = new Rook(Color.Black);
            IFigure Bishop = new Bishop(Color.Black);

            List<BoardBlock>[] BlocksforRook = new List<BoardBlock>[2];
            List<BoardBlock>[] BlocksforBishop = new List<BoardBlock>[2];

            BlocksforRook = Rook.MovableBlocks(boardGrid, verticalOrientation, horizontalOrientation);
            BlocksforBishop = Bishop.MovableBlocks(boardGrid, verticalOrientation, horizontalOrientation);

            MoveableRectangles.AddRange(BlocksforBishop[0]);
            CutableRectangles.AddRange(BlocksforBishop[1]);
            MoveableRectangles.AddRange(BlocksforRook[0]);
            CutableRectangles.AddRange(BlocksforRook[1]);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
