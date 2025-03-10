

using System.Windows.Media;
using System.Windows;
using Chess.Entities;

namespace Chess.Interfaces
{
    public interface IAnimationService
    {
        void AnimateCell(BoardBlock cell, SolidColorBrush newColor, double newOpacity, double radius);

        BoardBlock MovableBlocksAnimation(object sender,List<BoardBlock>[] rectangles, SolidColorBrush newColor, SolidColorBrush cutColor, double newOpacity, double radius);

        void MovableBlocksAnimationDisable();
    }
}
