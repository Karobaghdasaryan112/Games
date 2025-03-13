using Chess.Entities;
using Chess.Enums;

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

        public static BoardBlock GetBoardBlockWithFigureName(this List<BoardBlock> boardBlocks, string figureName, Color figureColor)
        {
            return boardBlocks.Where(B => B.Figure?.GetFigureName() == figureName && B?.Figure?.GetColor().ToString() == figureColor.ToString()).FirstOrDefault();
        }

        public static BoardBlock GetBoardBlockWithPosition(this List<BoardBlock> boardBlocks, Position position)
        {
            return boardBlocks.Where(Board => Board.Position == position).FirstOrDefault();
        }
    }
}
