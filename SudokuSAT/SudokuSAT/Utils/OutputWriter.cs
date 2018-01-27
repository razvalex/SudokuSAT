using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSAT.Models;

namespace SudokuSAT.Utils
{
    public class OutputWriter
    {
        #region Constructor & Properties 

        private static OutputWriter _instance;
        private char[,] _result;
        private const int ToBeCheckedValue = 1;
        private const int MinValue = 111;
        private const int MaxValue = 1000;
        private const int MaxColValue = 8;

        public static OutputWriter GetInstance()
        {
            return _instance ?? (_instance = new OutputWriter());
        }

        #endregion 

        public char[,] ParseResult(int[] passedResult, int length)
        {
            _result = Matrix.GetInstance();

            var row = 0;
            var col = 0;

            if (passedResult == null)
            {
                throw new Exception("The passed result is null.");
            }

            for (var counter = MinValue; counter < MaxValue; counter++)
            {
                if (passedResult[counter] != ToBeCheckedValue) continue;

                _result[row, col] = (char)MathHelper.Modulo10(counter);

                if (col < MaxColValue)
                {
                    col++;
                }
                else
                {
                    row++;
                    col = 0;
                }
            }
            return _result;
        }

    }
}
