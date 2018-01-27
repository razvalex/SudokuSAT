using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSAT.Infrastructure
{
    public class Formula
    {
        #region Constructor & Properties

        // General Operators
        private const int True = 1;
        private const int False = 0;
        private const int Unassigned = -1;

        // Holds the value of each variable
        public int[] CurrentAssign;
        public Stack<LinkedList<int>> CurrentFormula = new Stack<LinkedList<int>>();
        private readonly int[][] _storedFormula;
        private bool _hasEmptyClause;

        // Ranks using the Jeroslow-Wang heuristic function
        private static readonly double[] PowersOfTwo =
        {
            0, 0.5, 0.25, 0.125, 0.625,
            0.03125, 0.015625, 0.0078125, 0.00390625, 0.001953125, 0.0009765625
        };

        private readonly double[] _variableRanks;
        private int _highestRank;
        private double _highestRankValue;

        // Holds the unit variables
        private readonly SortedSet<int> _unitVariables = new SortedSet<int>();
        // Affected by unit propagation variables
        private readonly Stack<LinkedList<int>> _affectedVariables;

        public Formula(StringBuilder formulaStringBuilder)
        {
            try
            {
                var originalFormula = new LinkedList<int>();
                _affectedVariables = new Stack<LinkedList<int>>();
                _unitVariables = new SortedSet<int>();
                var formulaString = formulaStringBuilder.ToString().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Current clause
                var clauseCount = 0;
                var clauseTotal = 0;
                var formulaType = string.Empty;
                var counter = 0;
                while (counter < formulaString.Count())
                {
                    var temporaryString = formulaString[counter];
                    if (temporaryString.StartsWith("p"))
                    {
                        // split after space
                        var parsedStrings = temporaryString.Trim().Split(new char[0]).ToList();

                        // not used for now
                        formulaType = parsedStrings[1];

                        var totalNoOfVariables = Int32.Parse(parsedStrings[2]);
                        _variableRanks = new double[2 * totalNoOfVariables + 1];

                        // initialize current assignment with unassigned
                        CurrentAssign = new int[(totalNoOfVariables + 1)];
                        for (var innerCounter = 1; innerCounter < CurrentAssign.Length; innerCounter++)
                        {
                            CurrentAssign[innerCounter] = Unassigned;
                        }

                        clauseTotal = Int32.Parse(parsedStrings[3]);
                        _storedFormula = new int[clauseTotal][];

                    }
                    else
                    {
                        if (clauseCount != clauseTotal)
                        {
                            //read current clause - split after space
                            var parsedStrings = temporaryString.Trim().Split(new char[0]).ToList();

                            var noOfVariablesInClause = parsedStrings.Count() - 1;

                            originalFormula.AddLast(clauseCount);

                            // set the number of literals for this clause
                            _storedFormula[clauseCount] = new int[noOfVariablesInClause];

                            for (var innerCounter = 0; innerCounter < noOfVariablesInClause; innerCounter++)
                            {
                                _storedFormula[clauseCount][innerCounter] = Int32.Parse(parsedStrings[innerCounter]);
                            }
                            clauseCount++;

                        }
                    }
                    counter++;
                }

                CurrentFormula.Push(originalFormula);
                _affectedVariables.Push(new LinkedList<int>());
                _hasEmptyClause = false;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Private Methods 

        private void UnitPropagate()
        {
            var newFormula = CurrentFormula.Peek();
            var variablesSetting = new LinkedList<int>();
            var localFormula = new LinkedList<int>();

            while (_unitVariables.Count > 0)
            {

                var unitVar = _unitVariables.FirstOrDefault();
                _unitVariables.Remove(_unitVariables.FirstOrDefault());
                variablesSetting.AddLast(unitVar);

                // make sure variable is not assigned already to create contradiction           
                if (CurrentAssign[Abs(unitVar)] != Unassigned)
                {
                    _hasEmptyClause = true;
                    CurrentFormula.Push(newFormula);
                    _affectedVariables.Push(variablesSetting);
                    return;
                }

                // check if propagation made the formula empty out
                if (!newFormula.Any())
                {
                    CurrentFormula.Push(newFormula);
                    _affectedVariables.Push(variablesSetting);
                    return;
                }

                // assign the variable 
                if (unitVar < 0)
                {
                    CurrentAssign[-unitVar] = False;
                }
                else
                {
                    CurrentAssign[unitVar] = True;
                }

                foreach (var curClause in newFormula)
                {

                    var clauseRemoved = false;
                    var allVariablesAssigned = true;
                    var trueClauseSize = 0;

                    // Check each variable in the clause
                    for (var poz = 0; poz < _storedFormula[curClause].Length; poz++)
                    {
                        var variableToCheck = _storedFormula[curClause][poz];

                        if ((variableToCheck > 0 && CurrentAssign[variableToCheck] == True)
                                || (variableToCheck < 0 && CurrentAssign[-variableToCheck] == False))
                        {
                            //already satisfied, remove the clause
                            clauseRemoved = true;
                            break;
                        }
                        else if ((variableToCheck > 0 && CurrentAssign[variableToCheck] == Unassigned)
                                || (variableToCheck < 0 && CurrentAssign[-variableToCheck] == Unassigned))
                        {
                            trueClauseSize++;
                            allVariablesAssigned = false;
                        }
                    }

                    // empty clause
                    if (!clauseRemoved && allVariablesAssigned)
                    {
                        _hasEmptyClause = true;
                        CurrentFormula.Push(newFormula);
                        _affectedVariables.Push(variablesSetting);
                        return;
                    }


                    if (!clauseRemoved)
                    {
                        localFormula.AddLast(curClause);
                        // check if clause has a unit variable and add to unitVar set
                        if (trueClauseSize == 1)
                        {
                            for (var poz = 0; poz < _storedFormula[curClause].Length; poz++)
                            {
                                if (CurrentAssign[Abs(_storedFormula[curClause][poz])] != Unassigned) continue;
                                // check if a contradiction appears in the set already                                
                                if (_unitVariables.Contains(-_storedFormula[curClause][poz]))
                                {
                                    _hasEmptyClause = true;
                                    CurrentFormula.Push(newFormula);
                                    _affectedVariables.Push(variablesSetting);
                                    return;
                                }
                                else
                                {
                                    _unitVariables.Add(_storedFormula[curClause][poz]);
                                    break;
                                }
                            }
                        }
                    }
                }
                newFormula = localFormula;
                localFormula = new LinkedList<int>();
            }

            _hasEmptyClause = false;
            CurrentFormula.Push(newFormula);
            _affectedVariables.Push(variablesSetting);
        }

        //computes the ranks for the clauses using the Jeroslow-Wang heuristic  
        private void ComputeRanks()
        {
            ResetRanks();

            foreach (var clause in CurrentFormula.Peek())
            {
                var trueClauseSize = 0;
                for (var poz = 0; poz < _storedFormula[clause].Length; poz++)
                {
                    var varToCheck = _storedFormula[clause][poz];
                    if (CurrentAssign[Abs(varToCheck)] == Unassigned)
                    {
                        trueClauseSize++;
                    }
                }

                // calculate 2^(-length) sum
                for (var poz = 0; poz < _storedFormula[clause].Length; poz++)
                {
                    var tempVar = _storedFormula[clause][poz];
                    if (tempVar > 0 && (CurrentAssign[tempVar] == Unassigned))
                    {
                        if (trueClauseSize < 10)
                        {
                            _variableRanks[2 * tempVar - 1] += PowersOfTwo[trueClauseSize];
                        }
                        else
                        {
                            _variableRanks[2 * tempVar - 1] += Math.Pow(2, -trueClauseSize);
                        }
                    }
                    else if (tempVar < 0 && CurrentAssign[tempVar * -1] == Unassigned)
                    {
                        if (trueClauseSize < 10)
                        {
                            _variableRanks[2 * -tempVar] += PowersOfTwo[trueClauseSize];
                        }
                        else
                        {
                            _variableRanks[2 * -tempVar] += Math.Pow(2, -trueClauseSize);
                        }
                    }

                    //take max of sum of two-side
                    double tempNewHighestRankValue = 0;
                    if (tempVar > 0)
                    {
                        tempNewHighestRankValue = _variableRanks[2 * tempVar - 1]
                                + _variableRanks[2 * tempVar];
                    }
                    else
                    {
                        tempVar = -tempVar;
                        tempNewHighestRankValue = _variableRanks[2 * (tempVar) - 1]
                                + _variableRanks[2 * (tempVar)];
                    }

                    if (tempNewHighestRankValue > _highestRankValue)
                    {
                        if (_variableRanks[2 * tempVar - 1] > _variableRanks[2 * tempVar])
                        {
                            _highestRank = tempVar;
                            _highestRankValue = tempNewHighestRankValue;
                        }
                        else
                        {
                            _highestRank = -tempVar;
                            _highestRankValue = tempNewHighestRankValue;
                        }
                    }
                }
            }
        }

        private void ResetRanks()
        {
            _variableRanks[0] = -1;
            for (int poz = 1; poz < _variableRanks.Length; poz++)
            {
                _variableRanks[poz] = 0;
            }
            _highestRank = -1;
            _highestRankValue = -1;
        }

        private static int Abs(int num)
        {
            if (num < 0)
            {
                return -num;
            }
            return num;
        }

        #endregion

        #region Public Methods 

        public void UndoPropagate()
        {
            if (CurrentFormula.Count > 1)
            {
                CurrentFormula.Pop();
            }
            if (_affectedVariables.Count > 1)
            {
                var temporaryList = _affectedVariables.Pop();
                foreach (var poz in temporaryList)
                {
                    CurrentAssign[Abs(poz)] = Unassigned;
                }
            }
            _unitVariables.Clear();
        }

        public int GetHighestRankVariable()
        {
            ComputeRanks();
            return _highestRank;
        }

        public bool HasEmptyClause()
        {
            return _hasEmptyClause;
        }

        public void SetVariable(int var)
        {
            if (_unitVariables.Count == 0)
            {
                _unitVariables.Add(var);
            }
            UnitPropagate();
        }

        public bool IsEmpty()
        {
            return CurrentFormula.Peek().Count == 0;
        }

        public int[] GetCurrentAssigment()
        {
            return CurrentAssign;
        }

        #endregion

        public override string ToString()
        {
            var temporaryString = string.Empty;
            for (var counter = 1; counter < temporaryString.Length; counter++)
            {
                temporaryString += "Variable " + counter + "//\\ Value " + CurrentAssign[counter] + "\n";
            }
            return temporaryString;
        }
    }
}
