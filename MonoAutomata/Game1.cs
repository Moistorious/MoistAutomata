using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoistAutomata;
using System;
using System.Linq;

namespace MonoAutomata
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CellGrid grid;
        private KeyboardStates _keyboard = new KeyboardStates();

        bool _running = true;

        int cellSize = 10;
        int Width = 100;
        int Height = 50;
        Texture2D whiteRectangle;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = cellSize * Width;
            _graphics.PreferredBackBufferHeight = cellSize * Height;
            _graphics.ApplyChanges();

            grid = new CellGrid(Width, Height, GameOfLife);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            grid.InitGrid(new int[,]{
                { 1, 0, 0, },
                { 0, 1, 1, },
                { 1, 1, 0, },
            });

        }

        int GameOfLife(CellPoint point, CellGrid grid)
        {
            int neighbours =
                point.Up().Left() + point.Up() + point.Up().Right() +
                point.Left() + point.Right() +
                point.Down().Left() + point.Down() + point.Down().Right();

            return point == 1 ?
                neighbours < 2 ? 0  // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                : neighbours >= 2 && neighbours <= 3 ? 1 // Any live cell with two or three live neighbours lives on to the next generation.
                : 0 // Any live cell with more than three live neighbours dies, as if by overpopulation.
            : neighbours == 3 ? 1 : 0; // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a 1px square rectangle texture that will be scaled to the
            // desired size and tinted the desired color at draw time
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            // TODO: use this.Content to load your game content here
        }

        public (int x, int y) PointToGrid(int x, int y) => ((int)Math.Floor((float)x / cellSize), (int)Math.Floor((float)y / cellSize));


        double ElapsedSinceLastUpdate = 0;
        protected override void Update(GameTime gameTime)
        {

            _keyboard.UpdateState();

            if (_keyboard.KeyPressed(Keys.Escape))
                Exit();

            if (_keyboard.KeyPressed(Keys.Space))
                _running = !_running;

            var mouseState = Mouse.GetState();

            if (mouseState.X >= 0 && mouseState.X < Window.ClientBounds.Width &&
                mouseState.Y >= 0 && mouseState.Y < Window.ClientBounds.Height)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    grid[PointToGrid(mouseState.X, mouseState.Y)] = 1;
                }

                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    grid[PointToGrid(mouseState.X, mouseState.Y)] = 0;
                }

            }
            if (_running)
            {
                ElapsedSinceLastUpdate += gameTime.ElapsedGameTime.TotalSeconds;

                if (ElapsedSinceLastUpdate > .1)
                {
                    grid.Step();
                    ElapsedSinceLastUpdate = 0;
                }

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();

            // draw cells
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid[x, y] == 0) continue;
                    _spriteBatch.Draw(whiteRectangle, new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize), Color.Black);
                }
            }

            // draw grid lines
            for (int y = 0; y < grid.Height; y++)
            {
                _spriteBatch.Draw(whiteRectangle, new Rectangle(0, y * cellSize, cellSize * Width, 1), Color.Gray);
                for (int x = 0; x < grid.Width; x++)
                {
                    _spriteBatch.Draw(whiteRectangle, new Rectangle(x * cellSize, 0, 1, cellSize * Height), Color.Gray);
                }
            }
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}