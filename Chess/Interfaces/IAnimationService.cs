

using System.Windows.Media;
using System.Windows;
using Chess.Entities;

namespace Chess.Interfaces
{
    public interface IAnimationService
    {
        Task AnimateCell(BoardBlock cell, SolidColorBrush newColor, double newOpacity, double radius);

        Task<BoardBlock> MovableBlocksAnimation(object sender,List<BoardBlock>[] rectangles, SolidColorBrush newColor, SolidColorBrush cutColor, double newOpacity, double radius);

        Task MovableBlocksAnimationDisable();

        Task AnimateOpacity(BoardBlock block, double targetOpacity, int duration);
    }
}
