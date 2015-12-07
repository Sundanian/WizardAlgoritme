using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardAlgoritme
{
    enum CellType { EMPTY, PORTAL, STORM, ICE, FOREST, WALL, KEY, PATH, FORESTPATH }
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

        public Cell(Point position, int size)
        {
            this.position = position;
            this.cellSize = size;
            visitied = false;
        }

        public void CellCheck(Wizard wiz)
        {
            if (wiz.Position.position == this.position)
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
                    else if(this.visitied)
                    {
                        this.walkable = false;
                    }

                    this.visitied = true;
                }

                if (myType == CellType.KEY)
                {
                    this.walkable = true;
                    wiz.Key += 1;
                    this.myType = CellType.EMPTY;
                }

                if (myType == CellType.ICE || myType == CellType.STORM)
                {
                    if (wiz.Key > 0)
                    {
                        if (myType == CellType.STORM)
                        {
                            this.walkable = true;
                            wiz.HasPotion = true; // For at kunne f
                            Console.WriteLine("You now have a potion!"); //For fun and giggles
                        }
                        if (myType == CellType.ICE && wiz.HasPotion == true)
                        {
                            this.walkable = true;
                            wiz.HasPotion = false;
                            wiz.CanIWinNow = true; //Win condition
                            Console.WriteLine("You have delivered the potion!"); //For fun and giggles
                        }
                        wiz.Key -= 1;
                        this.walkable = false;
                    }
                    else
                    {
                        this.walkable = false;

                        if (myType == CellType.STORM)
                        {
                            Console.WriteLine("You need a key to enter!"); //For fun and giggles
                        }
                        if (myType == CellType.ICE && wiz.HasPotion == false)
                        {
                            Console.WriteLine("You need a potion from the Storm Tower and a key to enter!"); //For fun and giggles
                        }
                    }
                }

                if (myType == CellType.PORTAL)
                {
                    if (wiz.CanIWinNow)
                    {
                        this.walkable = true;
                        Console.WriteLine("Win win!"); //For fun and giggles
                    }
                    else
                    {
                        this.walkable = false;
                        Console.WriteLine("You shall not win yet!"); //For fun and giggles
                    }
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
