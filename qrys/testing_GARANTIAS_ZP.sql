/*
truncate table DOCUMENTO_GARANTIA
truncate table GARANTIA_PETICION
*/

select * from DOCUMENTO_GARANTIA
select * from GARANTIA_PETICION
select *
from (
	select IIF(pli.FOLIO_PLIEGO is null, CONCAT('PLG-',pli.CLAVE_ZP,'-',pli.ID_PLIEGO), pli.FOLIO_PLIEGO) FOLIO_PLIEGO,
		cat_pet.DESCRIPCION_CAT_PETICION, pet.DESC_PETICION,
		gar.DESC_GARANTIA
	from GARANTIA_PETICION gar
	inner join PLIEGO pli on pli.ID_PLIEGO = gar.ID_PLIEGO
	inner join CAT_CATEGORIA_PETICION  cat_pet on cat_pet.ID_CAT_PETICION = gar.ID_CAT_PETICION
	inner join PETICIONES_POR_UA pet on pet.ID_PETICION = gar.ID_PETICION
	where gar.CLAVE_ZP like '%'
)datos


select * from PLIEGO

--IIF(pli.FOLIO_PLIEGO is null, CONCAT('PLG-',pli.CLAVE_ZP,'-',pli.ID_PLIEGO), pli.FOLIO_PLIEGO) FOLIO_PLIEGO