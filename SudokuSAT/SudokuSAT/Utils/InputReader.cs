using System;
using System.Collections.Generic;
using System.IO;
using SudokuSAT.Annotations;
using SudokuSAT.Models;

namespace SudokuSAT.Utils
{
    public class InputReader
    {
        #region Constructor & Properties
        private static InputReader _fileReaderInstance;

        public static InputReader GetInstance()
        {
            return _fileReaderInstance ?? (_fileReaderInstance = new InputReader());
        }

        private static char[,] _sudokuMatrix = Matrix.GetInstance();

        public static char[,] GetSudokuMatrixInstance()
        {
            return _sudokuMatrix ?? (_sudokuMatrix = Matrix.GetInstance());
        }
        #endregion

        public void ReadFileAndSetMatrix(string problemPath)
        {
            // read all the lines from location
            var linesList = File.ReadAllLines(problemPath);
            var rowCounter = 0;

            foreach (var line in linesList)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var thisRow = line.ToCharArray();
                var currentCounter = 0;
                for (int col = 0, row = 0; col < line.Length; col += 2, row++)
                {
                    if (Validator.IsValid(thisRow[col]))
                    {
                        _sudokuMatrix[rowCounter, currentCounter] = thisRow[col];
                        currentCounter++;
                    }
                    else
                    {
                        throw new Exception("Invalid character at row " + rowCounter + " & column " + col);
                    }
                }
                rowCounter++;
            }
        }
    }
}
