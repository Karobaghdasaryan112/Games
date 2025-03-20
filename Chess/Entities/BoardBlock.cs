using Chess.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess.Entities
{
    public class BoardBlock : IBoardBlock,ICloneable
    {
        public static readonly SolidColorBrush MOVE_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFF00"));
        public static readonly SolidColorBrush EVENT_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFA9A9A9"));
        public static readonly SolidColorBrush WHITE_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FF2F4F4F"));
        public static readonly SolidColorBrush BLACK_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFDEB887"));
        public static readonly SolidColorBrush CUT_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFF5F5DC"));
        public static readonly SolidColorBrush CHECKED_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FF8B0000"));
        public static readonly SolidColorBrush CASTLING_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFD2691E"));

        public static readonly double MOUSE_ENTER_OPACITY = 0.6;
        public static readonly double NOUSE_LEAVE_OPACITY = 1.0;
        public static readonly int MOUSE_ENTER_RECTANGLE_RADIUS = 16;
        public static readonly int MOUSE_LEAVE_RECTANGLE_RADIUS = 0;
        public static readonly int ANIMATION_MILISECOND = 300;

        public Grid RectangleGrid { get; set; }

        public Rectangle RectangleForAnimation { get; set; }

        public IFigure Figure { get; set; }

        public Position Position { get; set; }

        public SolidColorBrush ActualColor { get; set; }

        public Image FigureImage { get; set; }
        

        public bool IsReadyToMove {  get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void SetFigureImageIntoRectangle(Image image)
        {
            this.FigureImage = image;

            var RectangleImage = RectangleGrid?.Children?.OfType<Image>().FirstOrDefault();

            if (RectangleImage != null)
            {
                RectangleGrid.Children.Remove(RectangleImage);
            }

            if (image != null)
            {
                var newImage = new Image
                {
                    Source = image.Source,
                };

                RectangleGrid.Children.Add(newImage);

                this.FigureImage = newImage;

                return;
            }
            RectangleGrid.Children.Add(new Image());
        }
    }
}
