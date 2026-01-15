SELECT 
    schema_name(schema_id) AS schema_name, 
    name AS table_name, 
    create_date, 
    modify_date 
FROM sys.tables 
WHERE create_date > DATEADD(DAY, -40, CURRENT_TIMESTAMP) 
ORDER BY create_date DESC;

DBCC FREEPROCCACHE WITH NO_INFOMSGS
DBCC DROPCLEANBUFFERS WITH NO_INFOMSGS


--EXEC sp_rename 'AUTORIDADES_ZP.INTERINO', 'TIPO', 'COLUMN';

--EXEC sp_addextendedproperty 
--    @name = N'MS_Description', 
--    @value = N'1 -> interinato, 2 -> prórroga, 3 -> titular',
--    @level0type = N'SCHEMA', 
--    @level0name = N'dbo', 
--    @level1type = N'TABLE', 
--    @level1name = N'AUTORIDADES_ZP', 
--    @level2type = N'COLUMN', 
--    @level2name = N'TIPO';

-- alter table garantia_peticion add FECHA_REALIZACION date NULL 
-- alter table garantia_peticion add ID_SUBCAT_PETICION int NULL 
-- alter table garantia_peticion drop column ID_SUBCAT_PETICION
-- alter table ASIGNACION_PETICION add ID_SUBCAT_PETICION int NULL
-- alter table GARANTIA_PETICION drop column CLAVE_ZP, ID_PLIEGO, ID_CAT_PETICION
-- alter table garantia_peticion add ID_PLIEGO int NULL 
-- alter table PETICIONES add ID_SUBCAT_PETICION int NULL

--alter table autoridades_zp drop column NOMBRAMIENTO;
--alter table autoridades_zp add NOMBRAMIENTO nvarchar(100) NULL;

ALTER TABLE [dbo].[AUTORIDADES_ZP] ADD  CONSTRAINT [DF_AUTORIDADES_ZP_ESTATUS]  DEFAULT ((1)) FOR [ESTATUS]
GO
ALTER TABLE [dbo].[AUTORIDADES_ZP] ADD  CONSTRAINT [DF_AUTORIDADES_ZP_FECHA_REGISTRO]  DEFAULT ((CURRENT_TIMESTAMP)) FOR [FECHA_REGISTRO]
GO


select * from USERS_PERFIL where ID_USER = '11828'
select * from USERS where CONCAT(NOMBRE,' ', APELLIDO_PAT) like 'tomas huerta%'
select * from users where USERNAME like 'enlac%'

select * from CAT_SUBCATEGORIA_PETICION
select * from CAT_CATEGORIA_PETICION
select * from PETICIONES pet inner join PLIEGO pli on pli.ID_PLIEGO = pet.ID_PLIEGO where pli.CLAVE_ZP = '1751'
select * from GARANTIA_PETICION -- truncate table GARANTIA_PETICION
select * from DOCUMENTO_GARANTIA -- truncate table DOCUMENTO_GARANTIA
select * from ASIGNACION_PETICION -- truncate table ASIGNACION_PETICION

--update PETICIONES set ID_SUBCAT_PETICION = '2' where ID_PETICION in(334)
select * from AUTORIDADES_ZP where CLAVE_ZP = '1751'

select DATEDIFF(month, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN) % 12 MESES from AUTORIDADES_ZP where CLAVE_ZP like '1751' and ID_PERFIL = '11' and ESTATUS = '1'
select DATEDIFF(day, (select FORMAT(GETDATE(),'yyyy-M-dd')),FECHA_FIN) dias from AUTORIDADES_ZP where CLAVE_ZP like '1751' and ID_PERFIL = '11' and ESTATUS = '1'
/*
insert into AUTORIDADES_ZP 
(CLAVE_ZP, ID_PERFIL, ID_USER, CORREO, CELULAR, EXTENSION, FOTO, FECHA_INICIO, FECHA_FIN, OBSERVACION, TIPO) 
values ('1751', '11', '12780', 'ediazg@ipn.mx', '5561838737', '50021', 'sin_foto.jpg', '2025-12-01', '2025-12-01', 'observacion', 1)
*/
-- UPDATE AUTORIDADES_ZP SET ESTATUS = 1 WHERE CLAVE_ZP ='1751' AND ID_USER = '12780'
-- UPDATE AUTORIDADES_ZP SET ESTATUS = 1, ID_USER ='11828' WHERE CLAVE_ZP ='1751' AND CORREO = 'ediazg@ipn.mx'
-- DELETE FROM AUTORIDADES_ZP WHERE CLAVE_ZP = '1751' AND ID_USER = '11828' or correo = 'ediazg@ipn.mx'

select count(ID_USER) total from AUTORIDADES_ZP where CLAVE_ZP = '1751' and ID_PERFIL = '11' and ESTATUS = '1'

select ID_USER from USERS where NUMERO_EMPLEADO = '2300675'
insert into AUTORIDADES_ZP (CLAVE_ZP, ID_PERFIL, ID_USER, CORREO, CELULAR, EXTENSION, FOTO, FECHA_INICIO, FECHA_FIN, OBSERVACION, TIPO) values ()

/*	Insertcion prueba DES	*/
--	insert into GARANTIA_PETICION (ID_GARANTIA,CLAVE_ZP, ID_PLIEGO,ID_CAT_PETICION,ID_PETICION,DESC_GARANTIA,ID_DOCUMENTO, ESTATUS, FECHA_REGISTRO) values('1','1751','12','2','332','Se solicita mantenimiento del camión a la dirección de servicios generales','1','1','2025-11-21 15:06:04.713')
--  insert into GARANTIA_PETICION (ID_GARANTIA, ID_PLIEGO,ID_PETICION,DESC_GARANTIA,ID_DOCUMENTO, ESTATUS, FECHA_REGISTRO) values('1','12','332','Se solicita mantenimiento del camión a la dirección de servicios generales','1','1','2025-11-21 15:06:04.713')
--	insert into DOCUMENTO_GARANTIA (TIPO_DOCUMENTO, RUTA_DOCUMENTO, FECHA_REGISTRO) OUTPUT INSERTED.ID_DOCUMENTO  values('.pdf','/public/src/garantias/1751/GAR-2025-2-85b.pdf','2025-11-21 15:06:04.710')

select IIF(FECHA_RESP_PETICION is null, 'sin dato registrado', FORMAT(FECHA_RESP_PETICION ,'dddd dd MMMM, yyyy', 'es-ES'))LIMITE from PETICIONES where ID_PETICION = '332'

select DESC_PETICION from PETICIONES where ID_PETICION = '332'


select * from USERS where USERNAME like '%2300675%'
select * from USERS_PERFIL where ID_USER = '11828'

select * from CAT_PERFILES


/*
                
select top 1 (ID_USER + 1) ID_USER from users order by ID_USER desc

INSERT INTO USERS (ID_USER, USERNAME, PASSWORD, NUMERO_EMPLEADO, NOMBRE,APELLIDO_PAT, APELLIDO_MAT, CURP, RFC, HOMOCLAVE, CORREO_INSTITUCIONAL, CORREO_PERSONAL, DOMICILIO, GENERO, ESTATUS)
			VALUES('66666669', 'kantonio', 'k4ntonio', '66666669', 'KARLA', 'ANTONIO', 'VARGAS', '', '', '', 'kantonio@ipn.mx', 'kantonio@ipn.mx', '', 'F', '1')

insert into USERS_PERFIL (ID_USER, ID_PERFIL, ESTATUS) 
				   values('66666669','7','1')

insert into USER_ZP (CLAVE_ZP, ID_USER) 
			  values('1031','66666669')

*/
