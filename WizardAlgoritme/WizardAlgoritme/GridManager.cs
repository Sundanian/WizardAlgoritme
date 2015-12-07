using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardAlgoritme
{
    class GridManager
    {
        private BufferedGraphics backBuffer;
        private Graphics dc;
        private Rectangle displayRectangle;

        private int cellRowCount;

        private List<Cell> grid;

        //private static Cell iceTower;
        //private static Cell stormTower;
        //private static Cell portal;
        //private static Cell forest;
        //private static Cell wall;
        //private static Cell key;
        //private static Cell path;

        public GridManager(Graphics dc, Rectangle displayRectangle)
        {
            this.backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;
            this.displayRectangle = displayRectangle;

            cellRowCount = 10;


            SetupWorld();
        }

        public void GameLoop()
        {
            Render();
        }

        private void Render()
        {
            dc.Clear(Color.White);

            foreach (Cell cell in grid)
            {
                cell.Render(dc);
            }

            backBuffer.Render();
        }

        private void CreateGrid()
        {
            grid = new List<Cell>();

            int cellSize = displayRectangle.Width / cellRowCount;

            for (int x = 0; x < cellRowCount; x++)
            {
                for (int y = 0; y < cellRowCount; y++)
                {
                    grid.Add(new Cell(new Point(x, y), cellSize));
                }
            }
        }
        private void SetupWorld()
        {
            CreateGrid();

            Cell portal = grid.Find(node => node.Position.X == 0 && node.Position.Y == 8);
            portal.myType = CellType.PORTAL;
            portal.Walkable = true;
            portal.Sprite = Image.FromFile(@"Images\test.png");

            Cell iceTower = grid.Find(node => node.Position.X == 2 && node.Position.Y == 4);
            iceTower.myType = CellType.ICE;
            iceTower.Walkable = false;
            iceTower.Sprite = Image.FromFile(@"Images\test.png");


            Cell stormTower = grid.Find(node => node.Position.X == 8 && node.Position.Y == 7);
            stormTower.myType = CellType.STORM;
            stormTower.Walkable = false;
            stormTower.Sprite = Image.FromFile(@"Images\test.png");


            for (int x = 4; x < 6; x++)
            {
                for (int y = 1; y < 7; y++)
                {
                    Cell wall = grid.Find(node => node.Position.X == x && node.Position.Y == y);

                    if (wall.myType != CellType.WALL)
                    {
                        wall.myType = CellType.WALL;
                        wall.Walkable = false;
                        wall.Sprite = Image.FromFile(@"Images\test.png");

                    }
                }
            }

            Random rnd = new Random();
            int rndtal = rnd.Next(0, grid.Count);

            Cell key = grid[rndtal];

            while (key.myType != CellType.EMPTY)
            {
                rndtal = rnd.Next(0, grid.Count);
            }

            key.myType = CellType.KEY;
            key.Walkable = true;
            key.Sprite = Image.FromFile(@"Images\test.png");



        }
    }
}
