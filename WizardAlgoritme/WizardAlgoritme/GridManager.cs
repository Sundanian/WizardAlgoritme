﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardAlgoritme
{
    class GridManager
    {
        private int cellRowCount;

        private BufferedGraphics backBuffer;
        private Graphics dc;
        private Rectangle displayRectangle;
        private List<Cell> grid;
        private List<Cell> goals = new List<Cell>();
        private Wizard wizard;
        private Cell wStartCell = null;
        private int algorithm;

        public Wizard Wizard
        {
            get { return wizard; }
            set { wizard = value; }
        }
        public List<Cell> Goals
        {
            get { return goals; }
            set { goals = value; }
        }
        public List<Cell> Grid
        {
            get { return grid; }
            set { grid = value; }
        }
        public int CellRowCount
        {
            get
            {
                return cellRowCount;
            }

            set
            {
                cellRowCount = value;
            }
        }

        public GridManager(Graphics dc, Rectangle displayRectangle, int algorithm)
        {
            this.algorithm = algorithm;
            this.backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;
            this.displayRectangle = displayRectangle;

            cellRowCount = 10;

            SetupWorld();
        }

        public void GameLoop()
        {
            Render();

            goals.Clear();

            foreach (Cell cell in grid)
            {
                cell.CellCheck(wizard);

                if (cell.MyType == CellType.PORTAL)
                {
                    goals.Add(cell);
                }
                else if (cell.MyType == CellType.ICE)
                {
                    goals.Add(cell);
                }
                else if (cell.MyType == CellType.STORM)
                {
                    goals.Add(cell);
                }
                else if (cell.MyType == CellType.STORMKEY)
                {
                    goals.Add(cell);
                }
                else if (cell.MyType == CellType.ICEKEY)
                {
                    goals.Add(cell);
                }

            }
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

            foreach (Cell item in grid)
            {
                FindNeibourghs(item);
            }
        }

        public void FindNeibourghs(Cell centerCell)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (!(y == 0 && x == 0))
                    {
                        if (algorithm > 1)
                        {
                            if (!(Math.Abs(x) == 1 && Math.Abs(y) == 1))
                            {
                                Cell gridtest = grid.Find(node => node.Position.X == centerCell.Position.X - x && node.Position.X >= 0 && node.Position.Y == centerCell.Position.Y - y && node.Position.Y >= 0
                                && node.Position.X <= cellRowCount && node.Position.Y <= cellRowCount);

                                if (grid.Exists(b => b == gridtest))
                                {
                                    Cell n = grid.Find(c => c.Position.X == centerCell.Position.X - x && c.Position.Y == centerCell.Position.Y - y);
                                    centerCell.Neibourghs.Add(n);
                                }
                            }
                        }
                        else
                        {
                            Cell gridtest = grid.Find(node => node.Position.X == centerCell.Position.X - x && node.Position.X >= 0 && node.Position.Y == centerCell.Position.Y - y && node.Position.Y >= 0
                            && node.Position.X <= cellRowCount && node.Position.Y <= cellRowCount);

                            if (grid.Exists(b => b == gridtest))
                            {
                                Cell n = grid.Find(c => c.Position.X == centerCell.Position.X - x && c.Position.Y == centerCell.Position.Y - y);
                                centerCell.Neibourghs.Add(n);
                            }
                        }
                    }
                }
            }
        }

        private void SetUpCells()
        {
            List<Cell> emptylist = new List<Cell>();
            
            //Creates the portal
            Cell portal = grid.Find(node => node.Position.X == 0 && node.Position.Y == 8);
            portal.MyType = CellType.PORTAL;
            portal.Walkable = true;
            portal.Sprite = Image.FromFile(@"Images\Portal.png");

            //Creates the ice tower
            Cell iceTower = grid.Find(node => node.Position.X == 8 && node.Position.Y == 7);
            iceTower.MyType = CellType.ICE;
            iceTower.Walkable = false;
            iceTower.Sprite = Image.FromFile(@"Images\Ice_Castle.png");

            //Creates the storm tower
            Cell stormTower = grid.Find(node => node.Position.X == 2 && node.Position.Y == 4);
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

                        if (forest.MyType != CellType.FOREST)
                        {
                            forest.MyType = CellType.FOREST;
                            forest.Walkable = false;
                            forest.Sprite = Image.FromFile(@"Images\ForestPath.png");
                        }

                    }
                }
            }

            //Creates the path
            #region Path
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
            p2.MyType = CellType.PATH;
            p3.MyType = CellType.PATH;
            p4.MyType = CellType.PATH;
            p5.MyType = CellType.PATH;
            p6.MyType = CellType.PATH;
            p7.MyType = CellType.PATH;
            p8.MyType = CellType.PATH;
            p9.MyType = CellType.PATH;
            p10.MyType = CellType.PATH;
            p11.MyType = CellType.PATH;
            p12.MyType = CellType.PATH;
            p13.MyType = CellType.PATH;
            p14.MyType = CellType.PATH;
            p15.MyType = CellType.PATH;
            p16.MyType = CellType.PATH;
            p17.MyType = CellType.PATH;
            p18.MyType = CellType.PATH;
            p19.MyType = CellType.PATH;
            p20.MyType = CellType.PATH;
            p21.MyType = CellType.PATH;
            p22.MyType = CellType.PATH;
            p23.MyType = CellType.PATH;

            p24.MyType = CellType.PATH;
            p25.MyType = CellType.FORESTPATH;
            p26.MyType = CellType.FORESTPATH;
            p27.MyType = CellType.FORESTPATH;
            p28.MyType = CellType.FORESTPATH;
            p29.MyType = CellType.FORESTPATH;
            #endregion

            //Finds the empty celltypes and gives them images, and adds them to a list so we can place the keys
            //and adds an image to the path types
            foreach (Cell item in grid)
            {
                if (item.MyType == CellType.EMPTY)
                {
                    item.Sprite = Image.FromFile(@"Images\Grass.png");
                    item.Walkable = true;
                    emptylist.Add(item);
                }
                if (item.MyType == CellType.PATH)
                {
                    item.Walkable = true;
                    item.Sprite = Image.FromFile(@"Images\Path.png");
                }
                if (item.MyType == CellType.FORESTPATH)
                {
                    item.Walkable = true;
                    item.Sprite = Image.FromFile(@"Images\DarkDirt.jpg");
                }
                if (item.Position == new Point(1, 8))
                {
                    wStartCell = item;
                }


            }
            //Creates the Wizard
            wizard = new Wizard(wStartCell, this);

            //Creates the keys
            Random rnd = new Random();

            int rndtal = rnd.Next(0, emptylist.Count);
            int rndtal2 = rnd.Next(0, emptylist.Count);

            Cell key = emptylist[rndtal];
            Cell key2 = emptylist[rndtal2];

            key.MyType = CellType.STORMKEY;
            key.Walkable = true;
            key.Sprite = Image.FromFile(@"Images\Key.png");

            key2.MyType = CellType.ICEKEY;
            key2.Walkable = true;
            key2.Sprite = Image.FromFile(@"Images\Key2.png");
        }
    }
}
