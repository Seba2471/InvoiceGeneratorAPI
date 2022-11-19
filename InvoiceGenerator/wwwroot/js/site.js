// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteItem(event) {
    const lineNumber = event.id.split('-')[1];
    const row = document.getElementById(`row-${lineNumber}`);
    const table = document.getElementById("tableBody");
    const rows = table.querySelectorAll("tr");

    rows.forEach(row => {
        const rowLineNumber = row.id.split('-')[1];
        if (rowLineNumber > lineNumber) {
            row.id = `row-${rowLineNumber - 1}`;
            row.querySelectorAll('th')[0].innerHTML = rowLineNumber - 1;
            console.log(row.querySelectorAll('th')[5].querySelector('button').id = `deleteButton-${rowLineNumber - 1}`);
        }
    })

    row.remove();

    counter = counter - 1;
}


function getDateInputValue(inputId) {
    return document.getElementById(inputId).valueAsDate;
}