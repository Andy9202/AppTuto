

//----------INGRESAR NUMERO DE MATRICULA DE LA MATERIA DEL ESTUDIANTE---------------------------------------
//--------------------------------------------------------------------------------------------
function repitencia() {
    $.ajax({
        url: "/SuperAdmin/repitencia",
        type: "POST",
        datatype: 'application/json',
        success: function (data) {

            $("#moduloRepitencia").html(data);
        },
        error: function () {

        }
    });
}

