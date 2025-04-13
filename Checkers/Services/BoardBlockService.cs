using Checkers.Entities.Figures;
using Checkers.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
namespace Checkers.Services
{
    public class BoardBlockService : IBoardBlockService
    {
        private IAnimationService _animationService;
        private IFigureService _figureService;

        public BoardBlockService(IAnimationService animationService, IFigureService figureService)
        {
            _animationService = animationService;
            _figureService = figureService;
        }


        public void SetBoardBlockIntoBoard(Grid boardGrid, CheckersBoardBlock checkersBoardBlock)
        {

            if (checkersBoardBlock == null)
                return;

            var rectangle = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            var cellGrid = new Grid();

            cellGrid.Children.Add(rectangle);

            if (checkersBoardBlock.Figure != null)
            {
                cellGrid.Children.Add(checkersBoardBlock.Figure.GetImage());
            }
            else
                cellGrid.Children.Add(new Image());

            checkersBoardBlock.AnimationRectangle = rectangle;
            checkersBoardBlock.RectangleGrid = cellGrid;
            checkersBoardBlock.AnimationRectangle.Fill = checkersBoardBlock.ActualColor;


            Grid.SetRow(cellGrid, (int)checkersBoardBlock.Position.GetVerticalOrientation());
            Grid.SetColumn(cellGrid, (int)checkersBoardBlock.Position.GetHorizontalOrientation());

            checkersBoardBlock.ApplyCheckersBoarBlockAnimation();

            BoardService.BoardBlocks.Add(checkersBoardBlock);

            boardGrid.Children.Add(cellGrid);
            SetEventsIntoNewBoardBlock(checkersBoardBlock);
        }

        public void OnEmptyBoardButtonUp(CheckersBoardBlock checkersBoardBlock)
        {
            if (checkersBoardBlock.Figure != null)
                return;

            if (BoardService.PreviusCheckersBoardBlock?.Figure == default)
                return;

            if (!BoardService.IsMyTurn(BoardService.PreviusCheckersBoardBlock.Figure))
                return;


            var myBlocks = BoardService.BoardBlocks.Where(block => block.Figure != default && BoardService.IsMyTurn(block.Figure) && block.Figure.CuttedBlocks.Any());

            if (myBlocks.Count() == 0)
            {


                var newEmptyBoardBlock = BoardService.BoardBlocks.FirstOrDefault(block => block.Position == BoardService.PreviusCheckersBoardBlock.Position);
                _figureService.Move(BoardService.PreviusCheckersBoardBlock, checkersBoardBlock);

                var newFigureBoaredBlock = BoardService.BoardBlocks.FirstOrDefault(block => block.Position == checkersBoardBlock.Position);

                SetEventsIntoNewBoardBlock(newFigureBoaredBlock);
                SetEventsIntoNewBoardBlock(newEmptyBoardBlock);

            }
            else
            {
                //Figure Service FigureCut 
                //Deleting the first CuttableBlock
                //painting the rest Cutables block
                //while the CutableBlocksCount is not equal to 0
                //Turn Switching
                
            }

            _animationService.RectangleColorsDisable();

            var myCheckersBlocks = BoardService.BoardBlocks.Where(block => block.Figure != default && BoardService.IsMyTurn(block.Figure));
            foreach (var myBlock in myCheckersBlocks)
            {
                _figureService.CutableBlocks(myBlock);
            }


        }

        public void OnFigureButtonUp(CheckersBoardBlock checkersBoardBlock)
        {


            _animationService.RectangleColorsDisable();
            _animationService.SetBlockClickedAniimation(checkersBoardBlock);

            BoardService.BoardPaintedToMove.Clear();
            BoardService.MovableBoardBlocks.Clear();


            if (checkersBoardBlock.Figure == null)
                return;

            var Turn = BoardService.Turn;
            if (!BoardService.IsMyTurn(checkersBoardBlock.Figure))
                return;

            BoardService.PreviusCheckersBoardBlock = checkersBoardBlock;

            var myBlocks = BoardService.BoardBlocks.Where(block => block.Figure != default && BoardService.IsMyTurn(block.Figure) && block.Figure.CuttedBlocks.Any()).ToList();


            if (myBlocks.Count == 0)
                _figureService.MovableBlocks(checkersBoardBlock);
            else
            {
                var myClickedBlock = myBlocks.Where(block => block == checkersBoardBlock).FirstOrDefault();

                if (myClickedBlock == default)
                    return;

                foreach (var nestedItem in myClickedBlock.Figure.CuttedBlocks)
                {
                    _animationService.AnimateCell(nestedItem.First(), CheckersBoardBlock.CUT_COLOR, CheckersBoardBlock.MOUSE_ENTER_OPACITY, CheckersBoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS);
                }
            }

        }

        private void SetEventsIntoNewBoardBlock(CheckersBoardBlock checkersBoardBlock)
        {
            if (checkersBoardBlock == default)
                return;

            checkersBoardBlock.ApplyCheckersBoarBlockAnimation();
            if (checkersBoardBlock.Figure != default)
                checkersBoardBlock.Figure.GetImage().MouseLeftButtonUp += (s, e) => OnFigureButtonUp(checkersBoardBlock);
            else
                checkersBoardBlock.AnimationRectangle.MouseLeftButtonUp += (s, e) => OnEmptyBoardButtonUp(checkersBoardBlock);
        }
    }
}
