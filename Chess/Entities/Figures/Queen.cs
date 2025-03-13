using Chess.Enums;
using Chess.Interfaces;
using Chess.Services;
using System.Windows.Controls;

namespace Chess.Entities.Figures
{
    public class Queen : FigureBase<Queen>, IFigure
    {
        public Queen(Color color) : base(color)
        {
        }

        public override string GetFigureName()
        {
            return typeof(Queen).Name;
        }

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation,Color color)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            IFigure Rook = new Rook(color);
            IFigure Bishop = new Bishop(color);

            List<BoardBlock>[] BlocksforRook = new List<BoardBlock>[2];
            List<BoardBlock>[] BlocksforBishop = new List<BoardBlock>[2];

            BlocksforRook = Rook.MovableBlocks(verticalOrientation, horizontalOrientation, color);
            BlocksforBishop = Bishop.MovableBlocks(verticalOrientation, horizontalOrientation, color);

            MoveableRectangles.AddRange(BlocksforBishop[0]);
            CutableRectangles.AddRange(BlocksforBishop[1]);
            MoveableRectangles.AddRange(BlocksforRook[0]);
            CutableRectangles.AddRange(BlocksforRook[1]);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
