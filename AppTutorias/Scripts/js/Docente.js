//validar campos login

var email = document.getElementById('correo');
var pass = document.getElementById('contrasena');


function validoLogin() {

    var valido = false;

    if (email.value !== "") {

        email.style.border = "1px solid rgba(255, 94,0,0.5)";

    } else {

        email.style.border = "1px solid red";
    }
    if (pass.value !== "") {

        pass.style.border = "1px solid rgba(255, 94,0,0.5)";

    } else {

        pass.style.border = "1px solid red";
    }

    if (email.value !== "" && pass.value !== "") {

        valido = true;
    }



    return valido;


}



/**
 * /
 */

function EditarDocente() {
    $.ajax({
        url: "/Docente/editarDocente",
        type: 'post',
        datatype: 'application/json',
        success: function (data) {



            $("#app").html(data);



        },
        error: function () {

        }
    });
}

function ActualizarPerDocente() {


    $.ajax({
        url: "/Docente/actualizarDocente",
        data: JSON.stringify({ Celular: $("#editCelular").val() }),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType:"json",
        success: function (mensaje) {

            PerfilDocente();

        },
        error: function () {
            alert('algo paso');
        }
    });







}


//TUTORIA DEL DOCENTE

function tutoriaEstudianteDocente() {
    $.ajax({
        url: "/Docente/moduloTutoria",
        type: 'post',
        datatype: 'application/json',
        success: function (data) {



            $("#tutoriaDocente").html(data);

         

        },
        error: function () {

        }
    });
}








