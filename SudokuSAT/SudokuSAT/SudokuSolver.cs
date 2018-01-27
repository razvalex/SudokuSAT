using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SudokuSAT.Encoder;
using SudokuSAT.Models;
using SudokuSAT.Solver;
using SudokuSAT.Utils;

namespace SudokuSAT
{
    public class SudokuSolver
    {
        #region Properties

        private static readonly string ProblemPath = AppDomain.CurrentDomain.BaseDirectory + "Data/sudoku_problem.txt";
        private const int CLAUSE_COUNT = 11988;
        private static int totalClauseCount = 0;

        #endregion

        #region Private Methods

        private static StringBuilder Mapper(char[,] sudokuMatrix)
        {
            var mapperStringList = new StringBuilder();
            var clauseCount = 0;

            for (var row = 0; row < Matrix.GetSquareSize(); row++)
            {
                for (var col = 0; col < Matrix.GetSquareSize(); col++)
                {
                    if (sudokuMatrix[row, col] == 88 && sudokuMatrix[row, col] == 'X') continue;
                    var tempRow = row + 1;
                    var tempCol = col + 1;
                    mapperStringList.Append(tempRow + "" + tempCol + "" + sudokuMatrix[row, col] + " 0\n");
                    clauseCount++;
                }
            }
            totalClauseCount = clauseCount + CLAUSE_COUNT;

            return mapperStringList;

        }

        #endregion

        public static void Main(string[] args)
        {
            var inputReader = InputReader.GetInstance();
            try
            {
                // before setting up data time
                var beforeSetup = DateTime.Now;
                // read matrix from file
                inputReader.ReadFileAndSetMatrix(ProblemPath);
                // get instance of current sudoku matrix as it was read from file
                var sudokuMatrix = InputReader.GetSudokuMatrixInstance();
                // display it
                DisplayHelper.DisplayMatrix(sudokuMatrix, Matrix.GetSquareSize(), Matrix.GetSquareSize());
                // get instance of encoder
                var satEncoder = new SATEncoder();
                // get current message which represents the encoding
                var encoderMessage = satEncoder.EncoderMessage;
                // create constraint list
                var combinedConstaint = new StringBuilder();
                // get mapper string messages
                var mapperStringB = Mapper(sudokuMatrix);
                // store data in constraint
                combinedConstaint.Append("p cnf 999 " + totalClauseCount + "\n");
                combinedConstaint.Append(mapperStringB);
                combinedConstaint.Append(encoderMessage);
                // initiate sat solver
                var satSolver = new SATSolver();
                // after setting up data time
                var afterSetup = DateTime.Now;
                // solve current constraint list
                var solution = satSolver.Solve(combinedConstaint);
                // instantiate output writer
                var outputWriter = OutputWriter.GetInstance();
                // if solution is null or empty return
                if (solution == null || !solution.Any()) return;
                // otherwise parse solution and resolve the current matrix
                var resolvedMatrix = outputWriter.ParseResult(solution, Matrix.GetSquareSize());
                // after resolve time
                var afterResolve = DateTime.Now;
                // display the resolved matrix
                DisplayHelper.DisplayResolvedMatrix(resolvedMatrix, Matrix.GetSquareSize(), Matrix.GetSquareSize());
                // display benchmarks
                Console.WriteLine("Data was setting up for: " + (afterSetup - beforeSetup).TotalMilliseconds + " ms");
                Console.WriteLine("Problem was resolved in: " + (afterResolve - afterSetup).TotalMilliseconds + " ms");
                //// stop the console closing
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                // stop the console closing
                Console.Read();
            }

        }
    }
}
