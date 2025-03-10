
using Chess.Entities;
using Chess.Enums;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess.Interfaces
{
    public interface IBoardBlockService
    {
        BoardBlock SetBoardBlockOnBoard<TFigure>(SolidColorBrush boardNormalColor, TFigure figure, Position position, Grid boardGrid) where TFigure : IFigure;
        void SetFigureAnimations(Grid boardGrid,VerticalOrientation verticalOrientation,HorizontalOrientation horizontalOrientation,IFigure newFigure,Grid RectangleGridForBlockBoard, BoardBlock newBoardBlockWithFigure);
        void SetEmptyBoardAnimations(BoardBlock newBoardBlockWithFigure,Grid boardGrid);
    }
}
