﻿using System;
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
        Point actualPosition;
        GridManager gridManager;

        public Wizard(Cell startCell, GridManager gridManager)
        {
            this.position = startCell;
            this.gridManager = gridManager;
            this.actualPosition = startCell.Position;
        }

        public void Render(Graphics dc)
        {
            dc.FillRectangle(new SolidBrush(Color.White), position.BoundingRectangle);
            dc.DrawRectangle(new Pen(Color.Black), position.BoundingRectangle);
            dc.DrawImage(Image.FromFile(@"Images\wizard_idle.png"), position.BoundingRectangle);
        }

        private Cell ShoppingList()
        {
            //Returns 1,1 for now
            foreach (Cell cell in gridManager.Grid)
            {
                if (cell.Position == new Point(1, 1))
                {
                    return cell;
                }
            }
            return null; //If 1,1 doesnt exist

            /*
             This method should figure out what the priority target is, and return it.
             */
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
                    if (diff.X <= 1 && diff.Y <= 1)
                    {
                        if (cell.Walkable == false)
                        {
                            if (cell.Position == new Point(start.Position.X - 1, start.Position.Y))
                            {
                                foreach (Cell c in gridManager.Grid)
                                {
                                    if (c.Position == new Point(start.Position.X - 1, start.Position.Y + 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                    if (c.Position == new Point(start.Position.X - 1, start.Position.Y - 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                }
                            }
                            else if (cell.Position == new Point(start.Position.X + 1, start.Position.Y))
                            {
                                foreach (Cell c in gridManager.Grid)
                                {
                                    if (c.Position == new Point(start.Position.X + 1, start.Position.Y + 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                    if (c.Position == new Point(start.Position.X + 1, start.Position.Y - 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                }
                            }
                            else if (cell.Position == new Point(start.Position.X, start.Position.Y - 1))
                            {
                                foreach (Cell c in gridManager.Grid)
                                {
                                    if (c.Position == new Point(start.Position.X + 1, start.Position.Y - 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                    if (c.Position == new Point(start.Position.X - 1, start.Position.Y - 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                }
                            }
                            else if (cell.Position == new Point(start.Position.X, start.Position.Y + 1))
                            {
                                foreach (Cell c in gridManager.Grid)
                                {
                                    if (c.Position == new Point(start.Position.X + 1, start.Position.Y + 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                    if (c.Position == new Point(start.Position.X - 1, start.Position.Y + 1))
                                    {
                                        falseNeibourghs.Add(c);
                                    }
                                }
                            }
                        }

                        if (!closedList.Contains(cell) && cell.Walkable == true)// if the cell is a actual neibourgh
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
            Cell returnCell = goal.Parent;

            do
            {
                if (returnCell.Parent == start)
                {
                    loopDone = true;
                }
                else
                {
                    returnCell = returnCell.Parent;
                }
            } while (!loopDone);
            return returnCell;
        }

        public Cell GetNextMove()
        {
            return Astar(ShoppingList());
        }
    }
}
