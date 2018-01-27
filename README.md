SudokuSAT
=========

Sudoku Solver using SAT Solver for Structural Complexity Course.

Normalize the input, assign sat structures for the current unknown variables.
<div>X 2 X X X X X X X</div>
<div>X X X 6 X X X X 3</div>
<div>X 7 4 X 8 X X X X</div>
<div>X X X X X 3 X X 2</div>
<div>X 8 X X 4 X X 1 X</div>
<div>6 X X 5 X X X X X</div> 
<div>X X X X 1 X 7 8 X</div>
<div>5 X X X X 9 X X X</div> 
<div>X X X X X X X 4 X</div>

Output will resolve (satisfy) the current sudoku matrix if formula is satisfiable.
<div>[ 1 | 2 | 6 ][ 4 | 3 | 7 ][ 9 | 5 | 8 ]</div>
<div>[ 8 | 9 | 5 ][ 6 | 2 | 1 ][ 4 | 7 | 3 ]</div>
<div>[ 3 | 7 | 4 ][ 9 | 8 | 5 ][ 1 | 2 | 6 ]</div>
<div>[ 4 | 5 | 7 ][ 1 | 9 | 3 ][ 8 | 6 | 2 ]</div>
<div>[ 9 | 8 | 3 ][ 2 | 4 | 6 ][ 5 | 1 | 7 ]</div>
<div>[ 6 | 1 | 2 ][ 5 | 7 | 8 ][ 3 | 9 | 4 ]</div>
<div>[ 2 | 6 | 9 ][ 3 | 1 | 4 ][ 7 | 8 | 5 ]</div>
<div>[ 5 | 4 | 8 ][ 7 | 6 | 9 ][ 2 | 3 | 1 ]</div>
<div>[ 7 | 3 | 1 ][ 8 | 5 | 2 ][ 6 | 4 | 9 ]</div>



<em>Inspired by RogerGuTeng's SudokuSolver written in Java & other solutions written in python.</em>
