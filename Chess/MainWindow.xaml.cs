using Chess.Entities;
using Chess.Interfaces;
using Chess.Services;
using System.Windows;

namespace Chess
{
    public partial class MainWindow : Window
    {
        private IBoard _board;
        private IAnimationService _animationService;
        private IBoardService _boardService;
        private IBoardBlockService _boardBlockService;
        public MainWindow()
        {
            InitializeComponent();
            //Inject Dependencies
            _animationService = new AnimatonService();
            _boardBlockService = new BoardBlockService(_animationService);
            _boardService = new BoardService(_boardBlockService);

            _boardService.BoardInitialize(BoardGrid, 8);
        }
    }
}