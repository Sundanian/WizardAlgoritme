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
        //private int keys; // Skal flyttes ind på Wizard
        private bool stormkey = false;
        private bool icekey = false;
        private bool hasPotion;
        private bool canIWinNow;
        private int moveCount = 0;

        List<Cell> returnCellList;
        List<Cell> openList;
        List<Cell> closedList;
        Cell position;
        Point actualPosition;
        GridManager gridManager;
        Cell nextGoal;

        public bool HasPotion
        {
            get
            {
                return hasPotion;
            }

            set
            {
                hasPotion = value;
            }
        }
        public bool CanIWinNow
        {
            get
            {
                return canIWinNow;
            }

            set
            {
                canIWinNow = value;
            }
        }
        public Cell Position
        {
            get { return position; }
            set { position = value; }
        }
        public bool Stormkey
        {
            get { return stormkey; }
            set { stormkey = value; }
        }
        public bool Icekey
        {
            get { return icekey; }
            set { icekey = value; }
        }

        public Wizard(Cell startCell, GridManager gridManager)
        {
            this.position = startCell;
            this.gridManager = gridManager;
            hasPotion = false;
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
            foreach (Cell cell in gridManager.Goals)
            {
                if (stormkey == false && canIWinNow == false)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.STORMKEY);
                    return nextGoal;
                }
                else if (stormkey == true && hasPotion == false && !canIWinNow)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.STORM);
                    return nextGoal;
                }
                else if (icekey == false && hasPotion == true && canIWinNow == false)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.ICEKEY);
                    return nextGoal;
                }
                else if (icekey == true && hasPotion == true && !canIWinNow)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.ICE);
                    return nextGoal;
                }
                else if (canIWinNow == true)
                {
                    nextGoal = gridManager.Goals.Find(node => node.MyType == CellType.PORTAL);
                    return nextGoal;
                }
            }
            return null;
        }

        private List<Cell> Astar(Cell goal)
        {
            List<Cell> returnCellList = new List<Cell>();
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

                //finds the false neibourghs
                List<Cell> falseNeibourghs = new List<Cell>();
                foreach (Cell n in chosenCell.Neibourghs)
                {
                    if (!n.Walkable)
                    {
                        if (n.Position == new Point(chosenCell.Position.X - 1, chosenCell.Position.Y))
                        {
                            foreach (Cell diaN in chosenCell.Neibourghs)
                            {
                                if (diaN.Position == new Point(chosenCell.Position.X - 1, chosenCell.Position.Y - 1) || diaN.Position == new Point(chosenCell.Position.X - 1, chosenCell.Position.Y + 1))
                                {
                                    falseNeibourghs.Add(diaN);
                                }
                            }
                        }
                        if (n.Position == new Point(chosenCell.Position.X + 1, chosenCell.Position.Y))
                        {
                            foreach (Cell diaN in chosenCell.Neibourghs)
                            {
                                if (diaN.Position == new Point(chosenCell.Position.X + 1, chosenCell.Position.Y - 1) || diaN.Position == new Point(chosenCell.Position.X + 1, chosenCell.Position.Y + 1))
                                {
                                    falseNeibourghs.Add(diaN);
                                }
                            }
                        }
                        if (n.Position == new Point(chosenCell.Position.X, chosenCell.Position.Y - 1))
                        {
                            foreach (Cell diaN in chosenCell.Neibourghs)
                            {
                                if (diaN.Position == new Point(chosenCell.Position.X - 1, chosenCell.Position.Y - 1) || diaN.Position == new Point(chosenCell.Position.X + 1, chosenCell.Position.Y - 1))
                                {
                                    falseNeibourghs.Add(diaN);
                                }
                            }
                        }
                        if (n.Position == new Point(chosenCell.Position.X, chosenCell.Position.Y + 1))
                        {
                            foreach (Cell diaN in chosenCell.Neibourghs)
                            {
                                if (diaN.Position == new Point(chosenCell.Position.X + 1, chosenCell.Position.Y + 1) || diaN.Position == new Point(chosenCell.Position.X - 1, chosenCell.Position.Y + 1))
                                {
                                    falseNeibourghs.Add(diaN);
                                }
                            }
                        }
                    }
                }

                //adds the neibourghs, calculates f/g/h and adds parent
                foreach (Cell n in chosenCell.Neibourghs)
                {
                    if (!falseNeibourghs.Contains(n)) //Sorts out the false/unreacable neibourghs
                    {
                        if (!closedList.Contains(n) && n.Walkable)
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
                }

                //ends loop if we have arrived at the goal
                if (closedList.Contains(goal) || openList.Count == 0)
                {
                    endLoop = true;
                }
            } while (!endLoop);

            //backtrack
            bool loopDone = false;
            Cell returnCell = goal;

            do
            {
                if (returnCell.Parent == start)
                {
                    returnCellList.Add(returnCell);
                    loopDone = true;
                }
                else
                {
                    returnCellList.Add(returnCell);
                    returnCell = returnCell.Parent;
                }
            } while (!loopDone);
            return returnCellList;
        }

        private List<Cell> BFS(Cell goal)
        {
            bool foundGoal = false;
            Queue<Cell> queue = new Queue<Cell>();
            Cell start = position;
            start.Parent = start;
            queue.Enqueue(start);

            do
            {
                Cell cell = queue.Dequeue();
                cell.Visitied = true;
                if (cell == goal)
                {
                    foundGoal = true;
                }
                foreach (Cell n in cell.Neibourghs)
                {
                    if (!(n.Visitied) && n.Walkable == true)
                    {
                        queue.Enqueue(n);
                        if (n.Parent == n.Parent)
                        {
                            n.Parent = cell;
                        }
                    }
                }
                if (foundGoal)
                {
                    List<Cell> path = new List<Cell>();
                    path.Add(cell);
                    do
                    {
                        cell = cell.Parent;
                        path.Add(cell);
                    } while (cell.Parent != cell);
                    return path;
                }
            } while (!foundGoal);
            return null;
        }

        private List<Cell> DFS(Cell goal)
        {
            bool foundGoal = false;
            Stack<Cell> stack = new Stack<Cell>();
            Cell start = position;
            start.Parent = start;
            stack.Push(start);

            do
            {
                Cell cell = stack.Pop();
                cell.Visitied = true;
                if (cell == goal)
                {
                    foundGoal = true;
                }
                foreach (Cell n in cell.Neibourghs)
                {
                    if (!(n.Visitied) && n.Walkable == true)
                    {
                        stack.Push(n);
                        if (n.Parent == n.Parent)
                        {
                            n.Parent = cell;
                        }
                    }
                }
                if (foundGoal)
                {
                    List<Cell> path = new List<Cell>();
                    path.Add(cell);
                    do
                    {
                        cell = cell.Parent;
                        path.Add(cell);
                    } while (cell.Parent != cell);
                    return path;
                }
            } while (!foundGoal);
            return null;
        }

        public Cell GetNextMove()
        {
            if (moveCount == 0)
            {
                foreach (Cell cell in gridManager.Grid)
                {
                    cell.Parent = cell;
                    if (cell.MyType != CellType.FORESTPATH)
                    {
                        cell.Visitied = false;
                    }
                }
                returnCellList = DFS(ShoppingList());
                moveCount = returnCellList.Count;
            }
            moveCount--;
            return returnCellList[moveCount];
        }
    }
}
