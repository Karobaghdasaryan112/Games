using Checkers.Enums;
using Checkers.Extentions;
using Checkers.Services;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Checkers.Entities.Figures
{
    public class Figure : FigureBase
    {
        public const string FigureName = "Figure";

        public override void Initialize(Color color)
        {
            if (color == default)
                return;

            _figureColor = color;

            _image = (_image == default) ? new Image() : _image;

            string FigurePath = $"{color}Figure{_pictureFormat}";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagesDirectory = System.IO.Path.Combine(baseDirectory, "PNGs", "FigurePNGs");
            string imagePath = System.IO.Path.Combine(imagesDirectory, FigurePath);

            _image.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }
    }
}
