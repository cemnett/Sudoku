using System;
using System.Collections.Generic;


namespace Sudoku
{
    public class SudokuGenerator
    {
        private static Random rand = new Random();
        private int[,] board = new int[9, 9];

        private int difficulty;

        public int Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }
        

        public void Generate(SudokuGenerator g){
            int numToRemove = g.Difficulty * 20;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            FillGrid();
            RemoveNumbers(numToRemove);
            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("Time to generate board: " + elapsedMs);
        }

        private bool FillGrid() {
            for (int row = 0; row < 9; row++) {
                for (int col = 0; col < 9; col++) {
                    if (board[row, col] == 0) {
                        List<int> numbers = new List<int>();

                        for (int i = 1; i <= 9; i++) {
                            numbers.Add(i);
                        }
                        Shuffle(numbers);

                        foreach (var num in numbers) {
                            if (IsValid(row, col, num)) {
                                board[row, col] = num;
                                if (FillGrid()) return true;
                                board[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }


        private void RemoveNumbers(int count) {
            List<(int row, int col)> candidates = new List<(int, int)>();
            for (int row = 0; row < 9; row++) {
                for (int col = 0; col < 9; col++) {
                    if (board[row, col] != 0) {
                        candidates.Add((row, col));
                    }
                }
            }
            
            Shuffle(candidates);

            int removed = 0;
            foreach (var (row, col) in candidates) {
                int oldVal = board[row, col];
                board[row, col] = 0;
                if (NumberOfSolutions() != 1) {
                    board[row, col] = oldVal;
                } else {
                    Console.WriteLine($"Removed number {removed + 1}...");
                    removed++;
                    if (removed >= count) break;
                }
            }
        }


        private int NumberOfSolutions() {    
            int count = 0;    
            int[,] tempBoard = (int[,])board.Clone();    
            int[] rows = new int[9], cols = new int[9], boxes = new int[9];
            // Initialize bitsets (rows, cols, and boxes)    
            for (int r = 0; r < 9; r++) {        
                for (int c = 0; c < 9; c++) {            
                    if (tempBoard[r, c] != 0) {                
                        int num = tempBoard[r, c] - 1;                
                        int boxIndex = (r / 3) * 3 + (c / 3);                
                        rows[r] |= (1 << num);                
                        cols[c] |= (1 << num);                
                        boxes[boxIndex] |= (1 << num);            
                    }        
                }    
            }
            // Backtracking with memoization (avoiding redundant checks)    
            if (CountSolutions(tempBoard, ref count, rows, cols, boxes)) return 2;    
            return count;
        }

        private bool CountSolutions(int[,] grid, ref int count, int[] rows, int[] cols, int[] boxes) {    
            int minOptions = 10, 
            targetRow = -1, 
            targetCol = -1;
            // Find the cell with the fewest possible numbers    
            for (int r = 0; r < 9; r++) {        
                for (int c = 0; c < 9; c++) {            
                    if (grid[r, c] == 0) {                
                        int boxIndex = (r / 3) * 3 + (c / 3);                
                        int possibilities = ~(rows[r] | cols[c] | boxes[boxIndex]) & 0x1FF; // 9-bit mask
                        int numOptions = CountBits(possibilities);                
                        if (numOptions < minOptions) {                    
                            minOptions = numOptions;                    
                            targetRow = r;                    
                            targetCol = c;                
                        }            
                    }        
                }    
            }
            // If no empty cells, we found a solution    
            if (targetRow == -1) {        
                count++;        
                return count > 1;    
            }
            int boxIdx = (targetRow / 3) * 3 + (targetCol / 3);    
            int availableNumbers = ~(rows[targetRow] | cols[targetCol] | boxes[boxIdx]) & 0x1FF;
            for (int num = 1; num <= 9; num++) {        
                if ((availableNumbers & (1 << (num - 1))) != 0) {            
                    grid[targetRow, targetCol] = num;
                    // Update bitsets            
                    rows[targetRow] |= (1 << (num - 1));            
                    cols[targetCol] |= (1 << (num - 1));            
                    boxes[boxIdx] |= (1 << (num - 1));
                    if (CountSolutions(grid, ref count, rows, cols, boxes)) return true;
                    // Backtrack            
                    rows[targetRow] &= ~(1 << (num - 1));            
                    cols[targetCol] &= ~(1 << (num - 1));            
                    boxes[boxIdx] &= ~(1 << (num - 1));            
                    grid[targetRow, targetCol] = 0;        
                }    
            }
            return false;
        }


        private int CountBits(int num) {    
            int count = 0;    
            while (num > 0) {        
                count += num & 1;        
                num >>= 1;    
            }    
            return count;
        
        }

        private bool IsValid(int row, int col, int num) {
            for (int i = 0; i < 9; i++) {
                if (board[row, i] == num || board[i, col] == num) return false;
                if (board[row / 3 * 3 + i / 3, col / 3 * 3 + i % 3] == num) return false;
            }
            return true;
        }


        private void Shuffle<T>(List<T> list) {
            for (int i = list.Count - 1; i > 0; i--) {
                int j = rand.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }


        public void Print() {
            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        
        static void Main(string[] args)
        {
            SudokuGenerator generator = new SudokuGenerator();

            Console.WriteLine("Enter the level of difficulty you would like: ");
            Console.WriteLine("1 - Easy");
            Console.WriteLine("2 - Medium");
            Console.WriteLine("3 - Hard");
            generator.Difficulty = Convert.ToInt32(Console.ReadLine());

            generator.Generate(generator);
            generator.Print();

        }
    }
}
