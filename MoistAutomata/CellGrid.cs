using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MoistAutomata
{
    public class CellGrid
    {

        public readonly int Width;
        public readonly int Height;

        private int[,] _cellBuffer;

        private int[,] Cells;

        public CellGrid(int width, int height, CalculateCell calculateCell)
        {
            Width = width;
            Height = height;
            Cells = new int[width, height];
            _cellBuffer = new int[width, height];
            _calculateCell = calculateCell;
        }
        public delegate int CalculateCell(CellPoint point, CellGrid grid);

        CalculateCell _calculateCell;

        public int this[CellPoint point] {
            get => Cells[point.X, point.Y];
            set => Cells[point.X, point.Y] = value;
        }

        public int this[int x, int y]
        {
            get => Cells[x, y];
            set => Cells[x, y] = value;
        }

        public void InitGrid(int[,] initializer)
        {
            for(int x = 0; x < initializer.GetLength(0); x++)
            {
                for (int y = 0; y < initializer.GetLength(1); y++)
                {
                    Cells[x,y] = initializer[x,y];
                }
            }
        }

        public void Step()
        {
            CellPoint point = new CellPoint(0,0,this);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    point.X = x;
                    point.Y = y;
                    _cellBuffer[x,y] = _calculateCell(point, this);
                }
            }
            // swap buffers
            (_cellBuffer, Cells) = (Cells, _cellBuffer);
        }

    }
}