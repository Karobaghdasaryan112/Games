using System.Windows.Controls;

namespace Checkers.Interfaces
{
    public interface IBoardService
    {
        void BoardInitialize(Grid BoardGrid, int boardSize);
    }
}
