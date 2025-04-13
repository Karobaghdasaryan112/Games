using Checkers.Entities;
using Checkers.Enums;
using System.Windows.Controls;

namespace Checkers.Interfaces
{
    public interface IFigure
    {
        void SetColor(Color color);
        Color GetColor();
        Image GetImage();
        void SetImage(Image image);
        void Initialize(Color color, Position position);
        void ChangeFigureImage(Image image);
    }
}
