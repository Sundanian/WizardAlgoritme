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
        public CellType myType = CellType.EMPTY;

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
