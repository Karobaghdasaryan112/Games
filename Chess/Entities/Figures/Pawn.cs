using Chess.Enums;
using Chess.Extentions;
using Chess.Interfaces;
using Chess.Services;
using System.Windows.Controls;

namespace Chess.Entities.Figures
{
    public class Pawn : FigureBase<Pawn>,IFigure
    {
        public static List<BoardBlock> MoveableRectangles = new List<BoardBlock>();
        public Pawn(Enums.Color color) : base(color)
        {
        }

        public List<BoardBlock> MovableBlocks(Grid boardGrid, VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation, int boardSize)
        {
            MoveableRectangles.Clear();
            int row = (int)verticalOrientation;
            int col = (int)horizontalOrientation;

            if (GetColor() == Enums.Color.White)
            {
                if (row == (int)VerticalOrientation.b)
                {
                    MoveableRectangles.Add(BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row + 1, (HorizontalOrientation)col)));
                    MoveableRectangles.Add(BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row + 2, (HorizontalOrientation)col)));
                }
                else
                {
                    if (row + 1 < boardSize)
                        MoveableRectangles.Add(BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row + 1, (HorizontalOrientation)col)));
                }
            }
            if (GetColor() == Enums.Color.Black)
            {
                if (row == (int)VerticalOrientation.g)
                {
                    MoveableRectangles.Add(BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row - 1, (HorizontalOrientation)col)));
                    MoveableRectangles.Add(BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row - 2, (HorizontalOrientation)col)));
                }
                else
                {
                    if (row + 1 >= 0)
                        MoveableRectangles.Add(BoardService.BoardBlocks.GetElement(new Position((VerticalOrientation)row - 1, (HorizontalOrientation)col)));
                }
            }

            return MoveableRectangles;
        }
    }
}
