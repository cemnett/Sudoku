namespace Sudoku.Models
{
    public class SudokuBoardViewModel
    {
        public int[,] Puzzle { get; set; }
        public int[,] Solution { get; set; }
    }
}
