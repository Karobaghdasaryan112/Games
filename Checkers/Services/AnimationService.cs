using Checkers.Interfaces;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Checkers.Entities.Figures;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows;

namespace Checkers.Services
{
    public class AnimationService : IAnimationService
    {
        public async Task AnimateCell(CheckersBoardBlock cellBlock, SolidColorBrush newColor, double newOpacity, double radius)
        {
            if (cellBlock != null)
            {

                var animationOpacity = new DoubleAnimation(newOpacity, TimeSpan.FromMilliseconds(300));
                var animationRadius = new DoubleAnimation(radius, TimeSpan.FromMilliseconds(300));
                cellBlock.AnimationRectangle.BeginAnimation(Rectangle.OpacityProperty, animationOpacity);
                cellBlock.AnimationRectangle.BeginAnimation(Rectangle.RadiusXProperty, animationRadius);
                cellBlock.AnimationRectangle.BeginAnimation(Rectangle.RadiusYProperty, animationRadius);
                cellBlock.AnimationRectangle.Fill = newColor;

                await Task.Delay(0);
            }
        }

        public void ApplyRectangleAnimationIntoBoardBlock(FigureBase figureBase, Rectangle rectangleAnimation)
        {

            figureBase.GetImage().MouseEnter += (s, e) => rectangleAnimation.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseEnterEvent });
            figureBase.GetImage().MouseLeave += (s, e) => rectangleAnimation.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = UIElement.MouseLeaveEvent });
        }

        public void ApplyRectangleAnimation(Rectangle rectangleAnimation, CheckersBoardBlock checkersBoardBlock)
        {
            rectangleAnimation.MouseEnter += async (s, e) =>
            {
                if (rectangleAnimation.Fill != CheckersBoardBlock.CUT_COLOR && rectangleAnimation.Fill != CheckersBoardBlock.MOVE_COLOR && rectangleAnimation.Fill != CheckersBoardBlock.CLICK_COLOR)
                {
                    await (AnimateCell(checkersBoardBlock, CheckersBoardBlock.EVENT_COLOR, CheckersBoardBlock.MOUSE_ENTER_OPACITY, CheckersBoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS));
                }
            };

            rectangleAnimation.MouseLeave += async (s, e) =>
            {
                if (rectangleAnimation.Fill != CheckersBoardBlock.CUT_COLOR && rectangleAnimation.Fill != CheckersBoardBlock.MOVE_COLOR && rectangleAnimation.Fill != CheckersBoardBlock.CLICK_COLOR)
                    await (AnimateCell(checkersBoardBlock, checkersBoardBlock.ActualColor, CheckersBoardBlock.MOUSE_LEAVE_OPACITY, CheckersBoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS));
            };
        }

        public async void RectangleColorsDisable()
        {
            foreach (var board in BoardService.BoardPaintedToMove)
            {
                await AnimateCell(board, board.ActualColor, CheckersBoardBlock.MOUSE_LEAVE_OPACITY, CheckersBoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
            }
        }

        public async void PaintCutableBlocks()
        {
            if (BoardService.CutableBoardBlocks.Count != 0)
                foreach (var CurrentAndCutableBlocks in BoardService.CutableBoardBlocks)
                {

                    await AnimateCell(CurrentAndCutableBlocks[1], CheckersBoardBlock.CUT_COLOR, CheckersBoardBlock.MOUSE_ENTER_OPACITY, CheckersBoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                }
        }

        public async void PaintMovableBlocks()
        {
            RectangleColorsDisable();
            if (BoardService.MovableBoardBlocks.Count != 0)
                foreach (var block in BoardService.MovableBoardBlocks)
                {
                    await AnimateCell(block, CheckersBoardBlock.MOVE_COLOR, CheckersBoardBlock.MOUSE_ENTER_OPACITY, CheckersBoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                }
        }

        private async void BoardBlockChecked(CheckersBoardBlock checkersBoardBlock)
        {
            await AnimateCell(checkersBoardBlock, CheckersBoardBlock.CLICK_COLOR, CheckersBoardBlock.MOUSE_ENTER_OPACITY, CheckersBoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);

        }

        private async void BoardBlockUnChecked(CheckersBoardBlock checkersBoardBlock)
        {
            await AnimateCell(checkersBoardBlock, checkersBoardBlock.ActualColor, CheckersBoardBlock.MOUSE_LEAVE_OPACITY, CheckersBoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS);
        }

        public async Task AnimationForPaintingBlock()
        {
            await Task.Delay(35);
        }

        public void SetBlockClickedAniimation(CheckersBoardBlock checkersBoardBlock)
        {
            BoardService.BoardBlocks.
                Where(block =>
                block.AnimationRectangle.Fill == CheckersBoardBlock.CLICK_COLOR).
                ToList().
                ForEach(block => BoardBlockUnChecked(block));

            BoardBlockChecked(checkersBoardBlock);
        }


        public Task AnimateOpacity(CheckersBoardBlock block, double targetOpacity, int duration)
        {
            var tcs = new TaskCompletionSource<bool>();

            var animation = new DoubleAnimation(targetOpacity, TimeSpan.FromMilliseconds(duration));


            animation.Completed += (s, e) => tcs.TrySetResult(true);

            block.AnimationRectangle.BeginAnimation(Rectangle.OpacityProperty, animation);

            return tcs.Task;
        }
    }
}
    