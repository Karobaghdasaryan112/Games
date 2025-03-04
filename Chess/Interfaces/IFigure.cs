using Chess.Entities;
using Chess.Enums;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace Chess.Interfaces
{
    public interface IFigure
    {
        //protected const string FIGURE_IMAGE_DIRECTORY = @"PNGs\FigurePNGs\";
        void Move(Position newPosition,Position currentPosition);

        List<BoardBlock> MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, int boardSize);
        string GetFigureName();
        Color GetColor();
        void SetColor(Color color);
        void Initialize();
        string GetPngPath();
        Image GetImage();
        bool IsReadyToMove { get; protected set; }
    }
}
