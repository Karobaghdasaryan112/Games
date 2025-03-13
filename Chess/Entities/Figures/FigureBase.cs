using Chess.Enums;
using Chess.Extentions;
using Chess.Interfaces;
using Chess.Services;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Chess.Entities.Figures
{
    public class FigureBase<TFigure> : IFigure where TFigure : IFigure
    {
        public static List<BoardBlock> MoveableRectangles = new List<BoardBlock>();
        public static List<BoardBlock> CutableRectangles = new List<BoardBlock>();
        protected Enums.Color _color;
        protected Image _element;
        protected string _pngPath;
        protected TFigure _figureType;
        protected bool _isReadyToMove;
        protected bool _isReadyToCut;

        public bool IsReadyToMove { get => _isReadyToMove; set => _isReadyToMove = value; }

        public bool IsReadyToCut { get => _isReadyToCut; set => _isReadyToCut = value; }
        public string FigureName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public virtual string GetFigureName()
        {
            return typeof(TFigure).Name;
        }

        public Image GetImage()
        {
            return _element;
        }

        public void SetImage(Image image)
        {
            _element = image;
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

        public void SetColor(Enums.Color color)
        {
            _color = color;
        }

        protected string GetPath(string FigureName, Enums.Color color)
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

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, Color color)
        {
            return default;
        }

        protected void RefreshOrientations(out int row, out int col, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            col = (int)horizontalOrientation; row = (int)verticalOrientation;
        }


        protected bool MoveCondition(int row, int col)
        {
            var MovableBoardBlock = BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row, (HorizontalOrientation)col));

            if (MovableBoardBlock?.Figure == default)
            {
                MoveableRectangles.Add(MovableBoardBlock);
                return true;
            }
            else
            {
                if ( this._color != MovableBoardBlock?.Figure?.GetColor())
                    CutableRectangles.Add(MovableBoardBlock);
            }

            return false;
        }
    }
}
