using GenSci.SnakeApp.SnakeModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenSci.SnakeApp
{
    /// <summary>
    /// Класс змейки
    /// </summary>
    public class Snake
    {
        private int _currentSize;
        private List<FiledPart> _snakeParts;
        private int _fieldSize;
        private eDirectionType _directionType;

        public FiledPart Head => _snakeHead;
        private FiledPart _snakeHead => _snakeParts.First();

        public int CurrentSize => _currentSize;
        public List<FiledPart> SnakeParts => _snakeParts;

        public Snake(SnakeParams snakeParams)
        {
            _fieldSize = snakeParams.FieldSize;
            _directionType = eDirectionType.Top;
            _currentSize = snakeParams.StartSnakeSize;
            _snakeParts = new List<FiledPart>();

            var headPart = new FiledPart(snakeParams.FieldSize);
            _snakeParts.Add(headPart);

            for (int i = 0; i < _currentSize - 1; i++)
            {
                var snakePart = new FiledPart(snakeParams.FieldSize, _snakeParts.Last());
                _snakeParts.Add(snakePart);
            }
        }

        public int GetIndexPart(FiledPart part)
        {
            if (_snakeParts.Contains(part))
                return _snakeParts.IndexOf(part);

            return 0;
        }

        public bool MoveSnake()
        {
            foreach (FiledPart part in _snakeParts.Skip(1).Reverse())
            {
                part.MovePart();
            }

            return _snakeHead.MoveHead(_directionType);
        }

        public void SetDirection(eDirectionType direction)
        {
            switch (direction)
            {
                case eDirectionType.Top:
                    if (_directionType == eDirectionType.Bottom)
                        return;
                    break;
                case eDirectionType.Bottom:
                    if (_directionType == eDirectionType.Top)
                        return;
                    break;
                case eDirectionType.Left:
                    if (_directionType == eDirectionType.Right)
                        return;
                    break;
                case eDirectionType.Right:
                    if (_directionType == eDirectionType.Left)
                        return;
                    break;
                default:
                    break;
            }

            _directionType = direction;
        }

        public void GrowUp()
        {
            _currentSize++;

            FiledPart snakePart = new FiledPart(_fieldSize, _snakeParts.Last());

            _snakeParts.Add(snakePart);
            snakePart.SetTailPosition();
        }
    }
}
