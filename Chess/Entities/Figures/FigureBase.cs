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
        protected Enums.Color _color;
        protected Image _element;
        protected string _pngPath;
        protected TFigure _figureType;
        protected bool _isReadyToMove;

        public bool IsReadyToMove { get => _isReadyToMove; set => _isReadyToMove = value; }

        public FigureBase(Enums.Color color)
        {
            _color = color;
            _pngPath = GetPath(GetFigureName(), GetColor());
            _element = new Image();
        }

        public Enums.Color GetColor()
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

        public string GetPngPath() => GetPath(GetFigureName(), GetColor());

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

        public virtual void Move(Position newPosition, Position currentPosition)
        {   

        }

        public void SetColor(Enums.Color color)
        {
            _color = color;
        }

        protected string GetPath(string FigureName,Enums.Color color)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string projectDirectory = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                        Directory.GetParent(currentDirectory).FullName)
                    .FullName)
                .FullName)
                .FullName;


            string imagesDirectory = System.IO.Path.Combine(projectDirectory, "PNGs", "FigurePNGs");

            string FigurePng = $"{FigureName}{color}.png";

            string imagePath = System.IO.Path.Combine(imagesDirectory, $"{FigurePng}");

            return imagePath;
        }

        public List<BoardBlock> MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, int boardSize)
        {
            throw new NotImplementedException();
        }
    }
}
