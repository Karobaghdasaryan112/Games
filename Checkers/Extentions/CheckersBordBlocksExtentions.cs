
using Checkers.Entities.Figures;
using Checkers.Enums;
using Checkers.Services;

namespace Checkers.Extentions
{
    public static class CheckersBordBlocksExtentions
    {
        public static CheckersBoardBlock GetBoardBlock(this List<CheckersBoardBlock> boardBlocks,VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            return boardBlocks.Where(block => block.Position.GetVerticalOrientation() == verticalOrientation && block.Position.GetHorizontalOrientation() == horizontalOrientation).FirstOrDefault();
        }
    }
}
