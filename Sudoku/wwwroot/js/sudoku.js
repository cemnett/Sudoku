document.addEventListener('DOMContentLoaded', function () {
    // ------------------------------
    // 1. Retrieve the solution board from the hidden element.
    let solutionDataDiv = document.getElementById('solutionData');
    let solutionBoard = [];
    if (solutionDataDiv) {
        solutionBoard = JSON.parse(solutionDataDiv.dataset.solution);
    }

    // ------------------------------
    // 2. Attach event listeners to all editable inputs (blank cells).
    const inputs = document.querySelectorAll('.sudoku-input');
    inputs.forEach(function (input) {
        input.addEventListener('blur', function () {
            let row = parseInt(input.getAttribute('data-row'), 10);
            let col = parseInt(input.getAttribute('data-col'), 10);
            let userInput = input.value.trim();

            // If input is blank, simply return.
            if (userInput === "") {
                return;
            }

            // Validate the user input to be a number between 1 and 9.
            let num = parseInt(userInput, 10);
            if (isNaN(num) || num < 1 || num > 9) {
                alert("Please enter a number between 1 and 9.");
                input.value = "";
                return;
            }

            // Validate the input against the solution board.
            if (num !== solutionBoard[row][col]) {
                alert("Incorrect value for this square!");
                input.value = "";
                return;
            }

            // If the answer is correct, replace the input with a span that mimics a fixed cell.
            let span = document.createElement("span");
            span.classList.add("fixed-cell");
            span.innerText = num;
            input.parentNode.replaceChild(span, input);

            checkCompletion();
        });
    });

    // ------------------------------
    // 3. Toggle solution button functionality.
    let toggleBtn = document.getElementById('toggleSolutionBtn');
    let solutionDiv = document.getElementById('solutionDiv');
    if (toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            if (solutionDiv.style.display === 'none') {
                solutionDiv.style.display = 'block';
                toggleBtn.innerText = 'Hide Solution';
            } else {
                solutionDiv.style.display = 'none';
                toggleBtn.innerText = 'Show Solution';
            }
        });
    }

    function checkCompletion() {
        // If there are no more inputs, assume the puzzle is solved.
        const remainingInputs = document.querySelectorAll('.sudoku-input');
        if (remainingInputs.length === 0) {
            let congratsMsg = document.getElementById("congratsMessage");
            if (congratsMsg) {
                congratsMsg.style.display = 'block';
            } else {
                alert("Congratulations, you solved it!");
            }
        }
    }

    const cells = document.querySelectorAll('table td');
    
    cells.forEach(function (cell) {
        cell.addEventListener('click', function () {
            // Process only if the cell contains a fixed value.
            const fixedSpan = cell.querySelector('span.fixed-cell');
            if (!fixedSpan) return; // Skip if this cell is editable (or not pre-filled).

            // Clear any previous highlighting.
            cells.forEach(function (c) {
                c.classList.remove('selected-cell', 'same-number-cell', 'highlighted-line');
            });

            // Get the row, column, and number from the clicked cell.
            const selectedRow = parseInt(cell.getAttribute('data-row'), 10);
            const selectedCol = parseInt(cell.getAttribute('data-col'), 10);
            const selectedValue = fixedSpan.innerText.trim();

            // Highlight the clicked cell (dark blue).
            cell.classList.add('selected-cell');

            // Highlight other cells accordingly.
            cells.forEach(function (c) {
                let currentRow = parseInt(c.getAttribute('data-row'), 10);
                let currentCol = parseInt(c.getAttribute('data-col'), 10);
                // Highlight the entire row and column (light blue) except the selected cell.
                if ((currentRow === selectedRow || currentCol === selectedCol) && c !== cell) {
                    c.classList.add('highlighted-line');
                }
                // For fixed cells with the same value (except the selected one), highlight them medium blue.
                let sp = c.querySelector('span.fixed-cell');
                if (c !== cell && sp && sp.innerText.trim() === selectedValue) {
                    c.classList.add('same-number-cell');
                }
            });
        });
    });
});
