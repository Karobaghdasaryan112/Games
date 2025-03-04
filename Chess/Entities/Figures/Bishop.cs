using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chess.Entities.Figures
{
    public class Bishop : FigureBase<Bishop>
    {

        public Bishop(Chess.Enums.Color color) : base(color)
        {

        }
        //Figure Specific Move Logic
        public override void Move(Position newPosition, Position currentPosition)
        {

        }
        public  Rectangle[] MovableBlocks(Grid boardGrid,VerticalOrientation verticalOrientation,HorizontalOrientation horizontalOrientation,int boardSize)
        {

            int col = (int)horizontalOrientation;
            int row = (int)verticalOrientation;

            do
            {
                col++; row++;

                if (col == 8 || row == 8)
                    break;

            } while ((col < boardSize && row < boardSize));

            //RefreshOrientations(out row, out col);

            do
            {
                col++; row--;

                if (col == -1 || row == 8)
                    break;

                //if (!AddExistingRectangleOnRectangeCollection(boardGrid, row, col, PAINT_RECTANGLES))
                //    break;
            } while (row >= 0 && col < boardSize);


            //RefreshOrientations(out row, out col);

            do
            {
                col--; row++;

                if (col == 8 || row == -1)
                    break;

                //if (!AddExistingRectangleOnRectangeCollection(boardGrid, row, col, PAINT_RECTANGLES))
                    break;
            } while ((col >= 0 && row < boardSize));

            //RefreshOrientations(out row, out col);

            do
            {
                col--; row--;

                if (col == -1 || row == -1)
                    break;

                //if (!AddExistingRectangleOnRectangeCollection(boardGrid, row, col, PAINT_RECTANGLES))
                    break;
            } while (row >= 0 && col >= 0);


            //AddRectanglesIntoDictionary();
            //return PAINT_RECTANGLES.ToArray();
            return null;
        }
    }
}
