using Chess.Entities;
using Chess.Enums;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chess.Interfaces
{
    public interface IFigureService
    {

        void FigureCut(BoardBlock cuttingBoardblock, BoardBlock cuttedBoardblock, Grid boardGrid);

        void FigureMove(BoardBlock figureBoardBlock, BoardBlock moveBoardBlock, Grid boardGrid);

        void ReInitializeFiguresAndEmptiBoardBlocks(BoardBlock figureBoardBlock, BoardBlock moveBoardBlock,Image setImage,IFigure setNewFigure);

        Task<bool> IsKingCheckedCondition();

        void RoleBackAfterMoving(BoardBlock cuttingBoardBlock,BoardBlock cuttedBoardBlock, Grid boardGrid);

        void SetImageEventIntoBoardBlockEvent(IFigure newFigure, BoardBlock newBoardBlockWithFigure);

        Task IsMateState(Grid boardGrid);

        List<BoardBlock[]> CastlingMoveAnimation(Grid boardGrid);

        void CastlingLogic(List<BoardBlock[]> listOfBoardBlocks, BoardBlock clickedBoardBlock, Grid boardGrid);
    }
}
