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
        private Wizard wizard;

        public List<Cell> Grid
        {
            get { return grid; }
            set { grid = value; }
        }

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

            wizard.GetNextMove().Sprite = Image.FromFile(@"Images\test.png");
        }

        private void Render()
        {
            dc.Clear(Color.Green);

            foreach (Cell cell in grid)
            {
                cell.Render(dc);
            }
            wizard.Render(dc);

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
            SetUpCells();
            Cell wStartCell = null;
            foreach (Cell cell in grid)
            {
                if (cell.Position == new Point(1, 8))
                {
                    wStartCell = cell;
                }
            }
            wizard = new Wizard(wStartCell, this);
        }

        private void SetUpCells()
        {
            //Creates the portal
            Cell portal = grid.Find(node => node.Position.X == 0 && node.Position.Y == 8);
            portal.MyType = CellType.PORTAL;
            portal.Walkable = true;
            portal.Sprite = Image.FromFile(@"Images\Portal.png");

            //Creates the ice tower
            Cell iceTower = grid.Find(node => node.Position.X == 2 && node.Position.Y == 4);
            iceTower.MyType = CellType.ICE;
            iceTower.Walkable = false;
            iceTower.Sprite = Image.FromFile(@"Images\Ice_Castle.png");

            //Creates the storm tower
            Cell stormTower = grid.Find(node => node.Position.X == 8 && node.Position.Y == 7);
            stormTower.MyType = CellType.STORM;
            stormTower.Walkable = false;
            stormTower.Sprite = Image.FromFile(@"Images\Lighting_Castle.png");

            //Creates the Rocks
            for (int x = 4; x < 7; x++)
            {
                for (int y = 1; y < 7; y++)
                {
                    Cell wall = grid.Find(node => node.Position.X == x && node.Position.Y == y);

                    if (wall.MyType != CellType.WALL)
                    {
                        wall.MyType = CellType.WALL;
                        wall.Walkable = false;
                        wall.Sprite = Image.FromFile(@"Images\Rock.png");

                    }
                }
            }

            //Creates the trees
            for (int forestX = 2; forestX < 7; forestX++)
            {
                for (int forestY = 7; forestY < 10; forestY++)
                {
                    if (forestY == 7 || forestY == 9)
                    {
                        Cell forest = grid.Find(node => node.Position.X == forestX && node.Position.Y == forestY);
                        forest.MyType = CellType.FOREST;
                        forest.Walkable = false;
                        forest.Sprite = Image.FromFile(@"Images\Tree.png");

                    }
                }
            }

            //Creates the path
            Cell p1 = grid.Find(node => node.Position.X == 1 && node.Position.Y == 8);
            Cell p2 = grid.Find(node => node.Position.X == 1 && node.Position.Y == 7);
            Cell p3 = grid.Find(node => node.Position.X == 1 && node.Position.Y == 6);

            Cell p4 = grid.Find(node => node.Position.X == 1 && node.Position.Y == 5);
            Cell p5 = grid.Find(node => node.Position.X == 2 && node.Position.Y == 5);
            Cell p6 = grid.Find(node => node.Position.X == 3 && node.Position.Y == 5);

            Cell p7 = grid.Find(node => node.Position.X == 3 && node.Position.Y == 4);
            Cell p8 = grid.Find(node => node.Position.X == 3 && node.Position.Y == 3);
            Cell p9 = grid.Find(node => node.Position.X == 3 && node.Position.Y == 2);
            Cell p10 = grid.Find(node => node.Position.X == 3 && node.Position.Y == 1);
            Cell p11 = grid.Find(node => node.Position.X == 3 && node.Position.Y == 0);

            Cell p12 = grid.Find(node => node.Position.X == 4 && node.Position.Y == 0);
            Cell p13 = grid.Find(node => node.Position.X == 5 && node.Position.Y == 0);
            Cell p14 = grid.Find(node => node.Position.X == 6 && node.Position.Y == 0);
            Cell p15 = grid.Find(node => node.Position.X == 7 && node.Position.Y == 0);

            Cell p16 = grid.Find(node => node.Position.X == 7 && node.Position.Y == 1);
            Cell p17 = grid.Find(node => node.Position.X == 7 && node.Position.Y == 2);
            Cell p18 = grid.Find(node => node.Position.X == 7 && node.Position.Y == 3);
            Cell p19 = grid.Find(node => node.Position.X == 7 && node.Position.Y == 4);
            Cell p20 = grid.Find(node => node.Position.X == 7 && node.Position.Y == 5);

            Cell p21 = grid.Find(node => node.Position.X == 8 && node.Position.Y == 5);
            Cell p22 = grid.Find(node => node.Position.X == 8 && node.Position.Y == 6);
            Cell p23 = grid.Find(node => node.Position.X == 8 && node.Position.Y == 8);

            Cell p24 = grid.Find(node => node.Position.X == 7 && node.Position.Y == 8);
            Cell p25 = grid.Find(node => node.Position.X == 6 && node.Position.Y == 8);
            Cell p26 = grid.Find(node => node.Position.X == 5 && node.Position.Y == 8);
            Cell p27 = grid.Find(node => node.Position.X == 4 && node.Position.Y == 8);
            Cell p28 = grid.Find(node => node.Position.X == 3 && node.Position.Y == 8);
            Cell p29 = grid.Find(node => node.Position.X == 2 && node.Position.Y == 8);

            p1.MyType = CellType.PATH;
            p1.Walkable = true;
            p1.Sprite = Image.FromFile(@"Images\Path.png");

            p2.MyType = CellType.PATH;
            p2.Walkable = true;
            p2.Sprite = Image.FromFile(@"Images\Path.png");

            p3.MyType = CellType.PATH;
            p3.Walkable = true;
            p3.Sprite = Image.FromFile(@"Images\Path.png");

            p4.MyType = CellType.PATH;
            p4.Walkable = true;
            p4.Sprite = Image.FromFile(@"Images\Path.png");

            p5.MyType = CellType.PATH;
            p5.Walkable = true;
            p5.Sprite = Image.FromFile(@"Images\Path.png");

            p6.MyType = CellType.PATH;
            p6.Walkable = true;
            p6.Sprite = Image.FromFile(@"Images\Path.png");

            p7.MyType = CellType.PATH;
            p7.Walkable = true;
            p7.Sprite = Image.FromFile(@"Images\Path.png");

            p8.MyType = CellType.PATH;
            p8.Walkable = true;
            p8.Sprite = Image.FromFile(@"Images\Path.png");

            p9.MyType = CellType.PATH;
            p9.Walkable = true;
            p9.Sprite = Image.FromFile(@"Images\Path.png");

            p10.MyType = CellType.PATH;
            p10.Walkable = true;
            p10.Sprite = Image.FromFile(@"Images\Path.png");

            p11.MyType = CellType.PATH;
            p11.Walkable = true;
            p11.Sprite = Image.FromFile(@"Images\Path.png");

            p12.MyType = CellType.PATH;
            p12.Walkable = true;
            p12.Sprite = Image.FromFile(@"Images\Path.png");

            p13.MyType = CellType.PATH;
            p13.Walkable = true;
            p13.Sprite = Image.FromFile(@"Images\Path.png");

            p14.MyType = CellType.PATH;
            p14.Walkable = true;
            p14.Sprite = Image.FromFile(@"Images\Path.png");

            p15.MyType = CellType.PATH;
            p15.Walkable = true;
            p15.Sprite = Image.FromFile(@"Images\Path.png");

            p16.MyType = CellType.PATH;
            p16.Walkable = true;
            p16.Sprite = Image.FromFile(@"Images\Path.png");

            p17.MyType = CellType.PATH;
            p17.Walkable = true;
            p17.Sprite = Image.FromFile(@"Images\Path.png");

            p18.MyType = CellType.PATH;
            p18.Walkable = true;
            p18.Sprite = Image.FromFile(@"Images\Path.png");

            p19.MyType = CellType.PATH;
            p19.Walkable = true;
            p19.Sprite = Image.FromFile(@"Images\Path.png");

            p20.MyType = CellType.PATH;
            p20.Walkable = true;
            p20.Sprite = Image.FromFile(@"Images\Path.png");

            p21.MyType = CellType.PATH;
            p21.Walkable = true;
            p21.Sprite = Image.FromFile(@"Images\Path.png");

            p22.MyType = CellType.PATH;
            p22.Walkable = true;
            p22.Sprite = Image.FromFile(@"Images\Path.png");

            p23.MyType = CellType.PATH;
            p23.Walkable = true;
            p23.Sprite = Image.FromFile(@"Images\Path.png");

            p24.MyType = CellType.PATH;
            p24.Walkable = true;
            p24.Sprite = Image.FromFile(@"Images\Path.png");

            p25.MyType = CellType.PATH;
            p25.Walkable = true;
            p25.Sprite = Image.FromFile(@"Images\Path.png");

            p26.MyType = CellType.PATH;
            p26.Walkable = true;
            p26.Sprite = Image.FromFile(@"Images\Path.png");

            p27.MyType = CellType.PATH;
            p27.Walkable = true;
            p27.Sprite = Image.FromFile(@"Images\Path.png");

            p28.MyType = CellType.PATH;
            p28.Walkable = true;
            p28.Sprite = Image.FromFile(@"Images\Path.png");

            p29.MyType = CellType.PATH;
            p29.Walkable = true;
            p29.Sprite = Image.FromFile(@"Images\Path.png");

            foreach (Cell item in grid)
            {
                if (item.MyType == CellType.EMPTY)
                {
                    item.Sprite = Image.FromFile(@"Images\Grass.png");
                }
            }
            //Creates the key
            Random rnd = new Random();
            int rndtal = rnd.Next(0, grid.Count);
            Cell key = grid[rndtal];

            while (key.MyType != CellType.EMPTY)
            {
                rndtal = rnd.Next(0, grid.Count);
                key = grid[rndtal];
            }

            key.MyType = CellType.KEY;
            key.Walkable = true;
            key.Sprite = Image.FromFile(@"Images\Key.png");
        }
    }
}
