using System.Windows.Controls;

namespace Chess.Interfaces
{
    public interface IBoard
    {
        void CreateStartPositions(Grid BoardGrid);

        void CreateExistPositions(object[] Params,Grid BoardGrid);
    }
}
