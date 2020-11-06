$(document).ready(function () {
    $('#tablaViajes').DataTable({
        "searching": false,
        lengthMenu: [[5, 10, 15, 20, 25, 50, -1], [5, 10, 15, 20, 25, 50, "Todos"]],
    });
});


var frmAdd = document.getElementById("frmAdd");
frmAdd.onsubmit = function (e) {
    e.preventDefaut();
    if (confirm("Desea agregar el registro")) {
        frmAdd.submit();
    }
    else {
        alert("Cancelo");
    }
}

var frmEdit = document.getElementById("frmEdit");
frmEdit.onsubmit = function (e) {
    e.preventDefaut();
    if (confirm("Desea actualizar el registro")) {
        frmEdit.submit();
    }
    else {
        alert("Cancelo");
    }
}