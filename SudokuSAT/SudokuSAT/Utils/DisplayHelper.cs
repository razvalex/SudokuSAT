using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSAT.Models;

namespace SudokuSAT.Utils
{
    public static class DisplayHelper
    {

        public static void DisplayMatrix(char[,] sudokuMatrix, int maxRow, int maxCol)
        {
            for (var row = 0; row < maxRow; row++)
            {
                var innerRow = row + 1;
                Console.Write("row #" + innerRow + ": ");
                for (var col = 0; col < maxCol; col++)
                {
                    Console.Write(sudokuMatrix[row, col] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void DisplayResolvedMatrix(char[,] resolvedMatrix, int maxRow, int maxCol)
        {
            for (var row = 0; row < maxRow; row++)
            {

                if (row == 0)
                {
                    for (var innerCounter = 0; innerCounter < maxRow; innerCounter++)
                    {
                        Console.Write((innerCounter + 1) % 3 == 0 ? "=====" : "====");
                    }
                    Console.WriteLine();
                }

                for (var col = 0; col < maxCol; col++)
                {
                    if (col == 0)
                    {
                        Console.Write("[ ");
                    }
                    if ((col + 1) % 3 == 0 && col != 8)
                    {
                        Console.Write((int)resolvedMatrix[row, col] + " ][ ");
                    }
                    else if (col == 8)
                    {
                        Console.Write((int)resolvedMatrix[row, col] + " ] ");
                    }
                    else
                    {
                        Console.Write((int)resolvedMatrix[row, col] + " | ");
                    }
                }
                Console.WriteLine();
                for (var innerCounter = 0; innerCounter < maxRow; innerCounter++)
                {
                    if ((innerCounter + 1) % 3 == 0)
                    {
                        Console.Write((innerCounter + 1) % 3 == 0 ? "=====" : "====");
                    }
                    else
                    {
                        Console.Write((innerCounter + 1) % 3 == 0 ? "-----" : "----");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
