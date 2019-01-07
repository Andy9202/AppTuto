//-----------------------------------------------------------------ADMIN-----------------------
//-------------------------------------------------------------------------------------------------


//TUTORIAS-----------------------
//--------------------------------------------------
//facultad
function tutorias() {
    $.ajax({
        url: "/Admin/Tutorias",
        type: 'post',
        datatype: 'application/json',
        success: function (data) {



            $("#app").html(data);



        },
        error: function () {

        }
    });
}


//ingresarTutoria


function ingresarTutorias() {

  
    var horarioTutorias = {
        Matricula: $("#listaEstudiantes").val(),
        CedulaDocente: $("#listaDocentes").val(),
        CodigoModulo: $("#listaModulos").val(),
        CodigoMateria: $("#listaMateria").val(),
        CodigoPeriodo: $("#listaPeriodo").val(),
        EstadoObligatorio: $("#listaObligatorio").val()


    };


    $.ajax({
        url: "/Admin/ingresarTutorias",
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
                    '<td>' + listaHorariosTutorias[i].CedulaDocente + '</td>' +
                    '<td>' + listaHorariosTutorias[i].NombreDocente + '</td>' +
                    '<td>' + listaHorariosTutorias[i].CodigoMateria + '</td>' +
                    '<td>' + listaHorariosTutorias[i].NombreMateria + '</td>' +
                    '<td>' + listaHorariosTutorias[i].Modulo + '</td>' +
                    '<td>' + listaHorariosTutorias[i].CodigoPeriodo + '</td>' +
                    '<td>' + listaHorariosTutorias[i].EstadoObligatorio + '</td>' +
                    //'<td>' + '<a type="button"  onclick="eliminarHorarioClasesEstudiantes(' + datos + ')" ></i> Eliminar</a>' + '</td>' +
                    '</tr>';

                a++;
            }



            $("#tablaHorarioTutorias").append(d);

        },
        error: function () {

        }
    });
}

