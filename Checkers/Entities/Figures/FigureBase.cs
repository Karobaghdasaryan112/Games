
using Checkers.Enums;
using System.Windows.Controls;

namespace Checkers.Entities.Figures
{
    public class FigureBase
    {
        protected Color _figureColor;
        protected Image _image;
        protected string _pictureFormat = ".png";
        public List<List<CheckersBoardBlock>> CuttedBlocks = new List<List<CheckersBoardBlock>>();
        public static List<int[]> AddedNumbersForFigure = new List<int[]>
            {
                new int[] { 1, 1 },
                new int[] { -1, 1 },
                new int[] { 1, -1},
                new int[] { -1, -1}
            };
        public Color GetColor()
        {
            return _figureColor;
        }

        public Image GetImage()
        {
            return _image;
        }


        public void SetColor(Color color)
        {
            _figureColor = color;
        }

        public void SetImage(Image image)
        {
            _image = image;
        }


        public void ChangeFigureImage(Image image)
        {

            _image.Source = image.Source;
        }

        public virtual void Initialize(Color color)
        {

        }

        public virtual void GetCutableBlocks(CheckersBoardBlock figureBoardBlock)
        {

        }

        public virtual void GetMovableBoardBlocks(Color figureColor, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {

        }
    }
}
