var cells = document.querySelectorAll('.gridview-style td');
cells.forEach(function (cell, columnIndex) {
    if (columnIndex < 2) {
        cell.addEventListener('click', function () {
            var input = document.createElement('input');
            if (columnIndex == 0) input.type = 'text';
            if (columnIndex == 1) input.type = 'password';
            input.value = "";
            cell.innerHTML = '';
            cell.appendChild(input);
            //input.style.width = 150 + 'px';
            input.style.height = 25 + 'px';
            input.focus();
            input.addEventListener('blur', function () {
                let newValue = input.value;
                if (columnIndex != 1) cell.textContent = newValue;
                else for (let i = 0; i < newValue.length; i++) cell.textContent = newValue.replace(/./g, '*');
                if (columnIndex == 0) document.getElementById('UsernameLabel').innerText = newValue;
                $.ajax({
                    type: "POST",
                    url: "Userpage.aspx/TextChanged",
                    data: JSON.stringify({ column: columnIndex, newValue: input.value }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        console.log(response.d);
                    },
                    error: function (error) {
                        console.log("Error: " + error.responseText);
                    }
                });
            });
        });
    }
});