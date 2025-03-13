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

        public override string GetFigureName()
        {
            return typeof(Rook).Name;
        }

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation,Color color)
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
