using Checkers.Interfaces;
using Checkers.Services;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Checkers.Entities.Figures
{
    public class CheckersBoardBlock
    {
        public static IAnimationService _animationService = new AnimationService();

        public static readonly SolidColorBrush MOVE_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFF00"));
        public static readonly SolidColorBrush EVENT_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFA9A9A9"));
        public static readonly SolidColorBrush BLACK_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FF2F4F4F"));
        public static readonly SolidColorBrush CUT_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFF5F5DC"));
        public static readonly SolidColorBrush WHITE_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFDEB887"));
        public static readonly SolidColorBrush CLICK_COLOR = ((SolidColorBrush)new BrushConverter().ConvertFrom("#FFD2691E"));

        public static readonly double MOUSE_ENTER_OPACITY = 0.6;
        public static readonly double MOUSE_LEAVE_OPACITY = 1.0;
        public static readonly int MOUSE_ENTER_RECTANGLE_RADIUS = 16;
        public static readonly int MOUSE_LEAVE_RECTANGLE_RADIUS = 0;
        public static readonly int ANIMATION_MILISECOND = 300;
        public SolidColorBrush ActualColor { get; set; }
        public FigureBase Figure { get; set; }
        public Position Position { get; set; }
        public Grid RectangleGrid { get; set; }
        public Rectangle AnimationRectangle { get; set; }

        public CheckersBoardBlock(Position position) : this()
        {

            this.Position = position;
        }

        public CheckersBoardBlock()
        {
            if (AnimationRectangle != default)
                ApplyCheckersBoarBlockAnimation();
        }

        public CheckersBoardBlock(Position position, FigureBase figure) : this()
        {
            Position = position;
            Figure = figure;
        }

        public CheckersBoardBlock(Rectangle animationRectangle, Grid rectangleGrid, Position position, FigureBase figure) : this()
        {
            AnimationRectangle = animationRectangle;
            RectangleGrid = rectangleGrid;
            Position = position;
            Figure = figure;
        }

        public void SetActualColor(SolidColorBrush color)
        {
            ActualColor = color;
            AnimationRectangle.Fill = ActualColor;
        }

        public void ApplyCheckersBoarBlockAnimation()
        {
            if (Figure != null)
                _animationService.ApplyRectangleAnimationIntoBoardBlock(Figure, AnimationRectangle);

            _animationService.ApplyRectangleAnimation(AnimationRectangle, this);
        }
    }
}
