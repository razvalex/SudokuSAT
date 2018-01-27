using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSAT.Encoder
{
    public class SATEncoder
    {
        #region Constructor & Properties

        private const int NumVars = 9;
        private const int SquareSize = 9;
        public StringBuilder EncoderMessage = new StringBuilder();
        public SATEncoder()
        {
            try
            {
                GenerateClauses();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        #endregion

        #region Private Methods - Validation, Checking

        //check that each number must occur at least once in the row
        private void RowLeastOnce()
        {
            try
            {
                for (var val = 1; val <= NumVars; val++)
                {
                    for (var row = 1; row <= SquareSize; row++)
                    {
                        var currentString = string.Empty;
                        for (var col = 1; col <= SquareSize; col++)
                        {
                            currentString += row + "" + col + "" + val + " ";
                        }
                        EncoderMessage.Append(currentString + "0\n");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //check that each number must occur only once in the row
        private void RowOnlyOnce()
        {
            try
            {
                for (var val = 1; val < NumVars; val++)
                {
                    for (var row = 1; row < SquareSize; row++)
                    {
                        for (var col = 1; col <= SquareSize; col++)
                        {
                            var after = 1;
                            for (var counter = col; counter < SquareSize; counter++)
                            {
                                var currentString = string.Empty;
                                currentString = "-" + row + "" + col + "" + val + " ";
                                currentString += "-" + row + "" + (col + after) + "" + val + " 0\n";
                                EncoderMessage.Append(currentString);
                                after++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //check that each number must occur at least once in the column
        private void ColLeastOnce()
        {
            try
            {
                for (var val = 1; val <= NumVars; val++)
                {
                    for (var col = 1; col <= SquareSize; col++)
                    {
                        var currentString = string.Empty;
                        for (var row = 1; row <= SquareSize; row++)
                        {
                            currentString += row + "" + col + "" + val + " ";
                        }
                        EncoderMessage.Append(currentString + "0\n");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //check that each number must occur only once in the column
        private void ColOnlyOnce()
        {
            try
            {
                for (var val = 1; val <= NumVars; val++)
                {
                    for (var col = 1; col <= SquareSize; col++)
                    {
                        for (var row = 1; row <= SquareSize; row++)
                        {
                            var after = 1;
                            for (var counter = row; counter < SquareSize; counter++)
                            {
                                var currentString = string.Empty;
                                currentString += "-" + row + "" + col + "" + val + " ";
                                currentString += "-" + (row + after) + "" + col + "" + val + " 0\n";
                                EncoderMessage.Append(currentString);
                                after++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //check that each number must occur at least once in each sub box
        private void BoxLeastOnce()
        {
            try
            {
                for (var val = 1; val <= NumVars; val++)
                {
                    for (var row = 1; row <= SquareSize; row += 3)
                    {
                        for (var col = 1; col <= SquareSize; col += 3)
                        {
                            var currentString = string.Empty;

                            for (var innerRow = 0; innerRow < 3; innerRow++)
                            {
                                for (var innerCol = 0; innerCol < 3; innerCol++)
                                {
                                    currentString += (row + innerRow) + "" + (col + innerCol) + "" + val + " ";
                                }
                            }
                            EncoderMessage.Append(currentString + "0\n");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //check that each number must occur only once in each sub box
        private void BoxOnlyOnce()
        {
            try
            {
                for (var val = 1; val <= NumVars; val++)
                {
                    for (var row = 1; row <= SquareSize; row += 3)
                    {
                        for (var col = 1; col <= SquareSize; col += 3)
                        {
                            var stringList = new List<string>(9);

                            for (var innerRow = 0; innerRow < 3; innerRow++)
                            {
                                for (var innerCol = 0; innerCol < 3; innerCol++)
                                {
                                    stringList.Add((row + innerRow) + "" + (col + innerCol) + "" + val);
                                }
                            }

                            //same as row_only_once	                   
                            for (var counter = 0; counter <= stringList.Count; counter++)
                            {
                                var after = 1;
                                for (var innerCounter = counter; innerCounter < stringList.Count - 1; innerCounter++)
                                {
                                    var currentString = string.Empty;
                                    currentString += "-" + stringList[counter] + " ";
                                    currentString += "-" + stringList[counter + after] + " 0\n";
                                    EncoderMessage.Append(currentString);
                                    after++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //check that each number must occur at least once in each unit
        private void UnitLeastOnce()
        {
            try
            {
                for (var row = 1; row <= SquareSize; row++)
                {
                    for (var col = 1; col <= SquareSize; col++)
                    {
                        var currentString = string.Empty;
                        for (var val = 1; val <= SquareSize; val++)
                        {
                            currentString += row + "" + col + "" + val + " ";
                        }
                        EncoderMessage.Append(currentString + "0\n");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //check that each number must occur only once in each unit
        private void UnitOnlyOnce()
        {
            try
            {
                for (var row = 1; row <= SquareSize; row++)
                {
                    for (var col = 1; col <= SquareSize; col++)
                    {
                        for (var val = 1; val <= NumVars; val++)
                        {
                            for (var nextValue = val + 1; nextValue <= SquareSize; nextValue++)
                            {
                                var currentString = string.Empty;
                                currentString += "-" + row + "" + col + "" + val + " ";
                                currentString += "-" + row + "" + col + "" + nextValue + " 0\n";
                                EncoderMessage.Append(currentString);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Public Methods
        public void GenerateClauses()
        {
            try
            {
                RowLeastOnce();
                RowOnlyOnce();
                ColLeastOnce();
                ColOnlyOnce();
                BoxLeastOnce();
                BoxOnlyOnce();
                UnitLeastOnce();
                UnitOnlyOnce();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion

    }
}
