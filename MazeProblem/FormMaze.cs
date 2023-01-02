using System;
using System.IO;
using System.Windows.Forms;

namespace MazeProblem
{
    public partial class FormMaze : Form
    {
        public FormMaze()
        {
            InitializeComponent();
        }

        string[,] grid = new string[100, 100];
        int rows, columns;
        int rowStart, columnStart;
        int rowGoal, columnGoal;

        private void BtnFindPath_Click(object sender, EventArgs e)
        {
            LoadData(grid);
            TxtResult.Text += "Original grid: " + Environment.NewLine;
            Display();
            TxtResult.Text += Environment.NewLine;

            if (IsPartOfThePath(rowStart, columnStart))
            {
                TxtResult.Text += "Exit of the maze is possible, below is the path: " + Environment.NewLine;
                Display();
            }
            else
            {
                TxtResult.Text += "There is no possible exit of the maze." + Environment.NewLine;
            }
        }

        private void LoadData(string[,] grid)
        {
            string path = Application.StartupPath + @"\rmaze.txt";

            StreamReader streamReader = new StreamReader(path);

            string line = "";

            rows = Convert.ToInt32(streamReader.ReadLine());
            columns = Convert.ToInt32(streamReader.ReadLine());

            for (int i = 1; i <= rows; i++)
            {
                line = streamReader.ReadLine();

                for (int j = 1; j <= columns; j++)
                {
                    grid[i, j] = line.Substring(j - 1, 1);

                    if (grid[i, j] == "S")
                    {
                        rowStart = i;
                        columnStart = j;
                    }

                    if (grid[i, j] == "G")
                    {
                        rowGoal = i;
                        columnGoal = j;
                    }
                }
            }
        }

        private void Display()
        {
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    TxtResult.Text += grid[i, j];
                }
                TxtResult.Text += Environment.NewLine;
            }
        }


        //We ask start position if it's part of the exit path,
        //then it asks one by one the positions around it if they are part of the exit path,
        //then each position that is on a valid location knows that it might be a part of the exit path and asks recoursively the positions around it if they are part of the correct path.
        //If eventually we come to a dead end, all positions return answer false to the position that was on a valid location. Then it continue to ask in the other directions.
        //If we reach the end position it returns true and we mark it with "p" as part of the exit path,
        //then all previous positions return answer to the previous that they are actually part of the exit path and we mark them with "p", until we reach back the first position that made the initial call and give the answer to it.
        private bool IsPartOfThePath(int row, int column)
        {
            bool done = false;

            if (CheckLocation(row, column))
            {
                //Mark the positions where we have been, so we won't wander around forever.
                grid[row, column] = "-";

                if (row == rowGoal && column == columnGoal)
                {
                    done = true;
                }
                else
                {
                    done = IsPartOfThePath(row + 1, column);

                    if (!done)
                        done = IsPartOfThePath(row - 1, column);
                    if (!done)
                        done = IsPartOfThePath(row, column + 1);
                    if (!done)
                        done = IsPartOfThePath(row, column - 1);
                }
            }

            //Mark all positions that return true as part of the exit path.
            if (done)
            {
                grid[row, column] = "p";
            }

            return done;
        }

        private bool CheckLocation(int row, int column)
        {
            if (row >= 1 && row <= rows && column >= 1 && column <= columns)
            {
                if (grid[row, column] == "." || grid[row, column] == "S" || grid[row, column] == "G")
                {
                    return true;
                }
            }
            return false;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
