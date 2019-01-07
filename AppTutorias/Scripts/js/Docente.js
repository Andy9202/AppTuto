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



//INGRESAR TUTORIA DEL DOCENTE

//ingresarTutoria


function ingresarTutoriasDocente() {


    var horarioTutorias = {
        Matricula: $("#listaEstudiantes").val(),
        CodigoModulo: $("#listaModulos").val(),
		CodigoMateria: $("#listaMateria").val(),
		Fecha: $("#fecha").val(),
        CodigoPeriodo: $("#listaPeriodo").val(),
		EstadoObligatorio: $("#listaObligatorio").val(),
		Aula: $("#aulaTuto").val()


	};

	$('#tablaHorarioTutorias').html(
		'<tr id="modalLoad" class="loadingTable"><td><span><i class="fas fa-spinner fa-spin"></i> Cargando...</span></td></tr>'
	);



    $.ajax({
        url: "/Docente/ingresarTutorias",
        data: JSON.stringify(horarioTutorias),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (listaHorariosTutorias) {

            $("#tablaHorarioTutorias").empty();



            var d;
            var a = 0;
            for (var i = 0; i < listaHorariosTutorias.length; i++) {

                var datos = "'" + listaHorariosTutorias[i].Id_HorarioTutorias + "'";

                d += '<tr>' +
                    '<td>' + listaHorariosTutorias[i].Matricula + '</td>' +
                    '<td>' + listaHorariosTutorias[i].NombreEstudiante + '</td>' +
                    '<td>' + listaHorariosTutorias[i].CodigoMateria + '</td>' +
                    '<td>' + listaHorariosTutorias[i].NombreMateria + '</td>' +
                    '<td>' + listaHorariosTutorias[i].Modulo + '</td>' +
                    '<td>' + listaHorariosTutorias[i].CodigoPeriodo + '</td>' +
					'<td>' + listaHorariosTutorias[i].EstadoObligatorio + '</td>' +
					'<td>' + listaHorariosTutorias[i].Fecha + '</td>' +
					'<td>' + listaHorariosTutorias[i].Aula + '</td>' +
					'<td>' + listaHorariosTutorias[i].Asistencia + '</td>' +

					'<td>' + '<button   onclick="confirmarAsistencia(' + datos + ')" ></i> Confirmar Asistencia</button>' + '</td>' +
					'</tr>';

                a++;
            }



			$("#tablaHorarioTutorias").append(d);

	

        },
        error: function () {

        }
    });
}





