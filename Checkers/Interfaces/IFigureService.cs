using Checkers.Entities.Figures;

namespace Checkers.Interfaces
{
    public interface IFigureService
    {
        void Move(CheckersBoardBlock checkersBoardBlock, CheckersBoardBlock emptyBoardBlock);
        void Cut(CheckersBoardBlock checkersBoardBlock, CheckersBoardBlock cuttedBoardBlock, CheckersBoardBlock emptyBoardBlock);
        void MovableBlocks(CheckersBoardBlock checkersBoardBlock);
        void CutableBlocks(CheckersBoardBlock checkersBoardBlock);
    }
}
