using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoistAutomata
{
    public class CellPoint
    {
        private Point _point;
        private CellGrid _grid;

        public int X { get => _point.X; set => _point.X = value; }
        public int Y { get => _point.Y; set => _point.Y = value; }

        public CellPoint(int X, int Y, CellGrid grid)
        {
            Point x = new Point(X, Y);
            _point = new Point(X, Y);
            _grid = grid;
        }

        public static implicit operator int(CellPoint point) => point._grid[point];

        public CellPoint Up()
        {
            return new CellPoint(_point.X, _point.Y <= 0 ? _grid.Height - 1 : _point.Y - 1, _grid);
        }

        public CellPoint Down()
        {
            return new CellPoint(_point.X, _point.Y >= _grid.Height - 1 ? 0 : _point.Y + 1, _grid);
        }
        public CellPoint Left()
        {
            return new CellPoint(_point.X <= 0 ? _grid.Width - 1 : _point.X - 1, _point.Y, _grid);
        }

        public CellPoint Right()
        {
            return new CellPoint(_point.X >= _grid.Width - 1 ? 0 : _point.X + 1, _point.Y, _grid);
        }

    }
}
