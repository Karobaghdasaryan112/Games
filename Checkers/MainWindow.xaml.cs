using Checkers.Interfaces;
using Checkers.Services;
using System.Windows;


namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   

    public partial class MainWindow : Window
    {
        private IBoardService _boardService;
        private IBoardBlockService _boardBlockService;
        private IAnimationService _animationService;
        private IFigureService _figureService;
        public MainWindow()
        {

            InitializeComponent();
            _animationService = new AnimationService();
            _figureService = new FigureService(_animationService);
            _boardBlockService = new BoardBlockService(_animationService, _figureService);
            _boardService = new BoardService(_boardBlockService);
            _boardService.BoardInitialize(BoardGrid, 8);
        }
    }
}