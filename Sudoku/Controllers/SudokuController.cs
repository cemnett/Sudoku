using Microsoft.AspNetCore.Mvc;
using Sudoku.Models; // Ensure this points to your SudokuGenerator class

namespace Sudoku.Controllers
{
    public class SudokuController : Controller
    {
        // GET: /Sudoku/
        // This action displays the form.
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Sudoku/
        // This action receives the selected difficulty, generates the sudoku board, and shows the board.
        [HttpPost]
        public IActionResult Index(int difficulty)
        {
            // Create generator and set difficulty based on user selection
            SudokuGenerator generator = new SudokuGenerator();
            generator.Difficulty = difficulty;
            // Make sure you have a parameterless Generate() method
            generator.Generate();
            
            // Retrieve the board and pass it to the view
            int[,] board = generator.GetBoard();
            
            var model = new SudokuBoardViewModel
            {
                Puzzle = generator.GetBoard(),
                Solution = generator.GetSolution(),
                JaggedSolution = ConvertToJaggedArray(generator.GetSolution())
            };

            return View("Board", model);
        }

        public int[][] ConvertToJaggedArray(int[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            int[][] jagged = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                jagged[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jagged[i][j] = array[i, j];
                }
            }
            return jagged;
        }

    }
}
