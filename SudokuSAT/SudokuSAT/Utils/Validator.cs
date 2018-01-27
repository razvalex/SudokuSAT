using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSAT.Utils
{
    public static class Validator
    {
        public static bool IsValid(char cellInfo)
        {
            return (cellInfo >= 49 && cellInfo <= 57) || cellInfo == 88;
        }
    }
}
