using Chess.Entities;
using Chess.Interfaces;
using System.Windows;

namespace Chess
{
    public partial class MainWindow : Window
    {
        private IBoard Board;
        public MainWindow()
        {
            InitializeComponent();
            Board = new Board();
            Board.CreateStartPositions(BoardGrid);
        }
    }
}