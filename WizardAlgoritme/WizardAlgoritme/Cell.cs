using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardAlgoritme
{
    enum CellType { EMPTY, PORTAL, STORM, ICE, FOREST, WALL, KEY, PATH }
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

        public Cell(Point position, int size)
        {
            this.position = position;
            this.cellSize = size;
        }

        public void CellCheck(Point wizPos)
        {
            int key = 0; // Skal flyttes ind på Wizard
            bool hasPotion = false;
            if (wizPos == this.position)
            {
                if (myType == CellType.PATH)
                {
                    this.Walkable = true;
                }

                if (myType == CellType.EMPTY)
                {
                    this.Walkable = true;
                }

                if (myType == CellType.WALL)
                {
                    this.Walkable = false;
                }

                if (myType == CellType.FOREST && this.Walkable == true)
                {

                    this.Walkable = false;
                }

                if (myType == CellType.KEY)
                {
                    this.Walkable = true;
                    key += 1;
                    this.myType = CellType.EMPTY;
                }

                if (myType == CellType.ICE || myType == CellType.STORM)
                {
                    if (key > 0)
                    {
                        if (myType == CellType.STORM)
                        {
                            this.Walkable = true;
                            hasPotion = true;
                            Console.WriteLine("You now have a potion!"); //For fun and giggles
                        }
                        if (myType == CellType.ICE && hasPotion == true)
                        {
                            this.Walkable = true;
                            hasPotion = false;
                            Console.WriteLine("You have delivered the potion!"); //For fun and giggles
                        }
                        key -= 1;
                        this.Walkable = false;
                    }
                    else
                    {
                        this.walkable = false;
                        Console.WriteLine("You need a key to enter!"); //For fun and giggles
                    }
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
