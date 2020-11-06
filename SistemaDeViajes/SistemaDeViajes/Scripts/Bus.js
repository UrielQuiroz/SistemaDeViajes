


$(document).ready(function () {
    $('#tabla').DataTable({
        "searching": false,
        lengthMenu: [[5, 10, 15, 20, 25, 50, -1], [5, 10, 15, 20, 25, 50, "Todos"]],
    });
});

var frmSearch = document.getElementById("frmSearch");

var controles = document.getElementsByClassName("form-control");
var NumControles = controles.length;

for (var i = 0; i < NumControles; i++) {
    controles[i].onchange = function () {
        frmSearch.submit();
    }
}

function Delete() {

    if (confirm("Desea eliminar el registro?") == 1) {
        alert("Se ha eliminado");
    }
    else {
        event.preventDefault();
    }

}



var frmAdd = document.getElementById("frmAdd");
frmAdd.onsubmit = function (e) {
    e.preventDefault();
    if (confirm("Desea ageragr el registro")) {
        frmAdd.submit();
    }
}