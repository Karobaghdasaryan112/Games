using System.Windows.Controls;

namespace Chess.Interfaces
{
    public interface IBoardService
    {
        public const int BOARD_SIZE = 8;
        void BoardInitialize(Grid BoardGrid, int boardSize);

    }
}
