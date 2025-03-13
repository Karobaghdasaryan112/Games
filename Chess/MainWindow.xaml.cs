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
        private IFigureService _figureService;
        private const int BOARD_SIZE = 8;
        public MainWindow()
        {
            InitializeComponent();

            _animationService = new AnimatonService();
            _figureService = new FigureService(_animationService);
            _boardBlockService = new BoardBlockService(_animationService, _figureService);
            _boardService = new BoardService(_boardBlockService);
            _boardService.BoardInitialize(BoardGrid, BOARD_SIZE);
        }
    }
}