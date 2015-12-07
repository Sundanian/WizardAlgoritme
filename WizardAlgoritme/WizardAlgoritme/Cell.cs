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

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        private int cellSize;
        private Image sprite;

        public Image Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        public CellType myType = CellType.EMPTY;

        private bool walkable;

        public bool Walkable
        {
            get { return walkable; }
            set { walkable = value; }
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

        public void Walk(Point wizPos)
        {
            int key = 0; // Skal flyttes ind på Wizard

            if (myType == CellType.FOREST)
            {
                if (wizPos == this.position)
                {
                    this.Walkable = false;
                }
            }

            if (myType == CellType.KEY)
            {
                if (wizPos == this.position)
                {
                    key += 1;
                    this.myType = CellType.EMPTY;
                }
            }

            if (myType == CellType.ICE || myType == CellType.STORM)
            {
                if (wizPos == this.position)
                {
                    if (key > 0)
                    {
                        key -= 1;
                        this.Walkable = false;
                    }
                    else
                    {
                        Console.WriteLine("You need a key to enter!"); //For fun and giggles
                    }
                }
            }

            if (myType == CellType.WALL)
            {
                this.Walkable = false;
            }

            if (myType == CellType.PATH)
            {
                this.Walkable = true;
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
    }
}
