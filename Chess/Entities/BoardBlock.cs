using Chess.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess.Entities
{
    public class BoardBlock : IBoardBlock
    {
        //private readonly IAnimationService animationService;
        //Block Colors
        //[
        public static readonly SolidColorBrush MOVE_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFF00"));
        public static readonly SolidColorBrush EVENT_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFA9A9A9"));
        public static readonly SolidColorBrush WHITE_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FF2F4F4F"));
        public static readonly SolidColorBrush BLACK_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFDEB887"));
        public static readonly double MOUSE_ENTER_OPACITY = 0.6;
        public static readonly double NOUSE_LEAVE_OPACITY = 1.0;
        public static readonly int MOUSE_ENTER_RECTANGLE_RADIUS = 16;
        public static readonly int MOUSE_LEAVE_RECTANGLE_RADIUS = 0;
        public static readonly int ANIMATION_MILISECOND = 300;
        //]

        //Board Block
        //[
        public Grid RectangleGrid { get; set; }

        public Rectangle RectangleForAnimation { get; set; }
        //]

        //Figure in Block
        //[
        public IFigure Figure { get; set; }
        //]

        //Block Position
        //[
        public Position Position { get; set; }
        //]

        //Block ActualColor
        //[
        public SolidColorBrush ActualColor { get; set; }
        //]

        //Figure Image
        //[
        public Image FigureImage { get; set; }


        //]
    }
}
