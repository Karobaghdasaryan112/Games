
using Checkers.Entities.Figures;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Checkers.Interfaces
{
    public interface IAnimationService
    {

        Task AnimateOpacity(CheckersBoardBlock block, double targetOpacity, int duration);
        void ApplyRectangleAnimationIntoBoardBlock(FigureBase figureBase, Rectangle rectangleAnimation);

        void ApplyRectangleAnimation(Rectangle rectangleAnimation,CheckersBoardBlock checkersBoardBlock);

        Task AnimateCell(CheckersBoardBlock cellBlock, SolidColorBrush newColor, double newOpacity, double radius);

        void PaintCutableBlocks();

        void PaintMovableBlocks();

        void RectangleColorsDisable();

        void SetBlockClickedAniimation(CheckersBoardBlock checkersBoardBlock);

        Task AnimationForPaintingBlock();
    }
}
