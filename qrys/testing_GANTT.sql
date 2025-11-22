select * from PETICIONES

/*
DECLARE @StartDate AS DATE = '2023-01-01';
DECLARE @EndDate AS DATE = '2023-12-31';

SELECT DATEADD(DAY, RAND(CHECKSUM(NEWID())) * (1 + DATEDIFF(DAY, @StartDate, @EndDate)), @StartDate) AS RandomDate;

	start: '2017-12-01',
	end: '2018-02-02',
	completed: {
		amount: 0.95
	},
	name: 'Prototyping'
*/


--json_query(QUOTENAME(STRING_AGG('"' + STRING_ESCAPE(item_id, 'json') + '"', char(44)))) as [json]
select * from PETICIONES

select FORMAT(FECHA_PETICION,'yyyy-MM-dd') 'start', FORMAT(FECHA_RESP_PETICION,'yyyy-MM-dd') 'end', completed.amount 'completed.amount', 
SUBSTRING(PETICIONES.DESC_PETICION, 0, 15) 'name'
from PETICIONES 
inner join (select SUBSTRING(CAST(RAND() as nvarchar), 0, 5) amount) completed on completed.amount is not null 
inner join PLIEGO pli on pli.ID_PLIEGO = PETICIONES.ID_PLIEGO 
where FECHA_PETICION is not null and FECHA_RESP_PETICION is not null and pli.CLAVE_ZP like '1751%'
for json path

WAITFOR DELAY '00:00:00:01'

select RAND() WAITFOR DELAY '00:00:00:01'

select SUBSTRING(CAST(RAND() as nvarchar), 0, 5)WAITFOR DELAY '00:00:00:01'

  select * from PETICIONES

  update peticiones 
  set DESC_RESP_PETICION = 'Respuesta a petición infraestructura',
  FECHA_RESP_PETICION = '2025-11-23'
  where ID_PETICION=331