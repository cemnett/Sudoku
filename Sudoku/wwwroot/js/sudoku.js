document.addEventListener('DOMContentLoaded', function () {
    // Retrieve the solution board from the hidden element.
    let solutionDataDiv = document.getElementById('solutionData');
    let solutionBoard = [];
    if (solutionDataDiv) {
        solutionBoard = JSON.parse(solutionDataDiv.dataset.solution);
    }

    // Attach event listeners to all editable inputs.
    const inputs = document.querySelectorAll('.sudoku-input');
    inputs.forEach(function (input) {
        input.addEventListener('blur', function () {
            let row = parseInt(input.getAttribute('data-row'), 10);
            let col = parseInt(input.getAttribute('data-col'), 10);
            let userInput = input.value.trim();

            // If left blank, do nothing.
            if (userInput === "") {
                return;
            }

            // Parse the userInput and validate it.
            let num = parseInt(userInput, 10);
            if (isNaN(num) || num < 1 || num > 9) {
                alert("Please enter a number between 1 and 9.");
                input.value = "";
                return;
            }

            // Validate against the solution board.
            if (num !== solutionBoard[row][col]) {
                alert("Incorrect value for this square!");
                input.value = "";
                return;
            }

            // If the answer is correct, replace the input with a span.
            let span = document.createElement("span");
            span.classList.add("fixed-cell");
            span.innerText = num;
            input.parentNode.replaceChild(span, input);
        });
    });

    // Toggle solution button functionality (unchanged).
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
});
