--select * from AUTORIDADES_ZP

select au.CLAVE_ZP, au.ID_PERFIL, au.ID_USER, dp.DESCRIPCION_DP, per.DESCRIPCION,us.APELLIDO_PAT,us.APELLIDO_MAT,us.NOMBRE, CONCAT(us.APELLIDO_PAT,' ',us.APELLIDO_MAT,' ',us.NOMBRE) NOMBRE_COMPLETO, FORMAT(au.FECHA_INICIO, 'dd/MM/yyyy', 'es-ES') FECHA_INICIO, FORMAT(au.FECHA_FIN, 'dd/MM/yyyy', 'es-ES') FECHA_FIN, 
case 
	when (DATEDIFF(day, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN)) >= 1 then 'VIGENTE'  
    when (DATEDIFF(day, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN)) <= 0 then 'VENCIDO'  
end NOMBRAMIENTO_EST, 
(select CONCAT(CLAVE_ZP,'_',(FORMAT(FECHA_INICIO,'yyyy')),'_',ID_PERFIL,'_',ID_USER,'.pdf') NOMBRAMIENTO from AUTORIDADES_ZP where ID_USER= au.ID_USER) PDF ,
OBSERVACION
from AUTORIDADES_ZP au 
    inner join CAT_DEPENDENCIAS_POLITECNICAS dp on dp.CLAVE_ZP = au.CLAVE_ZP 
    inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL 
    inner join USERS us on us.ID_USER = au.ID_USER 
where au.ESTATUS = 1-- and CONCAT(us.APELLIDO_PAT,' ',us.APELLIDO_MAT,' ',us.NOMBRE) like '%Doray%'-- and OBSERVACION like '%pró%'
order by dp.DESCRIPCION_DP asc

/*Mario Alberto*/
/*

update AUTORIDADES_ZP
set FECHA_INICIO = '2025-03-16',
	FECHA_FIN ='2028-03-16'
where ID_USER = '5016'

update AUTORIDADES_ZP
	set OBSERVACION = 'prórroga en tanto se designe a la persona que estará al frente de dicho cargo'
where ID_USER = '5016'

ALTER TABLE AUTORIDADES_ZP ALTER COLUMN OBSERVACION nvarchar(100)
*/

--select * from AUTORIDADES_ZP where OBSERVACION like '%pró%'