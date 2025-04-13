using Checkers.Entities;
using Checkers.Entities.Figures;
using Checkers.Extentions;
using Checkers.Interfaces;
using System.Windows.Controls;
namespace Checkers.Services
{
    public class FigureService : IFigureService
    {
        private IAnimationService _animationService;
        public FigureService(IAnimationService animationService)
        {
            _animationService = animationService;
        }

        public void Cut(CheckersBoardBlock checkersBoardBlock, CheckersBoardBlock cuttedBoardBlock, CheckersBoardBlock emptyBoardBlock)
        {
            throw new NotImplementedException();
        }

        public void Move(CheckersBoardBlock checkersBoardBlock, CheckersBoardBlock emptyBoardBlock)
        {
            if (checkersBoardBlock == null || emptyBoardBlock == null)
                throw new ArgumentNullException(nameof(emptyBoardBlock), "Invalid Params");

            if (checkersBoardBlock?.Figure == default && emptyBoardBlock?.Figure != default)
                throw new ArgumentNullException(nameof(emptyBoardBlock), "invalid BoardBlocks");

            if (checkersBoardBlock?.AnimationRectangle.Fill != CheckersBoardBlock.CLICK_COLOR ||
                emptyBoardBlock?.AnimationRectangle.Fill != CheckersBoardBlock.MOVE_COLOR)
                return;


            emptyBoardBlock.Figure = checkersBoardBlock.Figure;
            RemoveFigureImageFromBoardBlock(checkersBoardBlock);

            Image newImage = new Image()
            {
                Source = checkersBoardBlock.Figure.GetImage().Source,
                Width = checkersBoardBlock.Figure.GetImage().Width,
                Height = checkersBoardBlock.Figure.GetImage().Height
            };

            checkersBoardBlock.Figure = default;

            SetNewImageIntoBoardBlock(emptyBoardBlock, newImage);

            _animationService.RectangleColorsDisable();

            BoardService.PreviusCheckersBoardBlock = default;

            BoardService.TurnSwitch();

        }

        public void MovableBlocks(CheckersBoardBlock checkersBoardBlock)
        {
            if (checkersBoardBlock.Figure == default)
                return;
            _animationService.RectangleColorsDisable();

            switch (checkersBoardBlock.Figure)
            {
                case Figure figure:
                    FigureMovableBlocks(checkersBoardBlock);
                    break;
                case Queen queen:
                    QueenMovableBoardBlcoks(checkersBoardBlock);
                    break;
            }
        }
        public void CutableBlocks(CheckersBoardBlock checkersBoardBlock)
        {
            if (checkersBoardBlock.Figure == default)
                return;

            switch (checkersBoardBlock.Figure)
            {
                case Figure figure:
                    FigureCutableBlocks(checkersBoardBlock);
                    break;
                case Queen queen:
                    QueenCutableBlocks(checkersBoardBlock);
                    break;
            }
        }


        private async void FigureCutableBlocks(CheckersBoardBlock checkersBoardBlock)
        {
            var myFigureBlocks = BoardService.BoardBlocks.Where(block => block.Figure != default && BoardService.IsMyTurn(block.Figure)).ToList();

            foreach (var myBlock in myFigureBlocks)
            {
                FindCutableBlocks(myBlock);
                if (myBlock.Figure.CuttedBlocks.Any())
                {
                    await SimulationforVirtualCuttingAndMovingFigures(myBlock, CheckersBoardBlock.MOUSE_ENTER_OPACITY);
                    _animationService.AnimateCell(myBlock, CheckersBoardBlock.MOVE_COLOR, CheckersBoardBlock.MOUSE_ENTER_OPACITY, CheckersBoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS).Wait(0);
                    await SimulationforVirtualCuttingAndMovingFigures(myBlock, CheckersBoardBlock.MOUSE_ENTER_OPACITY);
                    foreach (var item in myBlock.Figure.CuttedBlocks)
                    {
                        foreach (var nestedItem in item)
                        {
                            await SimulationforVirtualCuttingAndMovingFigures(myBlock, CheckersBoardBlock.MOUSE_ENTER_OPACITY);
                            _animationService.AnimateCell(nestedItem, CheckersBoardBlock.CUT_COLOR, CheckersBoardBlock.MOUSE_ENTER_OPACITY, CheckersBoardBlock.MOUSE_ENTER_RECTANGLE_RADIUS).Wait(0);
                            await SimulationforVirtualCuttingAndMovingFigures(myBlock, CheckersBoardBlock.MOUSE_ENTER_OPACITY);
                        }
                    }

                    await SimulationforVirtualCuttingAndMovingFigures(myBlock, CheckersBoardBlock.MOUSE_ENTER_OPACITY);
                    _animationService.AnimateCell(myBlock, myBlock.ActualColor, CheckersBoardBlock.MOUSE_LEAVE_OPACITY, CheckersBoardBlock.MOUSE_LEAVE_RECTANGLE_RADIUS).Wait(0);
                    await SimulationforVirtualCuttingAndMovingFigures(myBlock, CheckersBoardBlock.MOUSE_ENTER_OPACITY);

                }
            }
        }

        private async Task SimulationforVirtualCuttingAndMovingFigures(CheckersBoardBlock block, double dimmedOpacity)
        {
            if (block == null) return;
            double initialOpacity = block.AnimationRectangle.Opacity;
            await _animationService.AnimateOpacity(block, dimmedOpacity, 70);
            await Task.Delay(100);
            await _animationService.AnimateOpacity(block, initialOpacity, 70);
        }

        private void FindCutableBlocks(CheckersBoardBlock boardBlock)
        {
            foreach (var AddedNumber in Entities.Figures.Figure.AddedNumbersForFigure)
            {
                var opponentFigureBlock = BoardService.BoardBlocks.Where(block =>
                block.Figure != default &&
                !BoardService.IsMyTurn(block.Figure) &&
                block.Position.GetVerticalOrientation() == boardBlock.Position.GetVerticalOrientation() + AddedNumber[0] &&
                block.Position.GetHorizontalOrientation() == boardBlock.Position.GetHorizontalOrientation() + AddedNumber[1]).
                FirstOrDefault();

                var emptyBlock = BoardService.BoardBlocks.Where(block =>
                block.Figure == default &&
                block.Position.GetVerticalOrientation() == boardBlock.Position.GetVerticalOrientation() + AddedNumber[0] * 2 &&
                block.Position.GetHorizontalOrientation() == boardBlock.Position.GetHorizontalOrientation() + AddedNumber[1] * 2).
                FirstOrDefault();

                if (opponentFigureBlock != default && emptyBlock != default)
                {
                    CheckersBoardBlock isSame = default;
                    boardBlock.Figure.CuttedBlocks.ForEach(block => block.ForEach(nestBlock =>
                    {
                        if (nestBlock == emptyBlock)
                            isSame = nestBlock;
                    }));
                    if (isSame != default)
                        continue;
                    if (!BoardService.BoardBlocks.Contains(boardBlock))
                        boardBlock.Figure.CuttedBlocks.Last().Add(emptyBlock);
                    else
                        boardBlock.Figure.CuttedBlocks.Add(new List<CheckersBoardBlock>() { emptyBlock });
                    var newCheckersBoardBlock = new CheckersBoardBlock();
                    BoardService.BoardPaintedToMove.Add(emptyBlock);
                    newCheckersBoardBlock.Figure = boardBlock.Figure;
                    newCheckersBoardBlock.Position = emptyBlock.Position;
                    FindCutableBlocks(newCheckersBoardBlock);
                }
            }
        }

        private void QueenCutableBlocks(CheckersBoardBlock checkersBoardBlock)
        {
            if (checkersBoardBlock.Figure == null)
                return;

            if (checkersBoardBlock.Figure is Figure)
                return;



            var position = GetBlockPosiution(checkersBoardBlock);
        }

        private void FigureMovableBlocks(CheckersBoardBlock checkersBoardBlock)
        {
            if (checkersBoardBlock.Figure == null)
                return;

            if (checkersBoardBlock.Figure is Queen)
                return;

            if (BoardService.CutableBoardBlocks.Count != 0)
                return;

            var position = GetBlockPosiution(checkersBoardBlock);
            int row = (int)position.GetVerticalOrientation();
            int col = (int)position.GetHorizontalOrientation();
            int direction = (BoardService.Turn == Enums.Turn.Black) ? +1 : -1;

            int targetRow = row + direction;
            int leftCol = col - 1;
            int rightCol = col + 1;

            var possibleMoves = BoardService.BoardBlocks.Where(block =>
                block.Figure == null &&
                (int)block.Position.GetVerticalOrientation() == targetRow &&
                ((int)block.Position.GetHorizontalOrientation() == leftCol 
                || (int)block.Position.GetHorizontalOrientation() == rightCol)
            );

            if (!possibleMoves.Any())
                return;

            var block = possibleMoves.First();

            BoardService.MovableBoardBlocks.AddRange(possibleMoves);
            BoardService.BoardPaintedToMove.AddRange(possibleMoves);

            BoardService.BoardPaintedToMove.Add(checkersBoardBlock);

            _animationService.PaintMovableBlocks();
            _animationService.SetBlockClickedAniimation(checkersBoardBlock);
        }

        private void QueenMovableBoardBlcoks(CheckersBoardBlock checkersBoardBlock)
        {
            if (checkersBoardBlock.Figure == default)
                return;

            if (checkersBoardBlock.Figure.GetType() == typeof(Figure))
                return;
        }

        private Position GetBlockPosiution(CheckersBoardBlock checkersBoardBlock)
        {
            return checkersBoardBlock.Position;
        }

        private void RemoveFigureImageFromBoardBlock(CheckersBoardBlock boardBlock)
        {
            var Image = boardBlock.RectangleGrid.Children.OfType<Image>().FirstOrDefault();

            if (Image != null)
                boardBlock.RectangleGrid.Children.Remove(Image);
        }

        private void SetNewImageIntoBoardBlock(CheckersBoardBlock boardBlock, Image newImage)
        {
            RemoveFigureImageFromBoardBlock(boardBlock);

            if (newImage == null)
                throw new ArgumentNullException(nameof(newImage));

            boardBlock.RectangleGrid.Children.Add(newImage);
            boardBlock.Figure.SetImage(newImage);
        }
    }
}
