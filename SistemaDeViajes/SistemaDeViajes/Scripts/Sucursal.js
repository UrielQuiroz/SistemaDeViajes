
$(document).ready(function () {
    $('#tabla').DataTable({
        "searching": false,
        lengthMenu: [[5, 10, 15, 20, 25, 50, -1], [5, 10, 15, 20, 25, 50, "Todos"]],
    });
});



var nombre = document.getElementById("Nombre")
nombre.onkeyup = function () {
    var frmSearch = document.getElementById("frmSearch");
    frmSearch.submit();
}




function Delete() {
    if (confirm("Desea eliminar el registro?") == 1) {
        alert("Se ha eliminado");
    }
    else {
        event.preventDefault();
    }
}




var frmEdit = document.getElementById("frmEdit");
frmEdit.onsubmit = function () {
    event.preventDefault();
    if (confirm("¿Desea actualizar la informacion?") == 1) {
        frmEdit.submit();
    }
    else {
        alert("Cancelada");
    }
}



var frmAdd = document.getElementById("frmAdd");
frmAdd.onsubmit = function () {
    event.preventDefault();
    if (confirm("¿Desea agregar la informacion?") == 1) {
        frmAdd.submit();
        alert("Exito");
    }
    else {
        alert("Cancelada");
    }
}
