using Chess.Entities;

namespace Chess.Extentions
{
    public static class ListExtentions
    {
        public static BoardBlock GetElement(this List<BoardBlock> uIElements, Position position)
        {
            return uIElements.FirstOrDefault(element =>
            element.Position.GetVerticalOrientation() == position.GetVerticalOrientation() &&
            element.Position.GetHorizontalOrientation() == position.GetHorizontalOrientation());
        }
    }
}
