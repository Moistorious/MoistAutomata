using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace MoistAutomata
{

    public class GameLoop
    {
        public int UPSLimit = 1;
        public int FPSLimit = 1;

        long microsPerFrame = 0;
        long microsPerUpdate = 0;

        Stopwatch sw;
        long lastMicros = 0;
        CellGrid _grid;

        public GameLoop(CellGrid grid)
        {
            sw = new Stopwatch();
            sw.Start();
            _grid = grid;
            Console.CursorVisible = false;
            microsPerFrame = (FPSLimit * (1000L * 1000L));
            microsPerUpdate = (UPSLimit * (1000L * 1000L));
        }
        public void Loop()
        {
            for (; ; )
            {
                long currentMicros = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
                long delta = currentMicros - lastMicros;
                Update(delta);
                Draw(delta);
                lastMicros = currentMicros;
            }
        }

        long microsSinceBoardUpdate = 1;
        public void Update(long delta)
        {
            microsSinceBoardUpdate += delta;
            
            if (microsSinceBoardUpdate >= microsPerUpdate)
            {
                microsSinceBoardUpdate = 0;
                _grid.Step();
            }
        }

        long microsSinceDraw = 0;
        public void Draw(long delta)
        {
            microsSinceDraw += delta;

            if (microsSinceDraw >= microsPerFrame)
            {
                microsSinceDraw = 0;
                DrawGrid(_grid);
            }
        }

        void DrawGrid(CellGrid grid)
        {
            char mychar = (char) 0x2588;

        }

        public void Pause()
        {
            sw.Stop();
        }
    }
}
