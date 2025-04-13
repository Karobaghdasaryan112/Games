using Checkers.Entities;
using Checkers.Entities.Figures;
using Checkers.Enums;
using System.Windows.Controls;
using System.Windows.Media;

namespace Checkers.Interfaces
{
    public interface IBoardBlockService
    {
        /// <summary>
        /// Initialize the BoardBlock and Figure on it with Position
        /// </summary>
        /// <param name="boardGrid"></param>
        void SetBoardBlockIntoBoard(Grid boardGrid, CheckersBoardBlock checkersBoardBlock);
        void OnFigureButtonUp(CheckersBoardBlock BoardBlock);
        void OnEmptyBoardButtonUp(CheckersBoardBlock checkersBoardBlock);
    }
}
