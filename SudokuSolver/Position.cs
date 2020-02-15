using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Position
    {
        public int x;
        public int y;
        public Position(int x_, int y_)
        {
            x = x_;
            y = y_;
        }
    }
}
