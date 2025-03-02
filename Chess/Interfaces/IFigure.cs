using Chess.Entities;
using Chess.Enums;
using System.Windows.Controls;
namespace Chess.Interfaces
{
    public interface IFigure
    {
        protected const string FigureImagesDirectory = @"PNGs\FigurePNGs\";

        void Move(Position newPosition);

        void Move(Position newPosition, IFigure killingFigure);

        string GetFigureName();

        Position GetPosition();

        void SetPosition(Position position);

        Color GetColor();

        void SetColor(Color color);

        bool IsMovePossible(Position newPosition);

        bool IsAttackPossible(Position newPosition);

        void Initialize();

        string GetPngPath();

        Image GetImage();   
    }
}
