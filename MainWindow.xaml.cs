using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[] 
        {
           new BitmapImage(new Uri("ASSETS/TileEmpty.png", UriKind.Relative)),
           new BitmapImage(new Uri("ASSETS/TileCyan.png", UriKind.Relative)),
           new BitmapImage(new Uri("ASSETS/TileBlue.png", UriKind.Relative)),
           new BitmapImage(new Uri("ASSETS/TileOrange.png", UriKind.Relative)),
           new BitmapImage(new Uri("ASSETS/TileYellow.png", UriKind.Relative)),
           new BitmapImage(new Uri("ASSETS/TileGreen.png", UriKind.Relative)),
           new BitmapImage(new Uri("ASSETS/TilePurple.png", UriKind.Relative)),
           new BitmapImage(new Uri("ASSETS/TileRed.png", UriKind.Relative)),



        };

        private readonly ImageSource[] BlockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("ASSETS/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("ASSETS/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("ASSETS/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("ASSETS/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("ASSETS/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("ASSETS/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("ASSETS/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("ASSETS/Block-Z.png", UriKind.Relative)),



        };
        private readonly Image[,] imageControls;
        private GameState gameState=new GameState();





        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);

        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++) 
            
            { 
                for(int c = 0; c < grid.Columns; c++) 
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r,c] = imageControl;

                
                }
               
            }
            return imageControls;



        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0;r < grid.Rows; r++)
            {
                for(int c =0; c < grid.Columns; c++)
                {
                    int id = grid[r,c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];



                }
            }

        }
        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Col].Opacity = 1;
                imageControls[p.Row, p.Col].Source = tileImages[block.Id];
            }

        }
        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = BlockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBLock)
        {
            if (heldBLock == null)
            {
                HoldImage.Source = BlockImages[0];

            }
            else
            {
                HoldImage.Source = BlockImages[heldBLock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BLockDropDistance();
            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Col].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Col].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue); 

            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";


        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                await Task.Delay(500);
                gameState.MoveBlockDown();
                Draw(gameState);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        { 
            if(gameState.GameOver)
            {
                return;

            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down: 
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock(); break;
                default:
                    return;






            }
            Draw(gameState);

        
        
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();


        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();


        }

    }

}