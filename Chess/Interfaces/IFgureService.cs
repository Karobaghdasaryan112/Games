using Chess.Enums;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chess.Interfaces
{
    public interface IFgureService
    {
        void AddExistingRectangleOnRectangeCollection(Grid boardGrid,VerticalOrientation row ,HorizontalOrientation col,List<Rectangle> MoveableRectangles);
    }
}
