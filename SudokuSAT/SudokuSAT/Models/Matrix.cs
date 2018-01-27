using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSAT.Models
{
    public class Matrix
    {
        #region Constructor & Properties

        private static char[,] _matrixInstance;
        private const int SquareSize = 9;
        private const int NumVars = 9;

        public static char[,] GetInstance()
        {
            if (_matrixInstance != null)
            {
                return _matrixInstance;
            }
            _matrixInstance = new char[SquareSize, SquareSize];
            return _matrixInstance;

        }

        public static int GetSquareSize()
        {
            return SquareSize;
        }

        public static int GetNumVars()
        {
            return NumVars;
        }

        #endregion
    }
}
