using Chess.Entities;
using Chess.Enums;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace Chess.Interfaces
{
    public interface IFigure
    {
        //protected const string FIGURE_IMAGE_DIRECTORY = @"PNGs\FigurePNGs\";
        List<BoardBlock>[] MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation);
        string GetFigureName();
        Color GetColor();
        void SetColor(Color color);
        void Initialize();
        string GetPngPath();
        Image GetImage();
        void SetImage(Image image);
        bool IsReadyToMove { get; protected set; }
    }
}
