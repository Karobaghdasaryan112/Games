using Chess.Enums;
using Chess.Interfaces;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chess.Entities.Figures
{
    public class Knight : FigureBase<Knight>
    {
        public Knight(Color color) : base(color)
        {

        }

        //public override Rectangle[] MovableBlocks(Grid boardGrid)
        //{
        //    PAINT_RECTANGLES.Clear();
        //    _boardService.MovableRectangles.Clear();

        //    int row = (int)GetPosition().GetVerticalOrientation();
        //    int col = (int)GetPosition().GetHorizontalOrientation();

        //    if (row + 2 < IBoardService.BOARD_SIZE && col + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row, col, PAINT_RECTANGLES);

        //    if (row + 2 < IBoardService.BOARD_SIZE && col - 1 >= 0)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row + 2, col - 1, PAINT_RECTANGLES);

        //    if (row - 2 >= 0 && col + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row - 2, col + 1, PAINT_RECTANGLES);

        //    if (row - 2 >= 0 && col - 1 >= 0)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row - 2, col - 1, PAINT_RECTANGLES);

        //    if (col + 2 < IBoardService.BOARD_SIZE && row + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row + 1, col + 2, PAINT_RECTANGLES);

        //    if (col + 2 < IBoardService.BOARD_SIZE && row - 1 >= 0)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row - 1, col + 2, PAINT_RECTANGLES);

        //    if (col - 2 >= 0 && row + 1 < IBoardService.BOARD_SIZE)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row + 1, col - 2, PAINT_RECTANGLES);

        //    if (col - 2 >= 0 && row - 1 >= 0)
        //        AddExistingRectangleOnRectangeCollection(boardGrid, row - 1, col - 2, PAINT_RECTANGLES);

        //    AddRectanglesIntoDictionary();
        //    return PAINT_RECTANGLES.ToArray();

        //}
    }
}
