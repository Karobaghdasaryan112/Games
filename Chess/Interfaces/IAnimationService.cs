

using System.Windows.Media;
using System.Windows;
using Chess.Entities;

namespace Chess.Interfaces
{
    public interface IAnimationService
    {
        void AnimateCell<TElement>(TElement cell, SolidColorBrush newColor, double newOpacity, double radius) where TElement : UIElement;

        void MovableBlocksAnimation(List<BoardBlock> rectangles, SolidColorBrush newColor, double newOpacity, double radius);
    }
}
