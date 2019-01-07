




/*MENU DESPLEGABLE*/
$("#btnMenu").on("click", () => {
	if ($("#menu").hasClass("inactive-menu")) {
		$("#menu").removeClass("inactive-menu");
		$("#menu").addClass("active-menu");
	} else {
		$("#menu").removeClass("active-menu");
		$("#menu").addClass("inactive-menu");
	}
});

$(window).resize(function () {
	if ($(window).width() < 769) {
		$("#menu").addClass("inactive-menu");
	} else {
		$("#menu").removeClass("active-menu");
	}
});



//*MENU NAVEGACION*/

//perfilEstudiante
$("#perfilEstudiante").on('click', () => {
	mostrarLoading();

	$.ajax({
		url: "/Estudiante/PerfilEstudiante",
		type: 'post',
		success: function (data) {

			

			$("#app").html(data);
			$("#menu").removeClass("active-menu");
			$("#menu").addClass("inactive-menu");



		},
		error: function () {

		}
	});
});

//tutorias estudiante

//perfilEstudiante
$("#tutoriasEstudiante").on('click', () => {

	mostrarLoading();
	$.ajax({
		url: "/Estudiante/TutoriasEstudiante",
		type: 'post',
		data: { },
		success: function (data) {



			$("#app").html(data);
		
			$("#menu").removeClass("active-menu");
			$("#menu").addClass("inactive-menu");



		},
		error: function () {

		}
	});
});


//horarioEstudiante
$("#horarioEstudiante").on('click', () => {
	mostrarLoading();

	$.ajax({
		url: "/Estudiante/moduloHorarioEstudiante",
		type: 'post',
		data: {},
		success: function (data) {



			$("#app").html(data);

			$("#menu").removeClass("active-menu");
			$("#menu").addClass("inactive-menu");



		},
		error: function () {

		}
	});
});



//modal de carga
function mostrarLoading() {

	$('#app').empty();

	$('#app').html(
		'<div id="modalLoad" class="modalLoading"><div><img src="/Content/img/logotipo.png" alt="" width="97px" /><br /><span><i class="fas fa-spinner fa-spin"></i> Cargando...</span></div></div>'
	);


}