using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardAlgoritme
{
    enum CellType { EMPTY, PORTAL, STORM, ICE, FOREST, WALL, STORMKEY, ICEKEY, PATH, FORESTPATH }
    class Cell
    {
        private GridManager gm;
        private Point position;
        private int cellSize;
        private Image sprite;
        private CellType myType = CellType.EMPTY;
        private int g = 0;
        private int h;
        private int f;
        private Cell parent;
        private bool walkable;
        private bool visitied;
        private List<Cell> neibourghs;

        public bool Walkable
        {
            get { return walkable; }
            set { walkable = value; }
        }
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }
        public Image Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        public CellType MyType
        {
            get { return myType; }
            set { myType = value; }
        }
        public Cell Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }
        public int G
        {
            get
            {
                return g;
            }

            set
            {
                g = value;
            }
        }
        public int H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(position.X * cellSize, position.Y * cellSize, cellSize, cellSize);
            }
        }
        public bool Visitied
        {
            get
            {
                return visitied;
            }

            set
            {
                visitied = value;
            }
        }

        internal List<Cell> Neibourghs
        {
            get
            {
                return neibourghs;
            }

            set
            {
                neibourghs = value;
            }
        }

        public Cell(Point position, int size)
        {
            this.position = position;
            this.cellSize = size;
            visitied = false;
            parent = this;

            neibourghs = new List<Cell>();
        }

        //public void FindNeibourghs(Cell centerCell)
        //{
        //    for (int x = -1; x <= 1; x++)
        //    {
        //        for (int y = -1; y <= 1; y++)
        //        {
        //            if (!(y == 0 && x == 0))
        //            {
        //                Cell gridtest = gm.Grid.Find(node => node.Position.X == centerCell.Position.X - x && node.Position.X >= 0 && node.Position.Y == centerCell.Position.Y - y && node.Position.Y >= 0
        //                && node.Position.X <= gm.CellRowCount && node.Position.Y <= gm.CellRowCount);

        //                if (gm.Grid.Exists(b => b == gridtest))
        //                {
        //                    Cell n = gm.Grid.Find(c => c.Position.X == centerCell.Position.X - x && c.Position.Y == centerCell.Position.Y - y);
        //                    if (!(n.Visitied))
        //                    {
        //                        if (n.Parent == null)
        //                        {
        //                            n.Parent = centerCell;
        //                            neibourghs.Add(n);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public void CellCheck(Wizard wiz)
        {
            if (myType == CellType.PATH)
            {
                this.walkable = true;
            }

            if (myType == CellType.EMPTY)
            {
                this.walkable = true;
            }

            if (myType == CellType.WALL)
            {
                this.walkable = false;
            }

            if (myType == CellType.FOREST)
            {
                this.walkable = false;
            }

            if (myType == CellType.FORESTPATH)
            {
                if (!this.visitied)
                {
                    this.walkable = true;
                }
                else if (this.visitied)
                {
                    this.walkable = false;
                }
                if (wiz.Position == this)
                {
                    this.visitied = true;
                    this.Sprite = Image.FromFile(@"Images\Monster.png");
                }
            }

            if (myType == CellType.STORMKEY || myType == CellType.ICEKEY)
            {
                this.walkable = true;
                if (wiz.Position == this)
                {
                    if (myType == CellType.STORMKEY)
                    {
                        wiz.Stormkey = true;
                        this.myType = CellType.EMPTY;
                        this.Sprite = Image.FromFile(@"Images\Grass.png");
                    }
                    else if (MyType == CellType.ICEKEY)
                    {
                        wiz.Icekey = true;
                        this.myType = CellType.EMPTY;
                        this.Sprite = Image.FromFile(@"Images\Grass.png");
                    }
                }
            }



            if (myType == CellType.ICE || myType == CellType.STORM)
            {
                if (wiz.Stormkey == true || wiz.Icekey == true)
                {
                    if (myType == CellType.STORM && wiz.Stormkey)
                    {
                        this.walkable = true;
                        if (wiz.Position == this)
                        {
                            wiz.HasPotion = true; // For at kunne f 
                        }
                    }
                    if (myType == CellType.ICE && wiz.HasPotion == true && wiz.Icekey)
                    {
                        this.walkable = true;
                        if (wiz.Position == this)
                        {
                            wiz.HasPotion = false;
                            wiz.CanIWinNow = true; //Win condition
                        }
                    }

                }
                else
                {
                    this.walkable = false;
                }

            }


            if (myType == CellType.PORTAL)
            {
                if (wiz.CanIWinNow)
                {
                    this.walkable = true;
                }
                else
                {
                    this.walkable = false;
                }
            }
        }

        public void Render(Graphics dc)
        {
            dc.FillRectangle(new SolidBrush(Color.White), BoundingRectangle);
            dc.DrawRectangle(new Pen(Color.Black), BoundingRectangle);
            if (sprite != null)
            {
                dc.DrawImage(sprite, BoundingRectangle);
            }
#if DEBUG
            dc.DrawString(string.Format("{0}", position), new Font("Arial", 7, FontStyle.Regular), new SolidBrush(Color.Black), position.X * cellSize, (position.Y * cellSize) + 10);
#endif


        }

        public int GetFValue(List<Cell> grid, Cell goal)
        {
            //g
            Point diff = new Point(Parent.Position.X - position.X, Parent.Position.Y - position.Y);
            if (diff.X == 1 && diff.Y == 1 || diff.X == -1 && diff.Y == 1 || diff.X == 1 && diff.Y == -1 || diff.X == -1 && diff.Y == -1) //Diagonal
            {
                g = Parent.G + 14;
            }
            else if (diff.X == 0 && diff.Y == 0)
            {
                g = Parent.G + 0;
            }
            else
            {
                g = Parent.G + 10;
            }
            //h
            diff = new Point(goal.Position.X - position.X, goal.Position.Y - position.Y);
            if (diff.X < 0)
            {
                diff.X *= -1;
            }
            if (diff.Y < 0)
            {
                diff.Y *= -1;
            }
            h = (diff.X + diff.Y) * 10;
            //f
            f = g + h;
            return f;
        }

    }
}
