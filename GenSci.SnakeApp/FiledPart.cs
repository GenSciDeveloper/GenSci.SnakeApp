using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GenSci.SnakeApp
{
    public class FiledPart
    {
        private int? _gridSize;
        private FiledPart _frontPart;
        private eDirectionType _partDirection;

        public Point Position;
        public Rectangle Part { get; set; }

        public eDirectionType PartDirection
        {
            get
            {
                if (_frontPart == null)
                    return _partDirection;

                if (Position.X == _frontPart.Position.X && Position.Y > _frontPart.Position.Y)
                    return eDirectionType.Top;

                if (Position.X == _frontPart.Position.X && Position.Y < _frontPart.Position.Y)
                    return eDirectionType.Bottom;

                if (Position.X < _frontPart.Position.X && Position.Y == _frontPart.Position.Y)
                    return eDirectionType.Right;

                return eDirectionType.Left;
            }
            set
            {
                if (value != _partDirection)
                    _partDirection = value;
            }
        }

        public FiledPart(int gridSize, Brush brush)
        {
            Part = new Rectangle();
            Part.Fill = brush;
            _gridSize = gridSize;
            Panel.SetZIndex(Part, 1);
        }

        public FiledPart(int gridSize, FiledPart snakePart) 
            : this(gridSize, Brushes.Red)
        {
            _frontPart = snakePart;
        }

        public FiledPart(int gridSize)
            : this(gridSize, Brushes.Red)
        {
            _partDirection = eDirectionType.Top;
        }

        public bool SetPartPosition(int x, int y)
        {
            if (checkWall(x, y))
                return true;

            Position.X = x;
            Position.Y = y;

            Grid.SetColumn(Part, Position.X);
            Grid.SetRow(Part, Position.Y);

            return false;
        }

        private bool checkWall(int x, int y)
        {
            if (x < 0 || y < 0)
                return true;

            if (x > _gridSize - 1 || y > _gridSize - 1)
                return true;

            return false;
        }

        public bool MovePart()
        {
            Position.X = _frontPart.Position.X;
            Position.Y = _frontPart.Position.Y;

            if (checkWall(Position.X, Position.Y))
                return true;

            Grid.SetColumn(Part, Position.X);
            Grid.SetRow(Part, Position.Y);

            return false;
        }

        public bool MoveHead(eDirectionType direction)
        {
            _partDirection = direction;

            switch (direction)
            {
                case eDirectionType.Top:
                    return SetPartPosition(Position.X, --Position.Y);
                case eDirectionType.Bottom:
                    return SetPartPosition(Position.X, ++Position.Y);
                case eDirectionType.Left:
                    return SetPartPosition(--Position.X, Position.Y);
                case eDirectionType.Right:
                    return SetPartPosition(++Position.X, Position.Y);
                default:
                    return true;
            }
        }

        public bool SetTailPosition()
        {
            switch (_frontPart.PartDirection)
            {
                case eDirectionType.Top:
                    return SetPartPosition(_frontPart.Position.X, _frontPart.Position.Y);
                case eDirectionType.Bottom:
                    return SetPartPosition(_frontPart.Position.X, _frontPart.Position.Y);
                case eDirectionType.Left:
                    return SetPartPosition(_frontPart.Position.X, _frontPart.Position.Y);
                case eDirectionType.Right:
                    return SetPartPosition(_frontPart.Position.X, _frontPart.Position.Y);
                default:
                    return true;
            }
        }
    }
}
