DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX);

SET @cols = STUFF((SELECT distinct ',' + QUOTENAME(pre.titulo) 
            FROM Pregunta pre
			Where visible = 1
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')



set @query = 'SELECT idAlumno, ' + @cols + ' from 
            (
                select idAlumno
                    , respuesta
                from Respuesta
           ) x 
            pivot 
            (
                 max(respuesta)
                for respuesta in (' + @cols + ')
            ) p 
			'
    
	execute(@query)
	

	