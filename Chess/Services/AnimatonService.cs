using Chess.Interfaces;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media;
using Chess.Entities;

namespace Chess.Services
{
    public class AnimatonService : IAnimationService
    {
        public async Task AnimateCell(BoardBlock cellBlock, SolidColorBrush newColor, double newOpacity, double radius)
        {
            if (cellBlock != null)
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
                await Task.Delay(0);

            }
        }

        public async Task<BoardBlock> MovableBlocksAnimation(object sender, List<BoardBlock>[] boardBlocks, SolidColorBrush noveColor, SolidColorBrush cutColor, double newOpacity, double radius)
        {
            var BoardBlock = sender as BoardBlock;
            BoardService.FigureAndMoveBlocks[0] = BoardBlock;
            await MovableBlocksAnimationDisable();
            if (boardBlocks != null)
            {
                foreach (var boardBlock in boardBlocks[0])
                {
                    await AnimateCell(boardBlock, noveColor, newOpacity, radius);
                    BoardService.BoardPaintedToMoveBlocks.Add(boardBlock);
                    boardBlock.IsReadyToMove = true;
                }
                foreach (var boardBlock in boardBlocks[1])
                {
                   await AnimateCell(boardBlock, cutColor, newOpacity, radius);
                    BoardService.BoardPaintedToMoveBlocks.Add(boardBlock);
                    boardBlock.IsReadyToMove = true;
                }
            }
            return BoardBlock;
        }
        public async Task MovableBlocksAnimationDisable()
        {
            if (BoardService.BoardPaintedToMoveBlocks.Count > 0)
            {

                foreach (var BoardBlock in BoardService.BoardPaintedToMoveBlocks)
                {
                    if (BoardBlock.RectangleForAnimation.Fill == BoardBlock.CHECKED_COLOR && BoardBlock.Figure != null)
                        continue;

                    BoardBlock.IsReadyToMove = false;
                    await AnimateCell(BoardBlock, BoardBlock.ActualColor, BoardBlock.NOUSE_LEAVE_OPACITY, BoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);

                }
            }
        }

        public  Task AnimateOpacity(BoardBlock block, double targetOpacity, int duration)
        {
            var tcs = new TaskCompletionSource<bool>();

            var animation = new DoubleAnimation(targetOpacity, TimeSpan.FromMilliseconds(duration));


            animation.Completed += (s, e) => tcs.TrySetResult(true);

            block.RectangleForAnimation.BeginAnimation(Rectangle.OpacityProperty, animation);

            return tcs.Task;
        }
    }
}
