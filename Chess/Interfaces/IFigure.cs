using Chess.Entities;
using Chess.Enums;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace Chess.Interfaces
{
    public interface IFigure
    {
        //protected const string FIGURE_IMAGE_DIRECTORY = @"PNGs\FigurePNGs\";
        List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation,Color color);
        virtual string GetFigureName()
        {
            return "";
        }
        Color GetColor();
        void SetColor(Color color);
        void Initialize();
        string GetPngPath();
        Image GetImage();
        void SetImage(Image image);
        bool IsReadyToMove { get;  set; }
        bool IsReadyToCut { get;set; }

    }
}
