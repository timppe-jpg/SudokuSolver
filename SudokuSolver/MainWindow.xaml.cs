using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SudokuSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] sudoku = new int[9, 9];

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Solves sudoku with backtracking algorithm recursively
        /// </summary>
        /// <param name="sud">array containing sudoku</param>
        /// <returns></returns>
        public bool Solve(int[,] sud)
        {
            bool? animate = false;
            checkBox_animate.Dispatcher.Invoke(new Action(
                () => animate = checkBox_animate.IsChecked));

            if ((bool)animate)
            {
                Print(sud);
                Thread.Sleep(50);
            }
            

            Position position = FindEmpty(sud);
            if (position == null) return true;

            int row = position.y;
            int column = position.x;

            //Try to insert numbers 1-9
            for (int i = 1; i < 10; i++)
            {
                if(Valid(sud,i,new Position(column, row)))
                {
                    sud[row, column] = i;

                    if (Solve(sud)) return true;

                    sud[row, column] = 0;    
                }
            }
            Print(sud);
            return false;
        }

        /// <summary>
        /// Check if next move follows sudoku rules
        /// </summary>
        /// <param name="sud">Current sudoku</param>
        /// <param name="number">Number to test</param>
        /// <param name="position">position to insert number</param>
        /// <returns>True, if follows sudoku rules</returns>
        public bool Valid(int[,] sud,int number, Position position)
        {
            //Row
            for (int i = 0; i < sud.GetLength(0); i++)
            {
                if (sud[position.y,i] == number && position.y != number) return false;
            }

            //Column
            for (int i = 0; i < sud.GetLength(1); i++)
            {
                if (sud[i,position.x] == number && position.x != number) return false;
            }

            //Box
            int boxX = position.x / 3;
            int boxY = position.y / 3;

            for (int i = boxY * 3; i < boxY*3+3; i++)
            {
                for (int j = boxX*3; j < boxX*3+3; j++)
                {
                    if (sud[i,j] == number && new Position(i,j) != position) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds next empty cell in sudoku
        /// </summary>
        /// <param name="sud">Current suodku</param>
        /// <returns>Next empty cell</returns>
        public Position FindEmpty(int[,] sud)
        {
            for (int i = 0; i < sud.GetLength(0); i++)
            {
                for (int j = 0; j < sud.GetLength(1); j++)
                {
                    if (sud[i, j] == 0) return new Position(j, i);
                }
            }
            return null;
        }

        /// <summary>
        /// Print 2d array containing the solved sudoku to UI textbox
        /// </summary>
        /// <param name="sud"></param>
        public void Print(int[,] sud)
        {
            string result = string.Empty;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBox_Solved.Text = "";
            }), DispatcherPriority.Background);

            

            for (int y = 0; y < sud.GetLength(0); y++)
            {
                for (int x = 0; x < sud.GetLength(1); x++)
                {
                    if (x % 3 == 0 && x != 0) result += " | ";

                    result += " " + sud[y, x] + " ";

                    if (x == sud.GetLength(1) - 1)
                    {
                        if (y % 3 == 2 && y != 0 && y != 8)
                        {
                            result += "\n";
                            result += "----------------------------";
                        }   
                        result += "\n";
                    }
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBox_Solved.Text = result;
            }), DispatcherPriority.Background);
        }

        /// <summary>
        /// EventHandler to allow window to be moved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Init sudoku data into 2d array from UI.
        /// It is overly complicated due to the way elements were copied 
        /// </summary>
        private void InitSudoku()
        {
            int lap = 0;
            for (int x = 0; x < 3; x++)//3
            {
                for (int z = 0; z < 3; z++)
                {
                    for (int i = 8; i > 5; i--)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int y_ = i - (x * 3);
                            int x_ = j + (x * 3) + ((z - x) * 3);
                            sudoku[y_, x_] = int.Parse((SudokuTable.Children[lap++] as TextBox).Text);
                        }
                    }
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitSudoku();
            Task.Factory.StartNew(() => Solve(sudoku));
        }
    }
}