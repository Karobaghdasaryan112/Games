
using Chess.Entities;
using Chess.Enums;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chess.Interfaces
{
    public interface IBoardBlockService
    {
        BoardBlock SetBoardBlockOnBoard<TFigure>(SolidColorBrush boardNormalColor, TFigure figure, Position position, Grid boardGrid) where TFigure : IFigure;
    }
}
