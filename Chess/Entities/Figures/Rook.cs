using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;

namespace Chess.Entities.Figures
{
    public class Rook : FigureBase<Rook>, IFigure
    {
        public Rook(Color color) : base(color)
        {
        }

        public List<BoardBlock>[] MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            var row = (int)verticalOrientation; var col = (int)horizontalOrientation;

            do
            {
                row++;
                if (row == IBoardService.BOARD_SIZE)
                    break;

                if (!MoveCondition(row, col))
                    break;

            } while (true);

            RefreshOrientations(out row, out col, verticalOrientation, horizontalOrientation);

            do
            {
                row--;
                if (row == -1)
                    break;

                if (!MoveCondition(row, col))
                    break;
            } while (true);

            RefreshOrientations(out row, out col, verticalOrientation, horizontalOrientation);

            do
            {
                col++;
                if (col == IBoardService.BOARD_SIZE)
                    break;

                if (!MoveCondition(row, col))
                    break;
            } while (true);

            RefreshOrientations(out row, out col, verticalOrientation, horizontalOrientation);

            do
            {
                col--;
                if (col == -1)
                    break;

                if (!MoveCondition(row, col))
                    break;
            } while (true);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
