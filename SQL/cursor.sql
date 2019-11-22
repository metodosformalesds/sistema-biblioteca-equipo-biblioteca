/****** Object:  StoredProcedure [dbo].[SP_Select_dataset]    Script Date: 11/22/2019 12:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    ALTER Procedure [dbo].[SP_Select_dataset]
	As
	SET FMTONLY OFF
	--Se crea tabla temporal donde se alojaran los datos
	Create Table #temp ( IdAlumno int );


    --Por cada pregunta activa, se creara una columna y se agregara a la tabla temporal
    Declare @alter nvarchar(MAX) = N'ALTER TABLE #temp ADD ';


	--Esto nos permite convertir cada pregunta en una columna y agregarle el tipo de dato
	--en este caso se trabajara toda la tabla como si fueran nvarchar(255).
    Set @alter += Stuff((   Select   ',' + QuoteName(p.titulo) + ' nvarchar(255)'
                            From     Pregunta AS p
                            Where    p.visible = 1
							Order By idPregunta
                            For XML Path(N''), Type ).value(N'.[1]', N'NVARCHAR(4000)'), 1, 1, N'')

	--Se ejecuta el query que agrega las columnas a nuestra tabla temporal
	Exec ( @alter );

	--Se pobla la tabla temporal con los id's de los alumnos
	--Esto nos permitirá hacer una búsqueda más adelante
	--Se agrego columna 'congelado'
	insert into #temp (idAlumno)select idAlumno from alumno where FueBorrado = 0
	

	--Variables auxiliares
	Declare @idAlumno nvarchar(255)
	Declare @titulo nvarchar(255)
	Declare @respuesta nvarchar(255)
	Declare @query nvarchar(255)


	--Se crea cursor para iterar en cada uno de los id's previamente insertados en nuestra tabla temporal
	Declare dataset Cursor For   

	--Obtenemos cada una de las respuestas que han dado los alumnos a las preguntas 'activas'
    Select respuesta.idAlumno, titulo, respuesta From pregunta 
	Inner Join respuesta on pregunta.idPregunta = respuesta.idPregunta
	Where visible = 1 --visible es el atributo con el que sabemos si una pregunta esta activa o no
	Order By respuesta.idAlumno, pregunta.idPregunta
  
    Open dataset  
	--En las variables auxiliares guardaremos
	--@idAlumno para hacer la búsqueda
	--@titulo es la columna en la que insertaremos los datos
	--@respuesta ... pues la respuesta del alumno xd
    Fetch Next From dataset Into @idAlumno, @titulo, @respuesta 
  
    --Comienza la iteracion
    While @@FETCH_STATUS = 0  
    Begin  
		--Sea crea el comando, que se veria algo asi como 'Update temp set Instituto = 'CU' Where idAlumno = '1' '
		Set @query = 'Update #temp set [' + @titulo + '] = ''' + @respuesta + ''' where idAlumno = ''' + @idAlumno + ''''
		--Se ejecuta el comando
		exec ( @query )
		
		--Se sigue iterando
        Fetch Next From dataset Into @idAlumno, @titulo, @respuesta   
        End  
    Close dataset  
    Deallocate dataset 

	--Finalmente tenemos una tabla dinamica pivoteada
	--de tal forma que podemos ver las respuestas de los alumnos en cada 
	--una de las preguntas, esto nos facilita exportar los datos y hacer los reportes

	Select * From #temp

	Drop Table #temp


	

