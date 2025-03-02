using Chess.Enums;
using Chess.Interfaces;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Chess.Entities.Figures
{
    public class Knight : FigureBase<Knight>
    {
        public Knight(Position position, Color color) : base(position, color)
        {

        }
    }
}
