using Chess.Enums;
using Chess.Extentions;
using Chess.Interfaces;
using Chess.Services;

namespace Chess.Entities.Figures
{
    public class Pawn : FigureBase<Pawn>, IFigure
    {
        public Pawn(Color color) : base(color)
        {
        }
        public override string GetFigureName()
        {
            return typeof(Pawn).Name;
        }

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, Color color)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            int row = (int)verticalOrientation;
            int col = (int)horizontalOrientation;
            int direction = (color == Color.White) ? 1 : -1; 

            bool isStartingRow = (color == Color.White && row == (int)VerticalOrientation.b) ||
                                 (color == Color.Black && row == (int)VerticalOrientation.g);

            if (MoveCondition(row + direction, col))
            {
                if (isStartingRow)
                    MoveCondition(row + 2 * direction, col);
            }
            CutableRectangles.Clear();
            if (col + 1 < IBoardService.BOARD_SIZE)
                TryToGiveCuttingFigure(row + direction, col + 1);

            if (col - 1 >= 0)
                TryToGiveCuttingFigure(row + direction, col - 1);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }

        private void TryToGiveCuttingFigure(int row, int col)
        {

            var TryingToCutFIgure = BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row, (HorizontalOrientation)col));

            if (TryingToCutFIgure.Figure != null)
            {
                if (TryingToCutFIgure?.Figure?.GetColor() != this.GetColor())
                {
                    CutableRectangles.Add(TryingToCutFIgure);
                }
            }
        }
    }
}
