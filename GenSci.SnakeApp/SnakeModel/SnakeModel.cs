using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GenSci.SnakeApp.SnakeModel
{
    /// <summary>
    /// Модель игры
    /// </summary>
    public class SnakeParams
    {
        private int _fieldSize = 40;
        private int _startSnakeSize = 3;
        private int _startVelocity = 200;
        
        /// <summary>
        /// Размер игрового поля
        /// </summary>
        public int FieldSize => _fieldSize;

        /// <summary>
        /// Стартовая позиция змейки
        /// </summary>
        public Point StartPosition => new Point(_fieldSize / 2, _fieldSize / 2);

        /// <summary>
        /// Стартовый размер змейки
        /// </summary>
        public int StartSnakeSize => _startSnakeSize;

        public int StartVelocity => _startVelocity;
    }
}
