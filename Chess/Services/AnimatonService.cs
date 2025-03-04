using Chess.Interfaces;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using Chess.Entities;

namespace Chess.Services
{
    public class AnimatonService : IAnimationService
    {
        public void PaintMovableBlockAnimation()
        {

        }

        public void AnimateCell<TElement>(TElement cell, SolidColorBrush newColor, double newOpacity, double radius) where TElement : UIElement
        {
            var animationOpacity = new DoubleAnimation(newOpacity, TimeSpan.FromMilliseconds(300));

            if (cell is Rectangle rectangle)
            {
                var animationRadius = new DoubleAnimation(radius, TimeSpan.FromMilliseconds(300));
                rectangle.BeginAnimation(Rectangle.OpacityProperty, animationOpacity);
                rectangle.BeginAnimation(Rectangle.RadiusXProperty, animationRadius);
                rectangle.BeginAnimation(Rectangle.RadiusYProperty, animationRadius);
                rectangle.Fill = newColor;
            }
        }

        public void MovableBlocksAnimation(List<BoardBlock> boardBlocks, SolidColorBrush newColor, double newOpacity, double radius) 
        {
            foreach (var boardBlock in boardBlocks)
            {
                AnimateCell<Rectangle>(boardBlock.RectangleGrid.Children.OfType<Rectangle>().FirstOrDefault(), newColor, newOpacity, radius);
            }
        }
    }
}
