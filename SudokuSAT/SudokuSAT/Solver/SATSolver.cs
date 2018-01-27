using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSAT.Infrastructure;

namespace SudokuSAT.Solver
{
    public class SATSolver
    {
        #region Formula & Related Methods
   
        private Formula _Formula;

        public void ReadFormula(StringBuilder formulaString)
        {
            _Formula = new Formula(formulaString);
        }

        public bool HasEmptyClause()
        {
            return _Formula.HasEmptyClause();
        }

        public bool IsEmpty()
        {
            return _Formula.IsEmpty();
        }

        public int SelectBranchVar()
        {
            return _Formula.GetHighestRankVariable();
        }

        public void SetVar(int var)
        {
            _Formula.SetVariable(var);
        }

        public void UnSet()
        {
            _Formula.UndoPropagate();
        }

        #endregion

        #region Solver


        public void Success()
        {
            Console.WriteLine("Formula is satisfiable");
        }

        public void Failure()
        {
            Console.WriteLine("Formula is unsatisfiable");
        }

        public int[] Solve(StringBuilder formulaString)
        {
            
            ReadFormula(formulaString);
            
            if (DPLL())
            {
                Success();
                return _Formula.GetCurrentAssigment();

            }
            else
            {
                Failure();
                return null;
            }
        }

        /**
         * This is the recursive method that performs the Davis-Putnam
         * algorithm
         */
        public bool DPLL()
        {

            //If the formula is empty, no more clauses need to be satisfied
            if (IsEmpty())
            {
                return true;

            //If a clause exists that cannot be satisfied with
            //the current assignment
            }
            else if (HasEmptyClause())
            {
                return false;

            }
            else
            {
                int var = SelectBranchVar();
           
                //compute ranks will give the branch variable and
                //unitProp will give the assignment.

                SetVar(var);

                if (DPLL())
                {
                    return true;

                }
                else
                {

                    // Unset var in the formula
                    // Undoes any unit propagation
                    UnSet();

                    // Try reversing the assignment
                    SetVar(-var);

                    if (DPLL())
                    {
                        return true;
                    }
                    else
                    {
                        UnSet();
                        return false;
                    }
                }
            }
        }

        #endregion
    }
}
