﻿using System;
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

        public bool Solve(int[,] sud)
        {
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
                //TODO add animation
            }

            return false;
        }

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
        public void Print(int[,] sud)
        {
            string result = string.Empty;

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
            
            TextBox_Solved.Text = result;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

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
            Solve(sudoku);
            Print(sudoku);
        }
    }
}