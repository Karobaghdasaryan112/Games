﻿using Chess.Enums;
using Chess.Interfaces;

namespace Chess.Entities.Figures
{
    public class Bishop : FigureBase<Bishop>, IFigure
    {
        public Bishop(Chess.Enums.Color color) : base(color)
        {
        }
        public override string GetFigureName()
        {
            return typeof(Bishop).Name;
        }

        public List<BoardBlock>[] MovableBlocks(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation,Color color)
        {
            
            MoveableRectangles.Clear();
            CutableRectangles.Clear();

            int col = (int)horizontalOrientation;int row = (int)verticalOrientation;

            do
            {
                col++; row++;

                if (col == IBoardService.BOARD_SIZE || row == IBoardService.BOARD_SIZE)
                    break;

                if (!MoveCondition(row, col))
                    break;

            } while ((col < IBoardService.BOARD_SIZE && row < IBoardService.BOARD_SIZE));

            RefreshOrientations(out row, out col, verticalOrientation, horizontalOrientation);

            do
            {
                col++; row--;

                if (col == IBoardService.BOARD_SIZE || row == -1)
                    break;

                if (!MoveCondition(row, col))
                    break;

            } while (row >= 0 && col < IBoardService.BOARD_SIZE);

            RefreshOrientations(out row, out col, verticalOrientation, horizontalOrientation);

            do
            {
                col--; row++;

                if (col == -1 || row == IBoardService.BOARD_SIZE)
                    break;

                if (!MoveCondition(row, col))
                    break;

            } while ((col >= 0 && row < IBoardService.BOARD_SIZE));

            RefreshOrientations(out row, out col, verticalOrientation, horizontalOrientation);

            do
            {
                col--; row--;

                if (col == -1 || row == -1)
                    break;

                if(!MoveCondition(row, col))
                    break;

            } while (row >= 0 && col >= 0);

            return new List<BoardBlock>[] { MoveableRectangles, CutableRectangles };
        }
    }
}
