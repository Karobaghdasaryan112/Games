using System.Windows.Controls;

namespace Chess.Interfaces
{
    public interface IBoardService
    {
        void BoardInitialize(Grid BoardGrid, int boardSize);
    }
}
