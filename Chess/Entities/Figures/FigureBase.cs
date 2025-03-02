using Chess.Enums;
using Chess.Interfaces;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Chess.Entities.Figures
{
    public class FigureBase<TFigure> : IFigure where TFigure : IFigure
    {
        protected Position _position;
        protected Color _color;
        protected Image _element;
        protected string _pngPath;
        protected TFigure _figureType;

        public FigureBase(Position position, Color color)
        {
            _position = position;
            _color = color;
            _pngPath = GetPath(GetFigureName(), GetColor());
            _element = new Image();
        }

        public Color GetColor()
        {
            return _color;
        }

        public string GetFigureName()
        {
            return typeof(TFigure).Name;
        }

        public Image GetImage()
        {
            return _element;
        }

        public string GetPngPath()=> GetPath(GetFigureName(), GetColor());

        public Position GetPosition()
        {
            return _position;
        }

        public void Initialize()
        {
            if (File.Exists(_pngPath))
            {
                _element.Source = new BitmapImage(new Uri(_pngPath, UriKind.Absolute));
            }
            else
            {
                MessageBox.Show($"File doesnt exist: {_pngPath}", "exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool IsAttackPossible(Position newPosition)
        {
            return true;
        }

        public bool IsMovePossible(Position newPosition)
        {
            return true;
        }

        public void Move(Position newPosition)
        {   
        }

        public void Move(Position newPosition, IFigure killingFigure)
        {
            
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void SetPosition(Position position)
        {
            _position = position;
        }

        protected string GetPath(string FigureName,Color color)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string projectDirectory = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                        Directory.GetParent(currentDirectory).FullName)
                    .FullName)
                .FullName)
                .FullName;


            string imagesDirectory = Path.Combine(projectDirectory, "PNGs", "FigurePNGs");

            string FigurePng = $"{FigureName}{color}.png";

            string imagePath = Path.Combine(imagesDirectory, $"{FigurePng}");

            return imagePath;
        }
    }
}
