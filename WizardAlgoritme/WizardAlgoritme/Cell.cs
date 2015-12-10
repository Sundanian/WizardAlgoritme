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

        public List<Cell> Neibourghs
        {
            get { return neibourghs; }
            set { neibourghs = value; }
        }
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
                return h    ;
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

        public Cell(Point position, int size)
        {
            this.position = position;
            this.cellSize = size;
            visitied = false;
            parent = this;

            neibourghs = new List<Cell>();
        }

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
            dc.DrawString(string.Format("F {0} \nG {1} \nH {2}", f, g, h, parent.Position), new Font("Arial", 7, FontStyle.Regular), new SolidBrush(Color.Red), position.X * cellSize, (position.Y * cellSize) + 20);
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
            diff = new Point(Math.Abs(goal.Position.X - position.X), Math.Abs(goal.Position.Y - position.Y));
            h = (diff.X + diff.Y) * 10;
            //f
            f = g + h;
            return f;
        }

    }
}
