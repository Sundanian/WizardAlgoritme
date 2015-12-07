using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardAlgoritme
{
    class Wizard
    {
        List<Cell> openList;
        List<Cell> closedList;
        Cell position;
        GridManager gridManager;
        int keycounter;
        Cell nextGoal;
        bool hasPotion = false;
        bool icanwin = false;

        public Wizard(Cell position, GridManager gridManager)
        {
            this.position = position;
            this.gridManager = gridManager;
        }

        private Cell ShoppingList()
        {
            foreach (Cell cell in gridManager.Goals)
            {
                if (keycounter == 0)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.KEY);
                    return nextGoal;
                }
                else if (keycounter >= 1 && hasPotion == false)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.STORM);
                    return nextGoal;
                }
                else if (keycounter >= 1 && hasPotion == true)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.ICE);
                    return nextGoal;
                }
                else if (hasPotion == false && icanwin == true)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.PORTAL);
                    return nextGoal;
                }
            }
            return null; 
        }

        private Cell Astar(Cell goal)
        {
            openList = new List<Cell>();
            closedList = new List<Cell>();

            //Finds and adds the start to the open list
            Cell start = position;
            openList.Add(start);

            //Loop
            bool endLoop = false;
            do
            {
                // finds the cell with the lowest f, so we have something to work from
                Cell chosenCell = openList[0]; // the cell to move to the closed list
                foreach (Cell cell in openList) // runs through our open list
                {
                    if (cell.GetFValue(gridManager.Grid, goal) < chosenCell.GetFValue(gridManager.Grid, goal)) // checks which cell has the lowest f
                    {
                        chosenCell = cell;
                    }
                }

                //moves the cell
                closedList.Add(chosenCell);
                openList.Remove(chosenCell);

                //finds the neibourghs
                List<Cell> neibourghs = new List<Cell>();
                List<Cell> falseNeibourghs = new List<Cell>();
                foreach (Cell cell in gridManager.Grid)
                {
                    Point diff = new Point(cell.Position.X - chosenCell.Position.X, cell.Position.Y - chosenCell.Position.Y); //diff
                    if (diff.X < 0)
                    {
                        diff.X *= -1;
                    }
                    if (diff.Y < 0)
                    {
                        diff.Y *= -1;
                    }
                    if (diff.X <= 1 && diff.Y <= 1) // if the cell is a actual neibourgh
                    {
                        if (cell.Walkable == false)
                        {
                            #region Diagonal
                            if (cell.Position == new Point(start.Position.X - 1, start.Position.Y - 1) || cell.Position == new Point(start.Position.X + 1, start.Position.Y + 1) || cell.Position == new Point(start.Position.X + 1, start.Position.Y - 1) || cell.Position == new Point(start.Position.X - 1, start.Position.Y + 1))
                            {
                                //finds the neibourgs of the diagonal cell
                                foreach (Cell cell2 in gridManager.Grid)
                                {
                                    Point diff2 = new Point(cell2.Position.X - cell.Position.X, cell2.Position.Y - cell.Position.Y); //diff
                                    if (diff2.X < 0)
                                    {
                                        diff2.X *= -1;
                                    }
                                    if (diff2.Y < 0)
                                    {
                                        diff2.Y *= -1;
                                    }
                                    if (diff2.X <= 1 && diff2.Y <= 1) // if the cell is a actual neibourgh
                                    {
                                        falseNeibourghs.Add(cell2);
                                    }
                                }
                            }
                            #endregion
                        }

                        if (!closedList.Contains(cell) && cell.Walkable == true)
                        {
                            neibourghs.Add(cell);
                        }
                    }
                }
                foreach (Cell cell in falseNeibourghs) //Removes all diagonally unaccessable neibourghs
                {
                    if (neibourghs.Contains(cell))
                    {
                        neibourghs.Remove(cell);
                    }
                }

                //adds the neibourghs, calculates f/g/h and adds parent
                foreach (Cell n in neibourghs)
                {
                    if (!openList.Contains(n))
                    {
                        openList.Add(n);
                        n.Parent = chosenCell;
                    }
                    else
                    {
                        //calculates the relative position to the chosenCell
                        int cost;
                        Point diff = new Point(n.Position.X - chosenCell.Position.X, n.Position.Y - chosenCell.Position.Y); //diff
                        if (diff.X == 1 && diff.Y == 1 || diff.X == -1 && diff.Y == 1 || diff.X == 1 && diff.Y == -1 || diff.X == -1 && diff.Y == -1) //Diagonal
                        {
                            cost = 14;
                        }
                        else if (diff.X == 0 && diff.Y == 0)
                        {
                            cost = 0;
                        }
                        else
                        {
                            cost = 10;
                        }

                        //checks if chosenCell is a better parent than the old one
                        if (n.G > chosenCell.G + cost)
                        {
                            n.Parent = chosenCell;
                        }
                    }
                    n.GetFValue(gridManager.Grid, goal);
                }

                //ends loop if we have arrived at the goal
                if (closedList.Contains(goal) || openList.Count == 0)
                {
                    endLoop = true;
                }
            } while (!endLoop);

            //backtrack
            bool loopDone = false;
            Cell c = goal.Parent;

            do
            {
                if (c.Parent == start)
                {
                    loopDone = true;
                }
                else
                {
                    c = c.Parent;
                }
            } while (!loopDone);
            return c;
        }

        public Cell GetNextMove()
        {
            return Astar(ShoppingList());
        }
    }
}
