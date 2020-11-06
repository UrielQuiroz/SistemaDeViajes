

$(document).ready(function () {
    $('#tabla').DataTable({
        "searching": false,
        lengthMenu: [[5, 10, 15, 20, 25, 50, -1], [5, 10, 15, 20, 25, 50, "Todos"]],
    });
});

var user = document.getElementById("IDTipoUser");
user.onchange = function () {
    var frmSearch = document.getElementById("frmSearch");
    frmSearch.submit();
}

function Delete() {

    if (confirm("Desea eliminar el registro") == 1) {
        alert("Se ha eliminado");
    }
    else {
        event.preventDefault();
    }
}

var frmAdd = document.getElementById("frmAdd");
frmAdd.onsubmit = function (e) {
    e.preventDefault();
    if (confirm("¿Desea actualizar el registro?") == 1) {
        frmAdd.submit();
    }
    else {
        alert("Cancelo");
    }
}