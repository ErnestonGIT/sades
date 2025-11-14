--select au.CLAVE_ZP, au.ID_PERFIL, au.ID_USER, dp.DESCRIPCION_DP, per.DESCRIPCION,us.APELLIDO_PAT,us.APELLIDO_MAT,us.NOMBRE, CONCAT(us.APELLIDO_PAT,' ',us.APELLIDO_MAT,' ',us.NOMBRE) NOMBRE_COMPLETO, FORMAT(au.FECHA_INICIO, 'dd/MM/yyyy', 'es-ES') FECHA_INICIO, 
select au.CLAVE_ZP, au.ID_PERFIL, au.ID_USER, dp.DESCRIPCION_DP, per.DESCRIPCION, CONCAT(us.APELLIDO_PAT,' ',us.APELLIDO_MAT,' ',us.NOMBRE) NOMBRE_COMPLETO, 
            (select CONCAT(CLAVE_ZP,'_',(FORMAT(FECHA_INICIO,'yyyy')),'_',ID_PERFIL,'_',ID_USER,'.pdf') NOMBRAMIENTO from AUTORIDADES_ZP where ID_USER= au.ID_USER) PDF 
                        from AUTORIDADES_ZP au 
                            inner join CAT_DEPENDENCIAS_POLITECNICAS dp on dp.CLAVE_ZP = au.CLAVE_ZP 
                            inner join CAT_PERFILES per on per.ID_PERFIL = au.ID_PERFIL 
                            inner join USERS us on us.ID_USER = au.ID_USER 
                        where au.ESTATUS = 1-- and au.CLAVE_ZP like '1873%'
                        order by dp.DESCRIPCION_DP asc

--select * from AUTORIDADES_ZP where CLAVE_ZP='1735'

/*

update AUTORIDADES_ZP
set FECHA_INICIO ='2020-03-15',
FECHA_FIN = '2023-03-15'
where ID_USER = '5333'

update AUTORIDADES_ZP
set OBSERVACION ='prórroga hasta en tanto concluya el proceso de elección de terna.'
where ID_USER = '5333'

*/

--select * from AUTORIDADES_ZP where OBSERVACION like '%pró%'
