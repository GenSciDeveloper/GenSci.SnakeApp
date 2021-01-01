using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GenSci.SnakeApp.SnakeModel;

namespace GenSci.SnakeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameParams _game = new GameParams();
        SnakeParams _snakeParams = new SnakeParams();
        Snake _snake;
        Random _random = new Random();
        FiledPart _food;

        /// <summary>
        /// Основное игровое поле
        /// </summary>
        private Grid _playGround => PlayGround;

        /// <summary>
        /// Таймер движения
        /// </summary>
        private Timer _stepTimer;
        /// <summary>
        /// Скорость движения
        /// </summary>
        private int _moveVelocity;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _game;

            for (int i = 0; i < _snakeParams.FieldSize - 1; i++)
            {
                _playGround.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_playGround.Height / _snakeParams.FieldSize - 1) });
                _playGround.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_playGround.Height / _snakeParams.FieldSize - 1) });
            }            
        }


        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            clearObjects();

            _food = new FiledPart(_snakeParams.FieldSize, Brushes.Black);

            int startFoodX = _random.Next(_snakeParams.FieldSize - 1);
            int startFoodY = _random.Next(_snakeParams.FieldSize - 1);
            _playGround.Children.Add(_food.Part);
            _food.SetPartPosition(startFoodX, startFoodY);
            Panel.SetZIndex(_food.Part, 0);

            _moveVelocity = _snakeParams.StartVelocity;
            _stepTimer = new Timer(doSnakeStep, null, _moveVelocity, _moveVelocity);
        }

        private void clearObjects()
        {
            if (_stepTimer != null)
                _stepTimer.Dispose();

            _playGround.Children.Clear();
            _game.Score = 0;
            _snake = new Snake(_snakeParams);
            setStartPosition();
        }

        /// <summary>
        /// Установление стартовой позиции змейки
        /// </summary>
        private void setStartPosition()
        {
            foreach (FiledPart part in _snake.SnakeParts)
            {
                _playGround.Children.Add(part.Part);
                part.SetPartPosition(_snakeParams.StartPosition.X, _snakeParams.StartPosition.Y + _snake.GetIndexPart(part));
            }
        }

        private void moveSnake()
        {
            if (_snake.MoveSnake())
            {
                _stepTimer.Dispose();
                MessageBox.Show("Змея задушена.");
            }

            if(_snake.Head.Position == _food.Position)
            {
                int startFoodX = _random.Next(_snakeParams.FieldSize - 1);
                int startFoodY = _random.Next(_snakeParams.FieldSize - 1);
                _food.SetPartPosition(startFoodX, startFoodY);

                _snake.GrowUp();
                _playGround.Children.Add(_snake.SnakeParts.Last().Part);

                _game.Score++;

                if (_game.Score != 0 && _game.Score % 3 == 0 && _moveVelocity > 50)
                {
                    _moveVelocity -= 20;
                    _stepTimer.Change(0, _moveVelocity);
                }

            }
        }

        /// <summary>
        /// Шаг движения
        /// </summary>
        /// <param name="state"></param>
        private void doSnakeStep(object state)
        {
            Dispatcher.Invoke(() => moveSnake());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _snake.SetDirection(eDirectionType.Left);

            if (e.Key == Key.Right)
                _snake.SetDirection(eDirectionType.Right);

            if (e.Key == Key.Up)
                _snake.SetDirection(eDirectionType.Top);

            if (e.Key == Key.Down)
                _snake.SetDirection(eDirectionType.Bottom);
        }
    }
}
