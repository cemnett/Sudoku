document.addEventListener('DOMContentLoaded', function () {
    let solutionDataDiv = document.getElementById('solutionData');
    let solutionBoard = [];
    if (solutionDataDiv) {
        solutionBoard = JSON.parse(solutionDataDiv.dataset.solution);
    }

    const inputs = document.querySelectorAll('.sudoku-input');
    inputs.forEach(function (input) {
        input.addEventListener('blur', function () {
            let row = parseInt(input.getAttribute('data-row'), 10);
            let col = parseInt(input.getAttribute('data-col'), 10);
            let userInput = input.value.trim();

            if (userInput === "") {
                return;
            }

            let num = parseInt(userInput, 10);
            if (isNaN(num) || num < 1 || num > 9) {
                alert("Please enter a number between 1 and 9.");
                input.value = "";
                return;
            }

            if (num !== solutionBoard[row][col]) {
                alert("Incorrect value for this square!");
                input.value = "";
                return;
            }

            let span = document.createElement("span");
            span.classList.add("fixed-cell");
            span.innerText = num;
            input.parentNode.replaceChild(span, input);

            checkCompletion();
        });
    });

    let toggleBtn = document.getElementById('toggleSolutionBtn');
    let solutionDiv = document.getElementById('solutionDiv');
    const boardsContainer = document.querySelector('.boards-container');

    if (toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            if (solutionDiv.style.display === 'none') {
                solutionDiv.style.display = 'block';
                toggleBtn.innerText = 'Hide Solution';
                boardsContainer.style.justifyContent = 'space-around';
            } else {
                solutionDiv.style.display = 'none';
                toggleBtn.innerText = 'Show Solution';
                boardsContainer.style.justifyContent = 'center';
            }
        });
    }

    function checkCompletion() {
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
            const fixedSpan = cell.querySelector('span.fixed-cell');
            if (!fixedSpan) return;

            cells.forEach(function (c) {
                c.classList.remove('selected-cell', 'same-number-cell', 'highlighted-line');
            });

            const selectedRow = parseInt(cell.getAttribute('data-row'), 10);
            const selectedCol = parseInt(cell.getAttribute('data-col'), 10);
            const selectedValue = fixedSpan.innerText.trim();

            cell.classList.add('selected-cell');

            cells.forEach(function (c) {
                let currentRow = parseInt(c.getAttribute('data-row'), 10);
                let currentCol = parseInt(c.getAttribute('data-col'), 10);
                if ((currentRow === selectedRow || currentCol === selectedCol) && c !== cell) {
                    c.classList.add('highlighted-line');
                }
                let sp = c.querySelector('span.fixed-cell');
                if (c !== cell && sp && sp.innerText.trim() === selectedValue) {
                    c.classList.add('same-number-cell');
                }
            });
        });
    });
});
