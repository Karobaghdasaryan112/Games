using Chess.Interfaces;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media;
using Chess.Entities;

namespace Chess.Services
{
    public class AnimatonService : IAnimationService
    {

        public void AnimateCell(BoardBlock cellBlock, SolidColorBrush newColor, double newOpacity, double radius) 
        {
            if (cellBlock.IsReadyToMove == false)
            {
                var animationOpacity = new DoubleAnimation(newOpacity, TimeSpan.FromMilliseconds(300));

                var animationRadius = new DoubleAnimation(radius, TimeSpan.FromMilliseconds(300));
                cellBlock.RectangleForAnimation.BeginAnimation(Rectangle.OpacityProperty, animationOpacity);
                cellBlock.RectangleForAnimation.BeginAnimation(Rectangle.RadiusXProperty, animationRadius);
                cellBlock.RectangleForAnimation.BeginAnimation(Rectangle.RadiusYProperty, animationRadius);
                cellBlock.RectangleForAnimation.Fill = newColor;
            }
        }

        public BoardBlock MovableBlocksAnimation(object sender, List<BoardBlock>[] boardBlocks, SolidColorBrush noveColor, SolidColorBrush cutColor, double newOpacity, double radius)
        {
            var BoardBlock = sender as BoardBlock;
            BoardService.FigureAndMoveBlocks[0] = BoardBlock;
            MovableBlocksAnimationDisable();
            if (boardBlocks != null)
            {
                foreach (var boardBlock in boardBlocks[0])
                {
                    AnimateCell(boardBlock, noveColor, newOpacity, radius);
                    BoardService.BoardPaintedToMoveBlocks.Add(boardBlock);
                    boardBlock.IsReadyToMove = true;
                }
                foreach (var boardBlock in boardBlocks[1])
                {
                    AnimateCell(boardBlock, cutColor, newOpacity, radius);
                    BoardService.BoardPaintedToMoveBlocks.Add(boardBlock);
                    boardBlock.IsReadyToMove = true;
                }
            }
            return BoardBlock;
        }

        public void MovableBlocksAnimationDisable()
        {
            if (BoardService.BoardPaintedToMoveBlocks.Count > 0)
            {
                foreach (var BoardBlock in BoardService.BoardPaintedToMoveBlocks)
                {
                    BoardBlock.IsReadyToMove = false;
                    AnimateCell(BoardBlock, BoardBlock.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
                }
            }
        }
    }
}
