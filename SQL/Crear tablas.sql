Create Table Administrador(
	idAdministrador int Primary Key Identity,
	correo nvarchar(100),
	nombre nvarchar(255)
);
--<ISSUE 1> No se pudo usar autenticaci? de uacj.mx, se guardar?la contrasena directamente en la BD
		Alter table Administrador Add contrasena nvarchar(100)
--</ISSUE 1>
Create Table TipoPrestamo(
	idTipoPrestamo int Primary Key Identity,
	nombrePrestamo nvarchar(255) not null,
	descripcion nvarchar(255),
	creadoPor int Foreign Key References Administrador(idAdministrador)
);
--<ISSUE 3> Si se borra una pregunta se perder?el "titulo", se opt?por simplemente ocultarlas y de esta forma no afecte al reporte
		Alter table TipoPrestamo Add visible bit default 1
--</ISSUE 3>
Create Table Alumno(
	idAlumno int Primary Key Identity,
	matricula int,
	completoCuestionario bit default 0
);


Create Table Prestamo(
	idPrestamo int Primary Key Identity,
	idAlumno int Foreign Key References Alumno(idAlumno),
	idTipoPrestamo int Foreign Key References TipoPrestamo(idTipoPrestamo),
	fecha datetime Default GetDate()
);

Create Table Pregunta(
	idPregunta int Primary Key Identity,
	titulo nvarchar(255),
	tipoDeDato nvarchar(50),
	respuestas nvarchar(2000),
	fechaCreacion datetime Default GetDate(),
	creadaPor int Foreign Key References Administrador(idAdministrador)
);
--<ISSUE 2> Si se borra una pregunta se perder?el "titulo", se opt?por simplemente ocultarlas y de esta forma no afecte al reporte
		Alter table Pregunta Add visible bit default 1
--</ISSUE 2>
Create Table Respuesta(
	idRespuesta int Primary Key Identity,
	idPregunta int Foreign Key References Pregunta(idPregunta),
	respuesta nvarchar(255),
	idAlumno int Foreign Key References Alumno(idAlumno),
	fechaRespuesta datetime Default GetDate()
);

Create Table datosHistoricos(
	idRegistro int primary key identity, 
	idPregunta int, 
	respuesta nvarchar(255),
	idAlumno int,
	fechaRespuesta datetime,
	fechaBorrado datetime default getdate()
)