using Chess.Interfaces;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chess.Entities.Figures
{
    public class King : FigureBase<King>
    {
        public King(Enums.Color color) : base(color)
        {
        }

        //public override Rectangle[] MovableBlocks(Grid boardGrid)
        //{
        //    PAINT_RECTANGLES.Clear();

        //    int col = (int)GetPosition().GetHorizontalOrientation();
        //    int row = (int)GetPosition().GetVerticalOrientation();

        //    if (col + 1 < IBoardService.BOARD_SIZE && row + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row + 1, col + 1, PAINT_RECTANGLES);

        //    if (col - 1 >= 0 && row - 1 >= 0)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row - 1, col - 1, PAINT_RECTANGLES);

        //    if (col - 1 >= 0 && row + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row + 1, col - 1, PAINT_RECTANGLES);

        //    if (col + 1 < IBoardService.BOARD_SIZE && row - 1 >= 0)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row - 1, col + 1, PAINT_RECTANGLES);

        //    if (col + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row, col + 1, PAINT_RECTANGLES);

        //    if (row + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row + 1, col, PAINT_RECTANGLES);

        //    if (col - 1 >= 0)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row, col - 1, PAINT_RECTANGLES);

        //    if (col + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row, col + 1, PAINT_RECTANGLES);

        //    AddRectanglesIntoDictionary();
        //    return PAINT_RECTANGLES.ToArray();
        //}
    }
}
